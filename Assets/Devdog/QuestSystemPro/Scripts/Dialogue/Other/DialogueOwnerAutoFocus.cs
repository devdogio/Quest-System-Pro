using UnityEngine.Assertions;

namespace Devdog.QuestSystemPro.Dialogue
{
    public class DialogueOwnerAutoFocus : AutoFocusBase
    {

        private IDialogueOwner _dialogueOwner;

        protected override void Awake()
        {
            base.Awake();
            this.type = DialogueOwnerType.DialogueOwner;
        }

        protected override void SetDialogueCamera()
        {
            _dialogueOwner = GetComponent<IDialogueOwner>();
            Assert.IsNotNull(_dialogueOwner, "No IDialogueOwner found on DialogueOwnerAutoFocus component.");
            Assert.IsNotNull(_dialogueOwner.dialogueCamera, "IDialogueOwner found, but it has no camera, can't auto focus.");

            dialogueCamera = _dialogueOwner.dialogueCamera;
        }

        protected override void RegisterEvent()
        {
            _dialogueOwner.dialogue.OnCurrentNodeChanged += OnNodeChanged;
        }
    }
}
