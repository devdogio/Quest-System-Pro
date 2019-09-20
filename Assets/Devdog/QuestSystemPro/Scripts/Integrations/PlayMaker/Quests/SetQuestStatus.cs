#if PLAYMAKER

using HutongGames.PlayMaker;

namespace Devdog.QuestSystemPro.Integration.PlayMaker
{
    [ActionCategory(QuestSystemPro.ProductName)]
    [HutongGames.PlayMaker.Tooltip("Set a quest's status.")]
    public class SetQuestStatus : FsmStateAction
    {
        [RequiredField]
        public Quest quest;

        public QuestStatusAction action;

        public override void OnEnter()
        {
            quest.DoAction(action);
            Finish();
        }
    }
}

#endif