#if PLAYMAKER

using HutongGames.PlayMaker;

namespace Devdog.QuestSystemPro.Integration.PlayMaker
{
    [ActionCategory(QuestSystemPro.ProductName)]
    [HutongGames.PlayMaker.Tooltip("Set a dialogue's string variable.")]
    public class SetDialogueStringVariable : FsmStateAction
    {
        [RequiredField]
        public Dialogue.Dialogue dialogue;

        [RequiredField]
        public FsmString variableName;

        public FsmString newValue;

        public override void OnEnter()
        {
            dialogue.variables.Get<string>(variableName.Value).value = newValue.Value;
            Finish();
        }
    }
}

#endif