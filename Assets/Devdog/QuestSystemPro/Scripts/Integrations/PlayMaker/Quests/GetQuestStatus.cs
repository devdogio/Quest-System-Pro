#if PLAYMAKER

using HutongGames.PlayMaker;

namespace Devdog.QuestSystemPro.Integration.PlayMaker
{
    [ActionCategory(QuestSystemPro.ProductName)]
    [HutongGames.PlayMaker.Tooltip("Get a quest's status as a string.")]
    public class GetQuestStatus : FsmStateAction
    {
        [RequiredField]
        public Quest quest;
        public FsmString result;

        public override void OnEnter()
        {
            result.Value = quest.status.ToString();
            Finish();
        }
    }
}

#endif