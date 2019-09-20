using System;
using Devdog.General;
using Devdog.General.ThirdParty.UniLinq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Devdog.QuestSystemPro.Dialogue
{
    [System.Serializable]
    [CreateAssetMenu(menuName = QuestSystemPro.ProductName + "/Dialogue")]
    public class Dialogue : BetterScriptableObject
    {
        public delegate void CurrentNodeChanged(NodeBase before, NodeBase after);
        public delegate void StatusChanged(DialogueStatus before, DialogueStatus after, Dialogue self, IDialogueOwner owner);

        public event CurrentNodeChanged OnCurrentNodeChanged;
        public event StatusChanged OnStatusChanged;


        public new string name;
        public string description;
        public string helpUrl;


        [NonSerialized]
        private DialogueStatus _status;
        public DialogueStatus status
        {
            get { return _status; }
            set
            {
                var before = _status;
                _status = value;

                if (before != _status)
                {
                    if (OnStatusChanged != null)
                    {
                        var o = DialogueManager.instance.currentDialogueOwner;
                        OnStatusChanged(before, _status, this, o != null && o.dialogue == this ? o : null);
                    }
                }
            }
        }

        [NonSerialized]
        private uint _currentNodeIndex = 0;
        public uint currentNodeIndex
        {
            get { return _currentNodeIndex; }
            protected set
            {
                var before = _currentNodeIndex;
                _currentNodeIndex = value;

                if (before != _currentNodeIndex && OnCurrentNodeChanged != null)
                {
                    OnCurrentNodeChanged(nodes[before], nodes[_currentNodeIndex]);
                }
            }
        }

        public NodeBase currentNode
        {
            get { return nodes[currentNodeIndex]; }
        }


        /// <summary>
        /// Unity won't actually serialize these, but FullSerializer will (abstract base class)
        /// </summary>
        [SerializeField]
        private NodeBase[] _nodes = new NodeBase[0];
        public NodeBase[] nodes
        {
            get { return _nodes; }
            set
            {
                _nodes = new NodeBase[0];
                Assert.IsNotNull(value, "Never set Dialogue.nodes to null! If you want to clear the nodes use an empty array instead.");

                if (value.Any(o => o is EntryNode) == false)
                {
                    DevdogLogger.LogWarning("Setting Dialogue.nodes, but no EntryNode was found in values - Added entry node with index 0.");
                    AddNode(NodeFactory.Create<EntryNode>());
                }

                AddNode(value);
            }
        }

        public IDialogueCondition[] conditions = new IDialogueCondition[0];

        [HideTypePicker]
        [HideGroup]
        public VariablesContainer variables = new VariablesContainer();

        protected Dialogue() : base()
        {
//            DevdogLogger.LogVerbose("New dialogue object created");
        }

        public override void Load()
        {
            base.Load();

            foreach (var node in nodes)
            {
                node.owner = this;
            }

            SetVariables();
            Variables.variableContainers.Add(variables);
        }

        private void SetVariables()
        {
            foreach (var variable in variables.variables)
            {
                variable.dialogue = this;
            }
        }

        public static Dialogue Create()
        {
            var dialogue = CreateInstance<Dialogue>();
            dialogue.AddNode(NodeFactory.Create<EntryNode>()); // A dialogue should always have an entry node.
            dialogue.status = DialogueStatus.InActive;

            return dialogue;
        }

        #region Management


        /// <summary>
        /// Add a node to this dialogue.
        /// Note that this is a slow method and should be avoided at run-time.
        /// </summary>
        public void AddNode(params NodeBase[] node)
        {
            uint index = (uint)this.nodes.Length;
            foreach (var n in node)
            {
                n.owner = this;
                n.index = index;
                index++;
            }

            var l = nodes.ToList();
            l.AddRange(node);
            _nodes = l.ToArray();
        }

        public void RemoveNode(uint index)
        {
            RemoveNode(nodes[index]);
        }

        /// <summary>
        /// This automatically removes all edges pointing to this node.
        /// </summary>
        /// <param name="node"></param>
        public void RemoveNode(params NodeBase[] node)
        {
            var l = nodes.ToList();
            for (int i = node.Length - 1; i >= 0; i--)
            {
                RemoveAllEdgesPointingToNode(node[i]);

                // Continue from where the node was deleted and lower all indexes after the deleted node by 1, realigning the ID's
                for (int j = 0; j < this.nodes.Length; j++)
                {
                    if (l[j].index > node[i].index)
                    {
                        l[j].index--;
                    }

                    // Re-align the edge indexes
                    for (int k = 0; k < l[j].edges.Length; k++)
                    {
                        var edge = l[j].edges[k];

                        // Only re-align if the toNodeIndex comes after the deleted node's index
                        if (edge.toNodeIndex > node[i].index)
                        {
                            edge.toNodeIndex--;
                        }
                    }
                }

                l.RemoveAt((int)node[i].index);
            }

            _nodes = l.ToArray();
        }

        public void RemoveAllEdgesPointingToNode(NodeBase node)
        {
            foreach (var tempNode in this.nodes)
            {
                var l = tempNode.edges.ToList();
                for (int i = tempNode.edges.Length - 1; i >= 0; i--)
                {
                    if (tempNode.edges[i].toNodeIndex == node.index)
                    {
                        l.RemoveAt(i);
                    }
                }

                tempNode.edges = l.ToArray();
            }
        }



#endregion

#region Checks



        public bool CanDiscover()
        {
            return CanDiscover(QuestManager.instance.localIdentifier);
        }

        public bool CanDiscover(ILocalIdentifier localIdentifier)
        {
            foreach (var condition in conditions)
            {
                if (condition.CanDiscover(this, localIdentifier).status == false)
                {
                    return false;
                }
            }

            return true;
        }


        public bool CanStart()
        {
            return CanStart(QuestManager.instance.localIdentifier);
        }

        public bool CanStart(ILocalIdentifier localIdentifier)
        {
//            if (status == DialogueStatus.Active)
//            {
//                return false;
//            }

            if (nodes.Length <= 1)
            {
                DevdogLogger.Log("Can't start a dialogue with with 0 or 1 nodes (start node). Add some nodes first.");
                return false;
            }

            foreach (var condition in conditions)
            {
                if (condition.CanStart(this, localIdentifier).status == false)
                {
                    return false;
                }
            }

            return true;
        }


        public bool CanStop()
        {
            return CanStop(QuestManager.instance.localIdentifier);
        }

        public bool CanStop(ILocalIdentifier localIdentifier)
        {
            if (status != DialogueStatus.Active)
            {
                return false;
            }

            foreach (var condition in conditions)
            {
                if (condition.CanStop(this, localIdentifier).status == false)
                {
                    return false;
                }
            }

            return true;
        }

#endregion

#region Actions

        // Renamed from Start() it's preserved by Unity and clashes with the variables...
        public bool StartDialogue(uint nodeStartIndex = 0)
        {
            return StartDialogue(null, nodeStartIndex);
        }

        public bool StartDialogue(IDialogueOwner dialogueOwner, uint nodeStartIndex = 0)
        {
            return StartDialogue(dialogueOwner, QuestManager.instance.localIdentifier, nodeStartIndex);
        }

        public bool StartDialogue(IDialogueOwner dialogueOwner, ILocalIdentifier localIdentifier, uint nodeStartIndex = 0)
        {
            if (CanStart(localIdentifier) == false)
            {
                return false;
            }

            // Start dialog
            DialogueManager.instance.SetCurrentDialogue(this, dialogueOwner);
            status = DialogueStatus.Active;
            currentNodeIndex = nodeStartIndex;

            MoveToNextNode(localIdentifier, dialogueOwner);

            DevdogLogger.LogVerbose("Started dialogue");

            return true;
        }

        public bool Stop(bool force = false)
        {
            return Stop(QuestManager.instance.localIdentifier);
        }

        public bool Stop(ILocalIdentifier localIdentifier, bool force = false)
        {
            if (CanStop(localIdentifier) == false && force == false)
            {
                return false;
            }

            // Stop dialog
            status = DialogueStatus.InActive;
            currentNodeIndex = 0;

            foreach (var node in nodes)
            {
                node.OnDialogueExit();
            }

            DialogueManager.instance.SetCurrentDialogue(null, null);
            DevdogLogger.LogVerbose("Stopped dialogue - Resetting current node index to 0 (entry node)");

            return true;
        }

        public void MoveToNextNode()
        {
            MoveToNextNode(QuestManager.instance.localIdentifier, DialogueManager.instance.currentDialogueOwner);
        }

        public void MoveToNextNode(ILocalIdentifier localIdentifier, IDialogueOwner owner)
        {
            var n = currentNode.GetNextNode();
            if (n != null)
            {
                MoveToNextNode(localIdentifier, n, owner);
            }
            else
            {
                if (currentNode.isLeafNode)
                {
                    // Got to end, stop.
                    Stop(localIdentifier);
                }
            }
        }

        public void MoveToNextNode(NodeBase node)
        {
            MoveToNextNode(node, DialogueManager.instance.currentDialogueOwner);
        }

        public void MoveToNextNode(NodeBase node, IDialogueOwner owner)
        {
            MoveToNextNode(QuestManager.instance.localIdentifier, node, owner);
        }

        protected void MoveToNextNode(ILocalIdentifier localIdentifier, NodeBase node, IDialogueOwner owner)
        {
            currentNode.OnExit();

            node.OnEnter(owner);
            currentNodeIndex = node.index;
            node.OnExecute(owner);
        }

#endregion
    }
}