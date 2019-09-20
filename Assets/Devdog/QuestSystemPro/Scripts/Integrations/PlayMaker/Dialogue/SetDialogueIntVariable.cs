#if PLAYMAKER

using HutongGames.PlayMaker;

namespace Devdog.QuestSystemPro.Integration.PlayMaker
{
    [ActionCategory(QuestSystemPro.ProductName)]
    [HutongGames.PlayMaker.Tooltip("Set a dialogue's int variable.")]
    public class SetDialogueIntVariable : FsmStateAction
    {
        [RequiredField]
        public Dialogue.Dialogue dialogue;

        [RequiredField]
        public FsmString variableName;

        public FsmInt newValue;

        public override void OnEnter()
        {
            dialogue.variables.Get<int>(variableName.Value).value = newValue.Value;
            Finish();
        }
    }
}

#endif