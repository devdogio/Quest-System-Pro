using System.Collections.Generic;
using Devdog.General;
using Devdog.General.Localization;
using Devdog.QuestSystemPro.Dialogue.UI;
using UnityEngine;

namespace Devdog.QuestSystemPro.Dialogue
{
    [System.Serializable]
    public abstract partial class NodeBase
    {
        [SerializeField]
        [InspectorReadOnly]
        [Header("General")]
        private uint _index;
        /// <summary>
        /// The index of this node (dialogue.nodes[index] == this)
        /// </summary>
        public uint index
        {
            get { return _index; }
            set { _index = value; }
        }


        [IgnoreCustomSerialization]
        public Dialogue owner { get; set; }
        public DialogueOwnerType ownerType = DialogueOwnerType.DialogueOwner;
        public bool useAutoFocus = true;


        /// <summary>
        /// The edges leading up to other nodes
        /// </summary>
        [HideInProperties]
        [SerializeField]
        protected Edge[] _edges = new Edge[0];

        public Edge[] edges
        {
            get { return _edges; }
            set { _edges = value; }
        }

        public bool isLeafNode
        {
            get { return edges.Length == 0; }
        }


        [ShowInNode]
        [TextArea]
        [SerializeField]
        protected LocalizedString _message = new LocalizedString();

        public string message
        {
            get { return _message.message; }
            set { _message.message = value; }
        }

        public LocalizedString localizedMessage
        {
            get { return _message; }
            set { _message = value; }
        }



        [Header("Audio & Visuals")]
        [HideTypePicker]
        public LocalizedAudioClipInfo audioInfo;
        [HideTypePicker]
        public MotionInfo motionInfo;

        /// <summary>
        /// A run-time only uiPrefab - This is the UI element used to display this node.
        /// </summary>
        [IgnoreCustomSerialization]
        public virtual NodeUIBase uiPrefab
        {
            get { return QuestManager.instance.settingsDatabase.defaultNodeUIPrefab; }
        }


#if UNITY_EDITOR

        [SerializeField]
        [HideInProperties]
        private Vector2 _editorPosition;

        public Vector2 editorPosition
        {
            get { return _editorPosition; }
            set
            {
                _editorPosition = value;
                _editorPosition.x = Mathf.RoundToInt(_editorPosition.x);
                _editorPosition.y = Mathf.RoundToInt(_editorPosition.y);
            }
        }

        public NodeStyle editorNodeStyle
        {
            get
            {
                switch (ownerType)
                {
                    default:
                    case DialogueOwnerType.DialogueOwner:
                        return NodeStyle.Default;
                    case DialogueOwnerType.Player:
                        return NodeStyle.Green;
//                    case DialogueOwnerType.None:
//                        return NodeStyle.Blue;
                }
            }
        }

#endif

        protected NodeBase()
        {
        }

        /// <summary>
        /// Called before the node is executed, and before the (UI) events are fired. 
        /// </summary>
        public virtual void OnEnter(IDialogueOwner dialogueOwner)
        { }

        public abstract void OnExecute(IDialogueOwner dialogueOwner);

        /// <summary>
        /// Called just before jumping to the next node.
        /// </summary>
        public virtual void OnExit()
        { }

        /// <summary>
        /// Called on ALL NODES when the dialogue is stopped.
        /// </summary>
        public virtual void OnDialogueExit()
        { }

        public void Finish(NodeBase moveToNode)
        {
            // If the owner already moved to another node ignore this MoveToNextNode() call.
            if (owner.currentNodeIndex == index)
            {
                owner.MoveToNextNode(moveToNode);
            }
        }

        public void Finish(bool autoMoveToNextNode)
        {
            if (autoMoveToNextNode || string.IsNullOrEmpty(message))
            {
                // If the owner already moved to another node ignore this MoveToNextNode() call.
                if (owner.currentNodeIndex == index)
                {
                    owner.MoveToNextNode();
                }
            }
        }

        public void Failed(MultiLangString message)
        {
            DevdogLogger.LogWarning("Couldn't complete node with message: " + message.message);
        }

        public void Error(MultiLangString message, bool abort)
        {
            DevdogLogger.LogError("Error on node with message: " + message.message);
        }

        public virtual NodeBase GetNextNode()
        {
            foreach (var edge in edges)
            {
                if (edge.CanUse(owner))
                {
                    return owner.nodes[edge.toNodeIndex];
                }
            }

            return null;
        }

        public virtual void NextNodes(IList<NodeBase> appendList)
        {
            foreach (var edge in edges)
            {
                appendList.Add(owner.nodes[edge.toNodeIndex]);
            }
        }

        public virtual bool CanUseNode()
        {
            return true;
        }

        public virtual bool CanViewNode()
        {
            return CanUseNode();
        }

        /// <summary>
        /// Is the node set up correctly and is it valid to use?
        /// </summary>
        public virtual ValidationInfo Validate()
        {
            return new ValidationInfo(ValidationType.Valid);
        }
    }
}