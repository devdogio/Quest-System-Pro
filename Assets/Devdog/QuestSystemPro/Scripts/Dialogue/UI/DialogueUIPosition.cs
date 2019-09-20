using System;
using Devdog.General;
using UnityEngine;

namespace Devdog.QuestSystemPro.Dialogue.UI
{
    public class DialogueUIPosition : MonoBehaviour
    {
        public Vector3 offset;

        private NodeBase _currentNode;
        protected void Start()
        {
            DialogueManager.instance.OnCurrentDialogueNodeChanged += OnCurrentDialogueNodeChanged;
        }

        private void OnCurrentDialogueNodeChanged(NodeBase before, NodeBase after)
        {
            _currentNode = after;
        }

        protected void LateUpdate()
        {
            if (_currentNode == null)
            {
                return;
            }

            switch (_currentNode.ownerType)
            {
                case DialogueOwnerType.Player:
                    PositionAt(PlayerManager.instance.currentPlayer.transform.position);
                    break;
                case DialogueOwnerType.DialogueOwner:
                    if (DialogueManager.instance.currentDialogueOwner != null)
                    {
                        PositionAt(DialogueManager.instance.currentDialogueOwner.transform.position);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PositionAt(Vector3 worldSpace)
        {
            if (Camera.current != null)
            {
                var screenSpace = Camera.current.WorldToScreenPoint(worldSpace);
                screenSpace += offset;

                transform.position = screenSpace;
            }
        }
    }
}