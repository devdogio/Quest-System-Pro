#if PLAYMAKER

using System;
using Devdog.QuestSystemPro.Dialogue;
using HutongGames.PlayMaker;

namespace Devdog.QuestSystemPro.Integration.PlayMaker
{
    [ActionCategory(QuestSystemPro.ProductName)]
    [HutongGames.PlayMaker.Tooltip("Set a dialogue's status.")]
    public class SetDialogueStatus : FsmStateAction
    {
        [RequiredField]
        public Dialogue.Dialogue dialogue;

        public DialogueStatus status;

        public override void OnEnter()
        {
            switch (status)
            {
                case DialogueStatus.InActive:
                    dialogue.StartDialogue();
                    break;
                case DialogueStatus.Active:
                    dialogue.Stop();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Finish();
        }
    }
}

#endif