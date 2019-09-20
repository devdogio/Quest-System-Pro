#if PLAYMAKER

using HutongGames.PlayMaker;

namespace Devdog.QuestSystemPro.Integration.PlayMaker
{
    [ActionCategory(QuestSystemPro.ProductName)]
    [HutongGames.PlayMaker.Tooltip("Reset a quest's progress.")]
    public class ResetQuestTaskProgress : FsmStateAction
    {
        [RequiredField]
        public Quest quest;

        public override void OnEnter()
        {
            quest.ResetProgress();
            Finish();
        }
    }
}

#endif