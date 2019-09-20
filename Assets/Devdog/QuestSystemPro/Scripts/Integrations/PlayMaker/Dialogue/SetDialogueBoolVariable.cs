#if PLAYMAKER

using HutongGames.PlayMaker;

namespace Devdog.QuestSystemPro.Integration.PlayMaker
{
    [ActionCategory(QuestSystemPro.ProductName)]
    [HutongGames.PlayMaker.Tooltip("Set a dialogue's bool variable.")]
    public class SetDialogueBoolVariable : FsmStateAction
    {
        [RequiredField]
        public Dialogue.Dialogue dialogue;

        [RequiredField]
        public FsmString variableName;

        public FsmBool newValue;

        public override void OnEnter()
        {
            dialogue.variables.Get<bool>(variableName.Value).value = newValue.Value;
            Finish();
        }
    }
}

#endif