using UnityEngine;

namespace Devdog.QuestSystemPro.Dialogue
{
    public abstract class AutoFocusBase : MonoBehaviour
    {
        public bool useInterpolation = false;
        public float interpolationTime = 0.5f;
//        public AnimationCurve interpolationCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        public string[] focusNames = new string[0];

        protected DialogueCamera dialogueCamera;
        protected DialogueOwnerType type;

        protected virtual void Awake()
        {
            SetDialogueCamera();
        }

        protected abstract void SetDialogueCamera();

        protected virtual void Start()
        {
            RegisterEvent();
        }

        protected abstract void RegisterEvent();

        protected void OnNodeChanged(NodeBase before, NodeBase after)
        {
            if (after.useAutoFocus == false)
            {
                return;
            }

            if (after.index == 0)
            {
                ResetCameraPosition();
            }
            else
            {
                if (after.ownerType == type)
                {
                    SetCameraPosition();
                }
            }
        }

        protected virtual void SetCameraPosition()
        {
            if (useInterpolation && focusNames.Length >= 2)
            {
                // TODO: Randomly assign these - Make sure r1 != r2.
                var r1 = focusNames[0];
                var r2 = focusNames[1];
                var position = new CameraPositionLookup()
                {
                    duration = interpolationTime, from = r1, to = r2
                };

                dialogueCamera.SetCameraPosition(position);
            }
            else
            {
                var r1 = focusNames[UnityEngine.Random.Range(0, focusNames.Length)];
                dialogueCamera.SetCameraPosition(r1);
            }
        }

        protected virtual void ResetCameraPosition()
        {
            DialogueCameraManager.instance.ResetMainCamera();
//            dialogueCamera.ResetCamera();
        }
    }
}
