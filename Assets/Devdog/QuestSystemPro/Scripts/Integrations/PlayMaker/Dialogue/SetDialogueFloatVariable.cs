#if PLAYMAKER

using HutongGames.PlayMaker;

namespace Devdog.QuestSystemPro.Integration.PlayMaker
{
    [ActionCategory(QuestSystemPro.ProductName)]
    [HutongGames.PlayMaker.Tooltip("Set a dialogue's float variable.")]
    public class SetDialogueFloatVariable : FsmStateAction
    {
        [RequiredField]
        public Dialogue.Dialogue dialogue;

        [RequiredField]
        public FsmString variableName;

        public FsmFloat newValue;

        public override void OnEnter()
        {
            dialogue.variables.Get<float>(variableName.Value).value = newValue.Value;
            Finish();
        }
    }
}

#endif