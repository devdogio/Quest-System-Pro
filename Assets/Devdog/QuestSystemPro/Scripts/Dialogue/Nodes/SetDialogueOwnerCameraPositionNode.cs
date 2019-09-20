using Devdog.General;

namespace Devdog.QuestSystemPro.Dialogue
{
    [System.Serializable]
    [Summary("Set the dialogue owner's camera position. Useful when you want to create specific shots from your character.")]
    public class SetDialogueOwnerCameraPositionNode : SetCameraPositionNodeBase
    {
        public override DialogueCamera camera
        {
            get
            {
                if (DialogueManager.instance == null || DialogueManager.instance.currentDialogueOwner == null)
                {
                    return null;
                }

                return DialogueManager.instance.currentDialogueOwner.dialogueCamera;
            }
        }

        public override void OnExecute(IDialogueOwner dialogueOwner)
        {
            if (camera == null)
            {
                DevdogLogger.LogWarning("The dialogue owner's camera is not defined. Can't set position");
                Finish(true);
                return;
            }

            camera.SetCameraPosition(position);
            Finish(true);
        }
    }
}