#if PLAYMAKER

using System;
using HutongGames.PlayMaker;

namespace Devdog.QuestSystemPro.Integration.PlayMaker
{
    [ActionCategory(QuestSystemPro.ProductName)]
    [HutongGames.PlayMaker.Tooltip("Set a quest tasks' progress.")]
    public class SetQuestTaskProgress : FsmStateAction
    {
        public enum Option
        {
            Add,
            Set
        }

        [RequiredField]
        public Quest quest;

        [RequiredField]
        public FsmString taskName;

        public Option option = Option.Add;

        [RequiredField]
        public FsmFloat set;

        public FsmFloat result;

        public override void OnEnter()
        {
            switch (option)
            {
                case Option.Add:
                    quest.ChangeTaskProgress(taskName.Value, set.Value);
                    break;
                case Option.Set:
                    quest.SetTaskProgress(taskName.Value, set.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            result.Value = quest.GetTask(taskName.Value).progress;
            Finish();
        }
    }
}

#endif