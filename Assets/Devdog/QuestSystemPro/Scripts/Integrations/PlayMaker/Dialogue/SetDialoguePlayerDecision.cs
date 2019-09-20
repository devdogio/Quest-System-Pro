#if PLAYMAKER

using Devdog.QuestSystemPro.Dialogue;
using HutongGames.PlayMaker;

namespace Devdog.QuestSystemPro.Integration.PlayMaker
{
    [ActionCategory(QuestSystemPro.ProductName)]
    [HutongGames.PlayMaker.Tooltip("Set a dialogue's player decision.")]
    public class SetDialoguePlayerDecision : FsmStateAction
    {
        [RequiredField]
        public Dialogue.Dialogue dialogue;

        public FsmInt playerDecisionIndex;

        public override void OnEnter()
        {
            var node = dialogue.currentNode as IPlayerDecisionNode;
            if (node != null)
            {
                node.SetPlayerDecisionAndMoveToNextNode(playerDecisionIndex.Value);
            }
            else
            {
                LogWarning("Current node is not an IPlayerDecisionNode");
            }

            Finish();
        }
    }
}

#endif