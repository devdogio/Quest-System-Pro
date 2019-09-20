using System;
using Devdog.General;
using Devdog.QuestSystemPro.Dialogue.UI;
using UnityEngine;

namespace Devdog.QuestSystemPro.Dialogue
{
    [System.Serializable]
    [Summary("The PlayerDecisionNode is used to create a player decision moment. The amount of output edges (outgoing lines) should be equal to the amount of decisions.")]
    [Category("Devdog/Flow control")]
    public class PlayerDecisionNode : NodeBase, IPlayerDecisionNode
    {
        public override NodeUIBase uiPrefab
        {
            get { return QuestManager.instance.settingsDatabase.playerDecisionNodeUI; }
        }

        [ShowInNode]
        [SerializeField]
        [HideTypePicker]
        [HideGroup(false)]
        private PlayerDecision[] _playerDecisions = new PlayerDecision[0];
        public PlayerDecision[] playerDecisions
        {
            get { return _playerDecisions; }
            protected set { _playerDecisions = value; }
        }



        [NonSerialized]
        private int _playerDecisionIndex = -1;
        public int playerDecisionIndex
        {
            get { return _playerDecisionIndex; }
            set { _playerDecisionIndex = value; }
        }

        public void SetPlayerDecisionAndMoveToNextNode(int index)
        {
            this.playerDecisionIndex = index;
            Finish(true);
        }

        public override void OnExecute(IDialogueOwner dialogueOwner)
        {
            // No finish, we only finish IF the player sets a decision index.
        }

        public override NodeBase GetNextNode()
        {
            if (playerDecisionIndex >= 0)
            {
                if (edges[playerDecisionIndex].CanUse(owner))
                {
                    return owner.nodes[edges[playerDecisionIndex].toNodeIndex];
                }
            }

            return null;
        }

        public override ValidationInfo Validate()
        {
            if (playerDecisions.Length != edges.Length)
            {
                return new ValidationInfo(ValidationType.Warning, "There are more/less player decisions then there are exit edges (outgoing lines)");
            }

            return new ValidationInfo(ValidationType.Valid);
        }
    }
}