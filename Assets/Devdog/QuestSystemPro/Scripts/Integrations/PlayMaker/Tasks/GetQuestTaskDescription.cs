#if PLAYMAKER

using HutongGames.PlayMaker;

namespace Devdog.QuestSystemPro.Integration.PlayMaker
{
    [ActionCategory(QuestSystemPro.ProductName)]
    [HutongGames.PlayMaker.Tooltip("Get a quest task's description.")]
    public class GetQuestTaskDescription : FsmStateAction
    {
        [RequiredField]
        public Quest quest;

        [RequiredField]
        public FsmString taskName;

        public FsmString result;

        public override void OnEnter()
        {
            result.Value = quest.GetTask(taskName.Value).description.message;
            Finish();
        }
    }
}

#endif