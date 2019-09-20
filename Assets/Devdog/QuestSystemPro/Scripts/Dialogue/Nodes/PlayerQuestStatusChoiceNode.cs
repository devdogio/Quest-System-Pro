using System;
using Devdog.General;
using Devdog.General.Localization;
using Devdog.General.ThirdParty.UniLinq;
using Devdog.QuestSystemPro.Dialogue.UI;

namespace Devdog.QuestSystemPro.Dialogue
{
    [System.Serializable]
    [Summary("A node used to let the player choose if he/she wants to accept a quest or not..\n\nEdge 0 = Accept, Edge 1 = Decline")]
    [Category("Devdog/Quests")]
    public class PlayerQuestStatusChoiceNode : NodeBase, IPlayerDecisionNode
    {
        [System.Serializable]
        public class PlayerDecisionsQuestStatus
        {
            public QuestStatus questStatus;

            [HideTypePicker]
            [HideGroup(false)]
            [OnlyDerivedTypes(typeof(PlayerQuestStatusDecision))]
            public PlayerDecision[] playerDecisions = new PlayerDecision[0];
        }

        public override NodeUIBase uiPrefab
        {
            get { return QuestManager.instance.settingsDatabase.playerDecisionNodeUI; }
        }

        [HideGroup(false)]
        [ArrayControlOptions(canAddItems = false, canRemoveItems = false)]
        public PlayerDecisionsQuestStatus[] playerDecisionsByStatus = new PlayerDecisionsQuestStatus[]
        {
            new PlayerDecisionsQuestStatus()
            {
                questStatus = QuestStatus.InActive,
                playerDecisions = new PlayerDecision[]
                {
                    new PlayerQuestStatusDecision() { option = new LocalizedString("PlayerQuestStatusChoiceNode_Accept"), action = QuestStatusAction.Activate }, 
                    new PlayerQuestStatusDecision() { option = new LocalizedString("PlayerQuestStatusChoiceNode_Decline"), action = QuestStatusAction.Cancel }, 
                }
            },
            new PlayerDecisionsQuestStatus()
            {
                questStatus = QuestStatus.Active,
                playerDecisions = new PlayerDecision[]
                {
                    new PlayerQuestStatusDecision() { option = new LocalizedString("PlayerQuestStatusChoiceNode_Cancel"), action = QuestStatusAction.Cancel }, 
                }
            },
            new PlayerDecisionsQuestStatus()
            {
                questStatus = QuestStatus.Completed,
                playerDecisions = new PlayerDecision[]
                {
                }
            },
            new PlayerDecisionsQuestStatus()
            {
                questStatus = QuestStatus.Cancelled,
                playerDecisions = new PlayerDecision[]
                {
                    new PlayerQuestStatusDecision() { option = new LocalizedString("PlayerQuestStatusChoiceNode_Accept"), action = QuestStatusAction.Activate },
                    new PlayerQuestStatusDecision() { option = new LocalizedString("PlayerQuestStatusChoiceNode_Decline"), action = QuestStatusAction.Cancel },
                }
            },
        };

        public PlayerDecisionsQuestStatus completablePlayerDecision = new PlayerDecisionsQuestStatus()
        {
            questStatus = QuestStatus.Active,
            playerDecisions = new PlayerDecision[]
            {
                new PlayerQuestStatusDecision() {  option = new LocalizedString("PlayerQuestStatusChoiceNode_Complete"), action = QuestStatusAction.Complete },
                new PlayerQuestStatusDecision() { option = new LocalizedString("PlayerQuestStatusChoiceNode_Cancel"), action = QuestStatusAction.Cancel },
            }
        };

        [NonSerialized]
        private int _playerDecisionIndex = -1;
        public int playerDecisionIndex
        {
            get { return _playerDecisionIndex; }
            protected set { _playerDecisionIndex = value; }
        }

        public PlayerQuestStatusDecision playerDecision { get; protected set; }

        public PlayerDecision[] playerDecisions
        {
            get
            {
                if (quest.val.CanComplete().status)
                {
                    return completablePlayerDecision.playerDecisions;
                }

                var v = playerDecisionsByStatus.FirstOrDefault(o => o.questStatus == quest.val.status);
                if (v == null)
                {
                    DevdogLogger.LogVerbose("No player decisions found for quest status: " + quest.val.status);
                    return new PlayerDecision[0];
                }

                return v.playerDecisions;
            }
        }

        public QuestStatusAction playerAction { get; set; }

        [ShowInNode]
        [Required]
        public Asset<Quest> quest;

        public bool canUseNodeWhenCompleted = false;
        public bool canUseNodeWhenActive = true;


        public virtual void SetPlayerDecisionAndMoveToNextNode(int decisionIndex)
        {
            this.playerDecisionIndex = decisionIndex;
            this.playerDecision = (PlayerQuestStatusDecision)playerDecisions[decisionIndex];

            quest.val.DoAction(playerDecision.action);
            Finish(true);
        }

        public override void OnExecute(IDialogueOwner dialogueOwner)
        {
            // No finish, we only finish when the player sets the decision.
        }

        public override NodeBase GetNextNode()
        {
            if (playerDecisionIndex >= 0 && playerDecisionIndex < edges.Length)
            {
                var edge = edges[(int)playerDecision.action];
                if (edge.CanUse(owner))
                {
                    return owner.nodes[edge.toNodeIndex];
                }
            }

            return null;
        }

        public override bool CanUseNode()
        {
            bool completed = QuestManager.instance.HasCompletedQuest(quest.val);
            bool active = QuestManager.instance.HasActiveQuest(quest.val);
            if (completed && canUseNodeWhenCompleted == false)
            {
                return false;
            }

            if (active && canUseNodeWhenActive == false)
            {
                return false;
            }

            return true;
        }
    }
}