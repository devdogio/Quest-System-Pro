using System;
using System.Collections.Generic;
using Devdog.General.ThirdParty.UniLinq;
using Devdog.General;
using Devdog.General.Editors;
using Devdog.General.Editors.GameRules;
using Devdog.General.Localization;
using UnityEngine;
using UnityEditor;
using Debug = UnityEngine.Debug;
using EditorStyles = UnityEditor.EditorStyles;
using EditorUtility = UnityEditor.EditorUtility;

namespace Devdog.QuestSystemPro.Dialogue.Editors
{
    public class DialogueEditorWindow : BetterEditorWindow
    {
        private Dialogue _dialogue;
        public Dialogue dialogue
        {
            get { return _dialogue; }
            set
            {
                if (ReferenceEquals(_dialogue, value))
                {
                    return; // Same, no need to change
                }

                if(_dialogue != null)
                {
                    _dialogue.OnCurrentNodeChanged -= Dialogue_OnCurrentNodeChanged;
                }

                _dialogue = value;
               
                if (_dialogue != null)
                {
                    ValidateDialogue(_dialogue);
                    _dialogue.OnCurrentNodeChanged += Dialogue_OnCurrentNodeChanged;
                }

                NotifyNodeEditorsChanged();
                if (DialogueManager.instance != null)
                {
                    DialogueManager.instance.SetCurrentDialogue(_dialogue, dialogueOwner);
                }
            }
        }

        private IDialogueOwner _dialogueOwner;
        public IDialogueOwner dialogueOwner
        {
            get { return _dialogueOwner; }
            set
            {
                _dialogueOwner = value;
                dialogue = _dialogueOwner != null ? _dialogueOwner.dialogue : null;

                if (DialogueManager.instance != null)
                {
                    DialogueManager.instance.SetCurrentDialogue(dialogue, _dialogueOwner);
                }
            }
        }

        public List<NodeEditorBase> nodeEditors = new List<NodeEditorBase>();

        private List<NodeEditorBase> _selectedNodeEditors = new List<NodeEditorBase>();
        public List<NodeEditorBase> selectedNodeEditors
        {
            get { return _selectedNodeEditors; }
            protected set
            {
                foreach (var node in _selectedNodeEditors)
                {
                    node.UnSelect();
                }

                value.RemoveAll(o => o == null);
                _selectedNodeEditors = value;

                foreach (var node in _selectedNodeEditors)
                {
                    node.Select();
                }
            }
        }

        public static DialogueEditorWindow window { get; protected set; }
        private Vector2 _dragStartPosition;

        private bool _isDraggingNode = false;
        private bool _isDraggingEdge = false;
        private bool _isDraggingSidebarWidth = false;
        private EdgeConnector _dragEdgeConnector;
        private EdgeConnector _connectToNextNode;
        //private NodeEditorBase _dragEdgeStartNode;
        //private uint _dragEdgeStartEdgeConnectorIndex;
        private bool _isDrawingSelectionGrid;
        private Vector2 _propertiesScrollPos;
        private Vector2 _rightClickStartPosition;
        private Vector2 _showAddNodesListPosition;
        private bool _showAddNodesList;
        private string _addNodesListSearch;

        public Rect sidebarResizableAreaRect
        {
            get { return new Rect(position.width - sidebarWidth - 20f, 0f, 20f, position.height); }
        }

        private const string SidebarWidthPrefsKey = "QuestSystemPro_SidebarWidth";
        private static int _sidebarWidth = 350;
        public static int sidebarWidth
        {
            get { return _sidebarWidth; }
            set
            {
                _sidebarWidth = value;
                _sidebarWidth = Mathf.Clamp(_sidebarWidth, MinSidebarWidth, MaxSidebarWidth);

                EditorPrefs.SetInt(SidebarWidthPrefsKey, _sidebarWidth);
            }
        }

        public const int MinSidebarWidth = 250;
        public const int MaxSidebarWidth = 450;

        const int AddNodesListWidth = 300;
        const int AddNodesListHeight = 400;

        public const float ZoomSpeed = 0.03f;
        public const float MinZoomAmount = 0.4f;
        public const float MaxZoomAmount = 1f;

        public float zoomAmount = 1f;
        public List<NodeBase> clipboard = new List<NodeBase>();
        public List<Edge> selectedEdges = new List<Edge>();

        public static List<DialogueEditorSidebarBase> sidebars = new List<DialogueEditorSidebarBase>();

        protected static Dictionary<string, List<Type>> addNodesListLookup = new Dictionary<string, List<Type>>();
        protected static List<string> addNodesListExpandedKeys = new List<string>();
        private static Rect _addNodesListRect;

        private const string SelectedSidebarPrefsKey = "QuestSystemPro_SelectedSidebarName";
        private static DialogueEditorSidebarBase _selectedSidebar;
        public static DialogueEditorSidebarBase selectedSidebar
        {
            get { return _selectedSidebar; }
            protected set
            {
                _selectedSidebar = value;
                EditorPrefs.SetString("SelectedSidebarPrefsKey", _selectedSidebar != null ? _selectedSidebar.name : "");
            }
        }

        private static bool _isListeningForDialogueChange = false;

        private static bool _isDirty;
        public Vector2 nodesOffset { get; protected set; }


        [MenuItem("Tools/Quest System Pro/Dialogue Editor")]
        protected static void Init()
        {
            window = GetWindow<DialogueEditorWindow>();
            window.minSize = new Vector2(400,400);
            window.autoRepaintOnSceneChange = true;
            window.wantsMouseMove = false;

            var icon = (Texture)Resources.Load("QuestSystemPro_DialogueEditorIcon");
            window.titleContent = new GUIContent("Dialogue editor", icon);

            CreateSidebars();

            _sidebarWidth = EditorPrefs.GetInt(SidebarWidthPrefsKey, 350);

            UpdateAvailableNodeTypes();
//            DialogueNodesSidebarEditor.Init();
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        public static void ReloadedScripts()
        {
//            Debug.Log("Scripts reloaded");
            _isDirty = true;
        }

        private void ValidateDialogue(Dialogue d)
        {
            for (int i = 0; i < d.nodes.Length; i++)
            {
                if (d.nodes[i].index != i)
                {
                    Debug.Log("Node index out of sync, resetting...");
                    foreach (var n in d.nodes)
                    {
                        foreach (var edge in n.edges)
                        {
                            if (edge.toNodeIndex == d.nodes[i].index)
                            {
                                edge.toNodeIndex = (uint)i;
                            }
                        }
                    }

                    var field = ReflectionUtility.GetFieldInherited(d.nodes[i].GetType(), "_index");
                    field.SetValue(d.nodes[i], (uint)i);
                }
            }
        }

        private static void CreateSidebars()
        {
            sidebars.Clear();
            sidebars.Add(new DialogueEditorSidebarProperties("Properties"));
            sidebars.Add(new DialogueEditorSidebarVariables("Variables"));

            _selectedSidebar = sidebars.FirstOrDefault(o => o.name == EditorPrefs.GetString(SelectedSidebarPrefsKey));
            if (_selectedSidebar == null)
            {
                _selectedSidebar = sidebars[0];
            }
        }

        private static void Dialogue_OnCurrentNodeChanged(NodeBase before, NodeBase after)
        {
            PingNode(after);
        }

        public static void PingNode(NodeBase node)
        {
            var selectedEditor = window.nodeEditors.FirstOrDefault(o => o.node == node);
            if (selectedEditor != null)
            {
                selectedEditor.PingDebugView(DebugNodeVisualType.Debug);
            }
        }

        public static void FocusOnNode(NodeBase node)
        {
            if (window == null)
            {
                Init();
            }

            var windowSize = window.position.size;
            windowSize.x -= sidebarWidth;
            windowSize /= 2f;

            var p = windowSize - node.editorPosition;
            p.x = Mathf.RoundToInt(p.x);
            p.y = Mathf.RoundToInt(p.y);

            window.nodesOffset = p;
            window.Repaint();
        }

        private void NotifyNodeEditorsChanged()
        {
            RegenerateAllEditors();
        }

        public static void RegenerateAllEditors()
        {
            if (window.dialogue == null)
            {
                return;
            }

            if (window.dialogue.nodes.FirstOrDefault() is EntryNode == false)
            {
                window.dialogue.AddNode(NodeFactory.Create<EntryNode>());
            }

            window.nodeEditors.Clear();
            foreach (NodeBase node in window.dialogue.nodes)
            {
                window.nodeEditors.Add(window.CreateEditorForNode(node));
            }

            window.Repaint();
        }

        public virtual NodeEditorBase CreateEditorForNode(NodeBase node)
        {
            var type = DialogueReflectionUtility.GetCustomNodeEditorFor(node.GetType());
            if (type != null)
            {
                var editor = (NodeEditorBase)Activator.CreateInstance(type);
                editor.Init(node, this);

                return editor;
            }

            var editor2 = new DefaultNodeEditor();
            editor2.Init(node, this);

            return editor2;
        }

        public override void OnGUI()
        {
            base.OnGUI();

            if (window == null)
            {
                Init();
                return;
            }

            if (dialogue == null)
            {
                GUI.Label(new Rect(0, 0, 400, EditorGUIUtility.singleLineHeight), "No dialogue selected");
                return;
            }

            if (LocalizationManager.instance == null || LocalizationManager.defaultDatabase == null || LocalizationManager.GetAvailableLanguageDatabases().Length == 0)
            {
                GUI.Label(new Rect(position.width / 2f - 300f, position.height / 2f - 10f, 600f, EditorGUIUtility.singleLineHeight), "No localization manager / databases found. The localization database saves all localizable data.", UnityEditor.EditorStyles.wordWrappedLabel);

                if (GUI.Button(new Rect(position.width / 2f - 100f, position.height / 2f + 10f, 200f, EditorGUIUtility.singleLineHeight), "Open setup wizard"))
                {
                    GameRulesWindow.ShowWindow();
                }
                return;
            }


            if (_isDirty)
            {
                _isDirty = false;
                NotifyNodeEditorsChanged();
            }

            // TODO: Consider adding a check to see if 'something' changed. reflection drawers can detect change with NotifyValueChanged (could mark dirty) + dragging nodes.
            EditorUtility.SetDirty(dialogue); // Always serialize..

            if (GUI.Button(new Rect(0, 0, 100, 24), "Save"))
            {
                dialogue.Save();
                UnityEditor.AssetDatabase.SaveAssets();
            }

            if (GUI.Button(new Rect(100, 0, 100, 24), "Load"))
            {
                dialogue.Load();
                NotifyNodeEditorsChanged();
            }

            //if (GUI.Button(new Rect(200, 0, 100, 24), "Open sidebar"))
            //{
            //    DialogueNodesSidebarEditor.Init();
            //}


            if (EditorApplication.isPlaying)
            {
                if (_isListeningForDialogueChange)
                {
                    if (GUI.Button(new Rect(300, 0, 150, 24), new GUIContent("[Stop] auto change")))
                    {
                        _isListeningForDialogueChange = false;
                        DialogueManager.instance.OnCurrentDialogueChanged -= OnCurrentDialogueChangedAutoChange;
                    }
                }
                else
                {
                    if (GUI.Button(new Rect(300, 0, 150, 24), new GUIContent("[Start] auto change", "Automatically change the dialogue when the player uses one in-game.")))
                    {
                        _isListeningForDialogueChange = true;
                        DialogueManager.instance.OnCurrentDialogueChanged += OnCurrentDialogueChangedAutoChange;
                    }
                }
            }


            if (_isDraggingEdge)
            {
                DialogueEditorUtility.DrawCurves(_dragStartPosition, Event.current.mousePosition, Color.white);
            }
            else if (_isDrawingSelectionGrid && _isDraggingSidebarWidth == false)
            {
                DrawSelectionRect();
            }


            var nodeBoxRect = GetNodeBoxRect();
            //Matrix4x4 trs = Matrix4x4.TRS(new Vector3(nodeBoxRect.width / 2, nodeBoxRect.height / 2f - 21f, 0f), Quaternion.identity, Vector3.one);
            //Matrix4x4 scale = Matrix4x4.Scale(new Vector3(zoomAmount, zoomAmount, zoomAmount));
            //using (new GUIMatrixBlock(trs * scale * trs.inverse))
            {
                //nodeBoxRect.width /= zoomAmount;
                //nodeBoxRect.height /= zoomAmount;

                using (new GroupBlock(nodeBoxRect))
                {
                    DrawAllEdges();
                    DrawAllNodes();
                }
            }

            //DrawZoomIndicator();
            DrawMinimap();

            if (_showAddNodesList)
            {
                DrawAddNodesList();
            }

            // Resizable sidebar
            EditorGUIUtility.AddCursorRect(sidebarResizableAreaRect, MouseCursor.ResizeHorizontal);

            using (new ScrollableBlock(
                    new Rect(position.width - sidebarWidth, 0f, sidebarWidth, position.height),
                    ref _propertiesScrollPos,
                    200))
            {
                using (new GroupBlock(new Rect(0f, 0f, sidebarWidth, position.height), GUIContent.none, EditorStyles.helpBox))
                {
                    DrawSidebar();
                }
            }
        }

        public static void UpdateAvailableNodeTypes()
        {
            var availableNodeTypes = ReflectionUtility.GetAllTypesThatImplement(typeof(NodeBase));
            addNodesListLookup.Clear();
            foreach (var n in availableNodeTypes)
            {
                var category = (CategoryAttribute)n.GetCustomAttributes(typeof(CategoryAttribute), true).FirstOrDefault();
                var hideInSidebar = (HideInCreationWindowAttribute)n.GetCustomAttributes(typeof(HideInCreationWindowAttribute), true).FirstOrDefault();
                if (hideInSidebar != null)
                {
                    continue;
                }

                string categoryName = "Undefined";
                if (category != null)
                {
                    categoryName = category.category;
                }

                if (addNodesListLookup.ContainsKey(categoryName) == false)
                {
                    addNodesListLookup[categoryName] = new List<Type>();
                }

                addNodesListLookup[categoryName].Add(n);
            }
        }

        private static List<Type> _addNodesListSearchedTypes = new List<Type>();
        private static int _addNodesListSelectedIndex = 0;

        private void DrawAddNodesList()
        {
            var isSearching = string.IsNullOrEmpty(_addNodesListSearch) == false;
            var buttonRect = new Rect(0, 0f, AddNodesListWidth - 10f, EditorGUIUtility.singleLineHeight);
            const int buttonMargin = 4;

            _addNodesListSearch = _addNodesListSearch.Replace(" ", "").Trim();
            _addNodesListRect = new Rect(_showAddNodesListPosition + (Vector2.one * 5f), new Vector2(AddNodesListWidth, AddNodesListHeight));
            _addNodesListSearchedTypes.Clear();

            if (isSearching && Event.current.type == EventType.KeyDown)
            {
                if (Event.current.keyCode == KeyCode.DownArrow)
                {
                    _addNodesListSelectedIndex++;
                }
                else if (Event.current.keyCode == KeyCode.UpArrow)
                {
                    _addNodesListSelectedIndex--;
                }
            }

            using (new GroupBlock(_addNodesListRect, GUIContent.none, "box"))
            {
                var rect = new Rect(3f, 2f, AddNodesListWidth - 10f, EditorGUIUtility.singleLineHeight + buttonMargin);
                _addNodesListSearch = Devdog.General.Editors.EditorStyles.SearchBar(rect, _addNodesListSearch, this, isSearching);
                GUI.FocusControl("SearchField");

                rect = new Rect(new Vector2(5f, EditorGUIUtility.singleLineHeight + (buttonMargin * 2f)), new Vector2(AddNodesListWidth - 10f, AddNodesListHeight - EditorGUIUtility.singleLineHeight));

                foreach (var kvp in addNodesListLookup)
                {
                    foreach (var type in addNodesListLookup[kvp.Key])
                    {
                        if (isSearching)
                        {
                            if (type.Name.ToLower().Contains(_addNodesListSearch.ToLower()) == false)
                            {
                                continue;
                            }

                            _addNodesListSearchedTypes.Add(type);
                        }
                    }
                }

                _addNodesListSelectedIndex = Mathf.Clamp(_addNodesListSelectedIndex, 0, Mathf.Max(0, _addNodesListSearchedTypes.Count - 1));
                _addNodesListSearchedTypes = _addNodesListSearchedTypes.OrderBy(o => LevenshteinDistance(o.Name.ToLower(), _addNodesListSearch.ToLower()) + o.Name.Length - _addNodesListSearch.Length).ToList();
                using (new GroupBlock(rect, GUIContent.none))
                {
                    if (isSearching)
                    {
                        for (int i = 0; i < _addNodesListSearchedTypes.Count; i++)
                        {
                            var type = _addNodesListSearchedTypes[i];
                            if (_addNodesListSelectedIndex == i)
                            {
                                GUI.color = Color.green;
                            }

                            if (GUI.Button(buttonRect, type.Name))
                            {
                                CreateAndAddNodeToCurrentDialog(type, _showAddNodesListPosition - nodesOffset);
                                HideAddNodesList();

                                break;
                            }

                            buttonRect.y += EditorGUIUtility.singleLineHeight + buttonMargin;

                            if (Event.current.keyCode == KeyCode.Return)
                            {
                                CreateAndAddNodeToCurrentDialog(_addNodesListSearchedTypes[_addNodesListSelectedIndex], _showAddNodesListPosition - nodesOffset);
                                HideAddNodesList();

                                break;
                            }

                            GUI.color = Color.white;
                        }
                    }
                    else
                    {
                        foreach (var kvp in addNodesListLookup)
                        {
                            GUI.color = Color.cyan;
                            if (GUI.Button(buttonRect, kvp.Key, "ButtonMid"))
                            {
                                if (addNodesListExpandedKeys.Contains(kvp.Key))
                                {
                                    addNodesListExpandedKeys.Remove(kvp.Key);
                                }
                                else
                                {
                                    addNodesListExpandedKeys.Add(kvp.Key);
                                }
                            }

                            buttonRect.y += EditorGUIUtility.singleLineHeight + buttonMargin;

                            GUI.color = Color.white;
                            if (addNodesListExpandedKeys.Contains(kvp.Key))
                            {
                                foreach (var type in addNodesListLookup[kvp.Key])
                                {
                                    buttonRect.x += 10f;
                                    buttonRect.width -= 10f;

                                    if (GUI.Button(buttonRect, type.Name))
                                    {
                                        CreateAndAddNodeToCurrentDialog(type, _showAddNodesListPosition - nodesOffset);
                                        HideAddNodesList();
                                    }

                                    buttonRect.x = 0f;
                                    buttonRect.width = AddNodesListWidth - 10f;
                                    buttonRect.y += EditorGUIUtility.singleLineHeight + buttonMargin;
                                }
                            }
                        }
                    }

                }

                GUI.color = Color.white;
            }
        }

        private static int LevenshteinDistance(string a, string b)
        {
            if (string.IsNullOrEmpty(a))
            {
                if (!string.IsNullOrEmpty(b))
                {
                    return b.Length;
                }
                return 0;
            }

            if (string.IsNullOrEmpty(b))
            {
                if (!string.IsNullOrEmpty(a))
                {
                    return a.Length;
                }
                return 0;
            }

            int cost;
            int[,] d = new int[a.Length + 1, b.Length + 1];
            int min1;
            int min2;
            int min3;

            for (int i = 0; i <= d.GetUpperBound(0); i += 1)
            {
                d[i, 0] = i;
            }

            for (int i = 0; i <= d.GetUpperBound(1); i += 1)
            {
                d[0, i] = i;
            }

            for (int i = 1; i <= d.GetUpperBound(0); i += 1)
            {
                for (int j = 1; j <= d.GetUpperBound(1); j += 1)
                {
                    cost = Convert.ToInt32(a[i - 1] != b[j - 1]);

                    min1 = d[i - 1, j] + 1;
                    min2 = d[i, j - 1] + 1;
                    min3 = d[i - 1, j - 1] + cost;
                    d[i, j] = Mathf.Min(Mathf.Min(min1, min2), min3);
                }
            }

            return d[d.GetUpperBound(0), d.GetUpperBound(1)];
        }

        private void OnCurrentDialogueChangedAutoChange(Dialogue before, Dialogue after, IDialogueOwner owner)
        {
            this.dialogueOwner = owner;
        }

        protected override void OnMouseDrag(Vector2 delta)
        {
            if (GetNodeBoxRect().Contains(Event.current.mousePosition))
            {
                OnTryDragNode(delta);
                OnPerformDrag(delta);
            }
        }

        private Rect GetNodeBoxRect()
        {
            //return new Rect(0, 0, (position.width - sidebarWidth), position.height);
            //var width = (position.width - sidebarWidth) / zoomAmount;
            //var height = position.height / zoomAmount;

            return new Rect(0, 0, position.width - sidebarWidth, position.height);
        }

        private void DrawMinimap()
        {
            var zoomRect = new Rect(position.width - sidebarWidth - 160f, 10f, 150f, 100f);

            GUI.color = new Color(1f, 1f, 1f, 0.5f);

            using (new GroupBlock(zoomRect, GUIContent.none, "CN Box"))
            {
                var allNodesInRect = nodeEditors;

                foreach (var n in allNodesInRect)
                {
                    var r = GetNodeBoxRect();
                    r.x += r.width;
                    r.y += r.height;

                    var scaleMult = zoomRect.width / r.width;
                    scaleMult *= 0.35f;

                    var scaledRect = n.GetNodeRect();
                    scaledRect.x += r.width/2f;
                    scaledRect.y += r.height/2f;
                    scaledRect.x -= scaledRect.width;
                    scaledRect.y -= scaledRect.height;

                    scaledRect.position *= scaleMult;
                    scaledRect.size *= 0.07f;
                    scaledRect = ToZoomSpaceRect(scaledRect);

                    EditorGUI.LabelField(scaledRect, GUIContent.none, (GUIStyle)"ColorPickerBox");
                }
            }

            GUI.color = Color.white;
        }

        private void DrawZoomIndicator()
        {
            var zoomRect = new Rect(position.width - sidebarWidth - 160f, 10f, 140f, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(zoomRect, "Zoom: " + zoomAmount * 100f + "%");

            GUI.color = new Color(0, 0, 0, 0.5f);

            zoomRect.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.LabelField(zoomRect, GUIContent.none, (GUIStyle)"MiniMinMaxSliderHorizontal");

            GUI.color = Color.white;

            zoomRect.width *= zoomAmount;
            EditorGUI.LabelField(zoomRect, GUIContent.none, (GUIStyle)"MiniMinMaxSliderHorizontal");
        }

        private void CopyNodesToClipboard(List<NodeEditorBase> nodes)
        {
            if (GUIUtility.keyboardControl >= 0)
            {
                return;
            }

            clipboard = nodes.Select(o => o.node).ToList();
            Debug.Log("Copied " + clipboard.Count + " nodes to clipboard");
        }

        private void PasteNodesFromClipboard()
        {
            if (GUIUtility.keyboardControl >= 0)
            {
                return;
            }

            if (clipboard.Count == 0)
            {
                return;
            }

            // Used for index before and index after adding it to the dialogue. This can be used to re-map edges to the new node.
            var lookupDict = new Dictionary<uint, uint>();

            var clones = new List<NodeBase>();
            foreach (var node in clipboard)
            {
                clones.Add(EditorReflectionUtility.CreateDeepClone<NodeBase>(node));
            }

            if (position.Contains(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)))
            {
                // If the cursor is inside the window paste the new nodes near the cursor.
                var firstNodePosition = clipboard[0].editorPosition;
                for (int i = 0; i < clones.Count; i++)
                {
                    var deltaPosition = clipboard[i].editorPosition - firstNodePosition;
                    clones[i].editorPosition = Event.current.mousePosition + deltaPosition - nodesOffset;
                }
            }
            else
            {
                foreach (var node in clones)
                {
                    node.editorPosition = new Vector2(node.editorPosition.x + 100f, node.editorPosition.y) - nodesOffset;
                }
            }

            for(int i = 0; i < clones.Count; i++)
            {
                var indexBefore = clipboard[i].index;
                AddNode(clones[i]);
                lookupDict[indexBefore] = clones[i].index;
            }

            int index = 0;
            foreach (var kvp in lookupDict)
            {
                var node = dialogue.nodes[kvp.Value];
                for (int edgeIndex = node.edges.Length - 1; edgeIndex >= 0; edgeIndex--)
                {
                    var edge = node.edges[edgeIndex];
                    if (clones.Any(o => o.index == edge.toNodeIndex) == false)
                    {
                        // Refers to node outside of clipboard.
                        if (lookupDict.ContainsKey(edge.toNodeIndex))
                        {
                            edge.toNodeIndex = lookupDict[edge.toNodeIndex];
                        }
                        else
                        {
                            nodeEditors[(int)kvp.Value].RemoveEdge((uint)edgeIndex);
                        }
                    }
                }

                index++;
            }

            UnSelectAllNodes();
            SelectNodes(clones.Select(o => nodeEditors[(int)o.index]).ToList());
        }

        private void DrawSelectionRect()
        {
            if (GetNodeBoxRect().Contains(Event.current.mousePosition))
            {

                var start = _dragStartPosition;
                var end = Event.current.mousePosition - start;

                var r = new Rect(start, end);
                SelectNodes(GetNodesInRect(r));

                GUI.Label(r, GUIContent.none, "SelectionRect");
            }
        }

        private void OnTryDragNode(Vector2 delta)
        {
            if (GetNodeBoxRect().Contains(Event.current.mousePosition))
            {
                if (_isDrawingSelectionGrid == false && _isDraggingNode == false && _isDraggingEdge == false && _isDraggingSidebarWidth == false)
                {
                    if (Event.current.button == 0)
                    {
                        foreach (var node in nodeEditors)
                        {
                            var connectors = node.GetEdgeConnectors();
                            for (uint i = 0; i < connectors.Length; i++)
                            {
                                if (ToZoomSpaceRect(connectors[i].rect).Contains(Event.current.mousePosition))
                                {
                                    _dragStartPosition = Event.current.mousePosition;
                                    _dragEdgeConnector = connectors[i];

                                    _isDraggingEdge = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        protected override void OnMouseDown(int button)
        {
            var mousePos = GetMousePosition();
            if (GetNodeBoxRect().Contains(mousePos))
            {
                if (button == 0)
                {
                    var edge = GetEdgeAtPosition(mousePos);
                    SelectEdges(edge);
                }
                else if (button == 1)
                {
                    _rightClickStartPosition = mousePos;
                }
            }
        }

        protected override void OnMouseUp(int button)
        {
            var mousePos = GetMousePosition();
            if (GetNodeBoxRect().Contains(mousePos))
            {
                if (button == 0)
                {
                    if (_isDraggingEdge)
                    {
                        bool attached = false;
                        foreach (var connectToNode in nodeEditors)
                        {
                            if(attached)
                            {
                                break;
                            }

                            if (connectToNode.node.index == _dragEdgeConnector.node.node.index)
                            {
                                continue; // We can never connect to ourselves...
                            }

                            foreach (var connector in connectToNode.GetReceivingEdgeConnectors())
                            {
                                if (connectToNode.CanConnectEdgeFromNode(_dragEdgeConnector.node.node))
                                {
                                    if (ToZoomSpaceRect(connector.rect).Contains(mousePos))
                                    {
                                        _dragEdgeConnector.node.SetEdge(_dragEdgeConnector, connectToNode.node);
                                        attached = true;
                                        break;
                                    }
                                }
                            }
                        }

                        if(attached == false)
                        {
                            _connectToNextNode = _dragEdgeConnector;

                            OnEndDrag();
                            ShowAddNodesList();
                            
                            return;
                        }
                    }
                    else if (_isDraggingNode)
                    {
                        OnEndDrag();
                    }
                    else
                    {
                        var hoveringNode = GetNodeAtPosition(mousePos);
                        if (hoveringNode == null)
                        {
                            EditorGUIUtility.editingTextField = false;
                            GUIUtility.keyboardControl = -1;
                            //GUIUtility.hotControl = -1;
                        }

                        if (selectedNodeEditors.Contains(hoveringNode))
                        {
                            if (Event.current.control)
                            {
                                hoveringNode.UnSelect();
                                selectedNodeEditors.Remove(hoveringNode);
                            }
                        }
                        else
                        {
                            SelectNodes(new List<NodeEditorBase>() { hoveringNode });
                        }
                    }

                    if (_addNodesListRect.Contains(mousePos) == false && _showAddNodesList)
                    {
                        HideAddNodesList();
                    }
                }
                else if (button == 1)
                {
                    bool removed = RemoveEdgeAtPosition(mousePos);
                    if(removed)
                    {
                        OnEndDrag();
                        return;
                    }

                    // Show the add node list
                    var mag = Mathf.Abs((_rightClickStartPosition - mousePos).magnitude);
                    if (mag < 5f)
                    {
                        OnEndDrag();
                        ShowAddNodesList();

                        return;
                    }
                }
            }

            if (_isDraggingNode)
            {
                OnEndDrag();
            }
        }

        protected override void OnKeyDown(KeyCode keyCode)
        {
            base.OnKeyDown(keyCode);

            if (keyCode == KeyCode.Escape)
            {
                HideAddNodesList();
            }
            else if (keyCode == KeyCode.Space)
            {
                if (selectedNodeEditors.Count == 0 && selectedEdges.Count == 0)
                {
                    ShowAddNodesList();
                }
            }
            

//            // TODO: Create setting to enable / disable arrow key movement behavior.
//            if (selectedNodeEditors.Count > 0)
//            {
//                var move = Vector2.zero;
//                if (keyCode == KeyCode.RightArrow)
//                    move.x += 1f;
//                else if (keyCode == KeyCode.LeftArrow)
//                    move.x -= 1f;
//                else if (keyCode == KeyCode.UpArrow)
//                    move.y -= 1f;
//                else if (keyCode == KeyCode.DownArrow)
//                    move.y += 1f;
//
//                if (Event.current.shift)
//                {
//                    move *= 20f;
//                }
//
//                move /= _zoomAmount;
//
//                foreach (var node in selectedNodeEditors)
//                {
//                    node.Drag(move);
//                }
//            }
        }

        private void SelectEdges(params Edge[] edges)
        {
            selectedEdges.Clear();
            selectedEdges.AddRange(edges.Where(o => o != null));
        }

        protected override void OnKeyUp(KeyCode keyCode)
        {
            base.OnKeyUp(keyCode);

            if (Event.current.control && keyCode == KeyCode.C)
            {
                // Copy
                CopyNodesToClipboard(selectedNodeEditors);
            }
            else if (Event.current.control && keyCode == KeyCode.V)
            {
                // Paste
                PasteNodesFromClipboard();
            }
            else if (Event.current.control && keyCode == KeyCode.D)
            {
                // Duplicate
                CopyNodesToClipboard(selectedNodeEditors);
                PasteNodesFromClipboard();
            }


            if (keyCode == KeyCode.Delete ||
                (((Event.current.modifiers & EventModifiers.Command) != 0) && keyCode == KeyCode.Backspace) ||
                (((Event.current.modifiers & EventModifiers.Control) != 0) && keyCode == KeyCode.Backspace))
            {
                if (selectedNodeEditors.Count > 0 && (EditorGUIUtility.editingTextField == false || GUIUtility.keyboardControl == -1))
                {
                    if (EditorUtility.DisplayDialog("Deleting nodes",
                    "Are you sure you want to delete " + selectedNodeEditors.Count + " nodes?", "Yes", "Cancel"))
                    {
                        RemoveNodes(selectedNodeEditors);
                        selectedNodeEditors.Clear();
                    }
                }
            }
        }

        protected override void OnScroll(Vector2 delta)
        {
            base.OnScroll(delta);
            //Zoom(zoomAmount + -delta.y * ZoomSpeed);
        }

        private void Zoom(float amount)
        {
            zoomAmount = amount;
            zoomAmount = Mathf.Clamp(zoomAmount, MinZoomAmount, MaxZoomAmount);
            zoomAmount = (float)System.Math.Round((decimal)zoomAmount, 1);
        }

        private bool RemoveEdgeAtPosition(Vector2 position)
        {
            foreach (var node in nodeEditors)
            {
                var connectors = node.GetEdgeConnectors();
                for (uint i = 0; i < connectors.Length; i++)
                {
                    if(connectors[i].isHelperConnector)
                    {
                        continue;
                    }

                    if (ToZoomSpaceRect(connectors[i].rect).Contains(position))
                    {
                        node.RemoveEdge((uint)connectors[i].index);
                        return true;
                    }
                }
            }

            return false;
        }

        public void AddNode(NodeBase node)
        {
            dialogue.AddNode(node);
            nodeEditors.Add(CreateEditorForNode(node));

            if(_connectToNextNode != null && _connectToNextNode.node != null)
            {
                nodeEditors[(int)_connectToNextNode.node.node.index].SetEdge(_connectToNextNode, node);
            }

            _connectToNextNode = null;
            EditorUtility.SetDirty(dialogue);
            GUI.changed = true;
            Repaint();
        }

        public void RemoveNode(NodeBase node)
        {
            dialogue.RemoveNode(node);
            nodeEditors.RemoveAt((int)node.index);

            EditorUtility.SetDirty(dialogue);
            GUI.changed = true;
            Repaint();
        }

        private void RemoveNodes(List<NodeEditorBase> nodes)
        {
            foreach (var node in nodes.Where(o => o.CanDelete()))
            {
                RemoveNode(node.node);
            }
        }

        private void DrawSidebar()
        {
            // Create some tabs
            var indexBefore = sidebars.IndexOf(selectedSidebar);
            var sidebarNames = sidebars.Select(o => o.name).ToArray();
            var index = GUI.Toolbar(new Rect(0, 0, sidebarWidth, 20), indexBefore, sidebarNames, "toolbarbutton");
            if (index != indexBefore)
            {
                selectedSidebar = sidebars[index];
            }

            GUI.Label(new Rect(10, 30f, sidebarWidth - 20f, EditorGUIUtility.singleLineHeight), _selectedSidebar.name, EditorStyles.boldLabel);
            using (new GroupBlock(new Rect(10f, 50f, sidebarWidth - 20f, position.height - 20f)))
            {
                var r = new Rect(0, 0, sidebarWidth - 20f, EditorGUIUtility.singleLineHeight);
                if (selectedSidebar != null)
                {
                    selectedSidebar.Draw(r, this);
                }
                else
                {
                    GUI.Label(r, "Couldn't find page to load...");
                }
            }
        }

        private void DrawAllEdges()
        {
            foreach (var node in nodeEditors)
            {
                if (node == null)
                {
                    continue;
                }

                node.DrawEdges();
            }
        }

        private void OnPerformDrag(Vector2 delta)
        {
            delta /= zoomAmount;

            if (_isDraggingNode == false && _isDraggingSidebarWidth == false)
            {
                if (Event.current.button == 0)
                {
                    var hoveringNode = GetNodeAtPosition(Event.current.mousePosition);
                    if (hoveringNode != null)
                    {
                        if (selectedNodeEditors.Contains(hoveringNode) == false)
                        {
                            // No nodes selected + started drag on a node, select it directly.
                            SelectNodes(new List<NodeEditorBase>() { hoveringNode });
                        }
                    }
                    else
                    {
                        _isDrawingSelectionGrid = true;
                    }
                }

                // Start dragging
                _dragStartPosition = Event.current.mousePosition;
                _isDraggingNode = true;
            }
            else
            {
                if (sidebarResizableAreaRect.Contains(Event.current.mousePosition))
                {
                    _isDraggingSidebarWidth = true;

                    sidebarWidth -= (int)delta.x;
                    return;
                }

                if (Event.current.button == 2 || Event.current.button == 1)
                {
                    // Scrollwheel or righ click drag
                    nodesOffset += delta;
                    return;
                }

                if (selectedNodeEditors.Count > 0 && _isDrawingSelectionGrid == false)
                {
                    if (Event.current.button == 0)
                    {
                        foreach (var selectedNode in selectedNodeEditors)
                        {
                            selectedNode.Drag(delta);
                        }
                    }
                }
            }
        }

        private void OnEndDrag()
        {
            _isDraggingNode = false;
            _isDraggingEdge = false;
            _isDrawingSelectionGrid = false;
            _isDraggingSidebarWidth = false;
            _dragEdgeConnector = null;

            if (_addNodesListRect.Contains(Event.current.mousePosition) == false && _showAddNodesList)
            {
                HideAddNodesList();
            }
        }

        private void HideAddNodesList()
        {
            _showAddNodesList = false;
            _addNodesListSearch = string.Empty;
            _connectToNextNode = null;

            // Remove control from the add node list
            GUIUtility.keyboardControl = -1;
            GUIUtility.hotControl = -1;
        }

        private void ShowAddNodesList()
        {
            _showAddNodesList = true;
            _showAddNodesListPosition = Event.current.mousePosition;
            _addNodesListSearch = string.Empty;
            _addNodesListSelectedIndex = 0;
        }

        private void UnSelectAllNodes()
        {
            foreach (var editor in selectedNodeEditors)
            {
                editor.UnSelect();
            }

            selectedNodeEditors.Clear();
        }

        private void SelectNodes(List<NodeEditorBase> nodes)
        {
            nodes.RemoveAll(o => o == null);

            if (Event.current.control)
            {
                foreach (var node in nodes)
                {
                    if (selectedNodeEditors.Contains(node) == false)
                    {
                        selectedNodeEditors.Add(node);
                        node.Select();
                    }
                }
            }
            else
            {
                foreach (var node in selectedNodeEditors)
                {
                    node.UnSelect();
                }

                selectedNodeEditors.Clear();
                selectedNodeEditors.AddRange(nodes);

                foreach (var node in selectedNodeEditors)
                {
                    node.Select();
                }


                //                if (nodes.Count == 0)
                //                {
                //                    GUI.FocusControl("QuestSystemPro_NonExisting");
                //                }
            }
        }

        protected Vector2 GetMousePosition()
        {
            var pos = Event.current.mousePosition;
            pos *= zoomAmount;

            return pos;
        }

        private Rect ToZoomSpaceRect(Rect rect)
        {
            rect.position *= zoomAmount;
            rect.size *= zoomAmount;

            return rect;
        }

        private List<NodeEditorBase> GetNodesInRect(Rect rect)
        {
            var selectedNodes = new List<NodeEditorBase>();
            foreach (var node in nodeEditors)
            {
                if (rect.Overlaps(ToZoomSpaceRect(node.GetNodeRect()), true))
                {
                    selectedNodes.Add(node);
                }
            }

            return selectedNodes;
        }

        private NodeEditorBase GetNodeAtPosition(Vector2 pos)
        {
            // Objects are drawn front to back, so last drawn item is on top; Reversed for to grab from back to front (top to bottom).
            for (int i = nodeEditors.Count - 1; i >= 0; i--)
            {
                var node = nodeEditors[i];
                if (ToZoomSpaceRect(node.GetNodeRect()).Contains(pos))
                {
                    return node;
                }
            }

            return null;
        }

        private Edge GetEdgeAtPosition(Vector2 pos)
        {
            foreach (var nodeEditor in nodeEditors)
            {
                // TODO: Iterate over GetEdgeConnectors of node editor instead of node.edgges and ../...
                foreach(var connector in nodeEditor.GetEdgeConnectors())
                {
                    if(connector.isHelperConnector)
                    {
                        continue;
                    }

                    if(connector.index >= nodeEditor.node.edges.Length)
                    {
                        continue;
                    }

                    var edge = nodeEditor.node.edges[connector.index];

                    var fromRect = ToZoomSpaceRect(connector.rect);
                    var fromPos = fromRect.position;
                    fromPos.x += fromRect.width / 2;
                    fromPos.y += fromRect.height / 2;

                    var to = nodeEditors[(int)edge.toNodeIndex];
                    var toRect = ToZoomSpaceRect(to.GetReceivingEdgeConnectors()[0].rect);
                    var toPos = toRect.position;
                    toPos.x += toRect.width / 2;
                    toPos.y += toRect.height / 2;

                    var dist = Vector3.Distance(fromPos, toPos);
                    Vector3 startTangent = fromPos + Vector2.up * (dist / 3f);
                    Vector3 endTangent = toPos + Vector2.down * (dist / 3f);
                    if (HandleUtility.DistancePointBezier(pos, fromPos, toPos, startTangent, endTangent) < 10f)
                    {
                        //                        Debug.Log("Select edge " + edge.fromNodeIndex);
                        return edge;
                    }
                }
            }

            return null;
        }

        private void DrawAllNodes()
        {
            foreach (var node in nodeEditors)
            {
                var before = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 60f;

                // Only draw visible nodes
                var viewingRect = GetNodeBoxRect();
                viewingRect.x -= node.nodeSize.x * 2;
                viewingRect.width += node.nodeSize.x * 2;
                viewingRect.y -= node.nodeSize.y * 2;
                viewingRect.height += node.nodeSize.y * 2;
                viewingRect.position -= nodesOffset;

                if (viewingRect.Contains(node.node.editorPosition))
                {
                    if (zoomAmount < 0.7f)
                    {
                        node.Draw(false);
                    }
                    else
                    {
                        node.Draw(true);
                    }
                }

                EditorGUIUtility.labelWidth = before;
            }
        }

//        private Dialogue GetDialogueFromGameObjects(GameObject[] gameObjects)
//        {
//            foreach (var obj in gameObjects)
//            {
//                var dialogueUser = obj.GetComponent<IDialogueUser>();
//                if (dialogueUser != null)
//                {
//                    var d = dialogueUser.dialogues.FirstOrDefault();
//                    return d;
//                }
//            }
//
//            return null;
//        }

        public static T CreateAndAddNodeToCurrentDialog<T>() where T : NodeBase
        {
            return (T)CreateAndAddNodeToCurrentDialog(typeof (T));
        }

        public static NodeBase CreateAndAddNodeToCurrentDialog(Type type)
        {
            if (window != null && window.dialogue != null)
            {
                var pos = -window.nodesOffset + (window.position.size / 2);
                pos.x -= sidebarWidth / 2f;
                pos.x -= 100;
                pos.y -= 60;

                return CreateAndAddNodeToCurrentDialog(type, pos);
            }

            return null;
        }

        public static NodeBase CreateAndAddNodeToCurrentDialog(Type type, Vector2 position)
        {
            if (window != null && window.dialogue != null)
            {
                if(type.IsGenericType)
                {
                    // TODO: Make the user pick a type to fill the generic with
                    type = type.MakeGenericType(typeof(float));
                }

                var inst = NodeFactory.Create(type);
                inst.editorPosition = position;
                window.AddNode(inst);

                return inst;
            }

            return null;
        }

        public static void Edit(IDialogueOwner owner)
        {
            Init();
            if (window != null)
            {
                window.dialogueOwner = owner;
            }
        }

        public static void Edit(Dialogue dialogue)
        {
            Init();
            if (window != null)
            {
                window.dialogue = dialogue;
            }
        }
    }
}