using Devdog.General;

namespace Devdog.QuestSystemPro.Dialogue
{
    [Category("Devdog/Camera")]
    public abstract class SetCameraPositionNodeBase : ActionNodeBase
    {
        [ShowInNode]
        [HideTypePicker]
        [HideGroup]
        public CameraPositionLookup position = new CameraPositionLookup();


        public abstract DialogueCamera camera { get; }

        public override void OnExecute(IDialogueOwner dialogueOwner)
        {
            camera.SetCameraPosition(position);
            Finish(true);
        }

        public override void OnDialogueExit()
        {
            DialogueCameraManager.instance.ResetMainCamera();
        }

        public override ValidationInfo Validate()
        {
            if (camera == null)
            {
                return new ValidationInfo(ValidationType.Warning, "DialogueCamera not found. If you're adding it at run-time you can ignore this message.");
            }

            var lookup = camera.GetCameraPosition(position.from);
            if (lookup == null)
            {
                return new ValidationInfo(ValidationType.Error, "DialogueCamera position with key " + position.from + " does not exist.");
            }

            if (string.IsNullOrEmpty(position.to) == false)
            {
                lookup = camera.GetCameraPosition(position.to);
                if (lookup == null)
                {
                    return new ValidationInfo(ValidationType.Error, "DialogueCamera position with key " + position.to + " does not exist.");
                }
            }

            return base.Validate();
        }
    }
}