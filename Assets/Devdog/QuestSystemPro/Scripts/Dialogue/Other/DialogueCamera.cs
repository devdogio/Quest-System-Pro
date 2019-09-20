using System.Collections.Generic;
using Devdog.General.ThirdParty.UniLinq;
using UnityEngine;

namespace Devdog.QuestSystemPro.Dialogue
{
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(AudioListener))]
    public class DialogueCamera : MonoBehaviour
    {
        public CameraPosition[] lookups = new CameraPosition[0];

        public string uniqueName;

        public bool isMainCamera
        {
            get { return gameObject.CompareTag("MainCamera"); }
        }

        private static Dictionary<string, DialogueCamera> _cameraLookups;
        protected static Dictionary<string, DialogueCamera> cameraLookups
        {
            get
            {
                if (_cameraLookups == null)
                {
                    _cameraLookups = new Dictionary<string, DialogueCamera>();
                }

                return _cameraLookups;
            }
        }

        public static AnimationCurve defaultAnimationCurve
        {
            get { return AnimationCurve.EaseInOut(0f, 0f, 1f, 1f); }
        }

        private CameraPosition _activatePosition;
        protected virtual void Awake()
        {
            if (string.IsNullOrEmpty(uniqueName) == false)
            {
                cameraLookups[uniqueName] = this;
            }

            if (isMainCamera == false)
            {
                gameObject.SetActive(false);
            }
        }

        protected virtual void Start()
        {

        }

        public static DialogueCamera GetCamera(string name)
        {
#if UNITY_EDITOR
            if (Application.isPlaying == false && Application.isEditor)
            {
                var cams = FindObjectsOfType<DialogueCamera>();
                return cams.FirstOrDefault(o => o.uniqueName == name);
            }
#endif

            name = name ?? "";
            if (cameraLookups.ContainsKey(name))
            {
                return cameraLookups[name];
            }

//            DevdogLogger.LogVerbose("DialogueCamera with UniqueName: " + name + " not found.");
            return null;
        }

        public void Activate(CameraPosition activatePosition)
        {
            if (gameObject.activeSelf == false)
            {
                gameObject.SetActive(true);
            }

            _activatePosition = activatePosition;
        }

        public virtual CameraPosition GetCameraPosition(string key)
        {
            foreach (var l in lookups)
            {
                if (l.key == key)
                {
                    return l;
                }
            }

            return null;
        }

        public virtual void ResetCamera()
        {
            if (isMainCamera == false)
            {
                gameObject.SetActive(false);
            }

            if (_activatePosition != null)
            {
                SetCameraPosition(_activatePosition);
            }
        }

        public virtual void SetCameraPosition(CameraPosition position)
        {
            DialogueCameraManager.instance.SetPositionAndRotation(this, position);
        }

        public virtual void SetCameraPosition(string key)
        {
            DialogueCameraManager.instance.Enqueue(new CameraPositionLookup() { animationCurve = defaultAnimationCurve, duration = -1f, from = key, to = string.Empty }, this);
        }

        public virtual void SetCameraPosition(CameraPositionLookup lookup)
        {
            DialogueCameraManager.instance.Enqueue(lookup, this);
        }
    }
}
