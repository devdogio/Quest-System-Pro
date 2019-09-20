using Devdog.General;

namespace Devdog.QuestSystemPro.Dialogue
{
    [System.Serializable]
    [Summary("Set the player's camera position. Useful when you want to create specific shots for your character.")]
    public class SetPlayerCameraPositionNode : SetCameraPositionNodeBase
    {
        private DialogueCamera _camera;
        public override DialogueCamera camera
        {
            get
            {
                if (_camera == null)
                {
                    if (QuestManager.instance != null && PlayerManager.instance.currentPlayer != null)
                    {
                       _camera = PlayerManager.instance.currentPlayer.questSystemPlayer.dialogueCamera;
                    }

#if UNITY_EDITOR
                    if (_camera == null)
                    {
                        var p = UnityEngine.Object.FindObjectOfType<Player>();
                        if (p != null)
                        {
                            _camera = p.questSystemPlayer.dialogueCamera;
                        }
                    }
#endif
                }

                return _camera;
            }
        }

        public override void OnExecute(IDialogueOwner dialogueOwner)
        {
            if (camera == null)
            {
                DevdogLogger.LogWarning("The player's camera is not defined. Can't set position");
                Finish(true);
                return;
            }

            camera.SetCameraPosition(position);
            Finish(true);
        }
    }
}