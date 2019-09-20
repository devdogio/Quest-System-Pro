#if PLAYMAKER

using HutongGames.PlayMaker;

namespace Devdog.QuestSystemPro.Integration.PlayMaker
{
    [ActionCategory(QuestSystemPro.ProductName)]
    [HutongGames.PlayMaker.Tooltip("Move a dialogue to the next node.")]
    public class DialogueMoveToNextNode : FsmStateAction
    {
        [RequiredField]
        public Dialogue.Dialogue dialogue;
        

        public override void OnEnter()
        {
            dialogue.MoveToNextNode();
            Finish();
        }
    }
}

#endif