using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Devdog.QuestSystemPro.Dialogue
{
    [AddComponentMenu(QuestSystemPro.AddComponentMenuPath + "Managers/Dialogue Camera Manager")]
    public partial class DialogueCameraManager : MonoBehaviour
    {
        public class CameraPositionLookupKeyValuePair
        {
            public DialogueCamera camera;
            public CameraPositionLookup lookup;
        }

        protected Queue<CameraPositionLookupKeyValuePair> queue = new Queue<CameraPositionLookupKeyValuePair>();


        public Camera mainCamera { get; protected set; }
        protected virtual void Awake()
        {
            mainCamera = Camera.main;
        }

        private static CameraPositionLookupKeyValuePair _current;
        private static bool _requestCancel = false;

        private static DialogueCameraManager _instance;
        public static DialogueCameraManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<DialogueCameraManager>();
                }

                return _instance;
            }
        }


        protected virtual void Start()
        {
            StartCoroutine(RunQueue());
        }

        public void ResetMainCamera()
        {
            _requestCancel = true;
            ClearQueue();
            SetMainCameraActive(true);
        }

        protected void SetMainCameraActive(bool setActive)
        {
            if (setActive && _current != null)
            {
                _current.camera.ResetCamera();
            }

            if (mainCamera != null && mainCamera.gameObject.activeSelf != setActive)
            {
                mainCamera.gameObject.SetActive(setActive);
            }
        }

        protected IEnumerator RunQueue()
        {
            while (true)
            {
                if (queue.Count > 0)
                {
                    SetMainCameraActive(false);

                    _current = queue.Dequeue();
                    var from = _current.camera.GetCameraPosition(_current.lookup.from);
                    var to = _current.camera.GetCameraPosition(_current.lookup.to);

                    Assert.IsNotNull(from, "Trying to set dialogue camera position but 'from' is empty on lookup.");
                    if(_current == null || _current.camera == null)
                    {
                        continue;
                    }

                    _current.camera.Activate(from);

                    if (_current.lookup.duration > 0f)
                    {
                        yield return StartCoroutine(HandleQueueItem(_current, from, to));
                    }
                    else
                    {
                        SetPositionAndRotation(_current.camera, from);
                    }

                    if (_current.lookup.duration <= 0f)
                    {
                        // If we set a position without time we wait until a new position is added to the queue; Or stop when it's cancelled.
                        while (queue.Count == 0)
                        {
                            if (_requestCancel)
                            {
                                _requestCancel = false;
                                break;
                            }

                            yield return null;
                        }
                    }

                    if (queue.Count > 0)
                    {
                        var next = queue.Peek();
                        if (next.camera != _current.camera)
                        {
                            _current.camera.ResetCamera();
                        }
                    }
                    else
                    {
                        _current.camera.ResetCamera();
                    }
                }

                _current = null;
                SetMainCameraActive(true);
                if (queue.Count == 0)
                {
                    yield return null;
                }
            }
        }

        protected IEnumerator HandleQueueItem(CameraPositionLookupKeyValuePair kvp, CameraPosition from, CameraPosition to)
        {
            float timer = 0f;
            if (string.IsNullOrEmpty(kvp.lookup.to))
            {
                SetPositionAndRotation(kvp.camera, from);
                while (timer < kvp.lookup.duration)
                {
                    timer += Time.deltaTime;
                    yield return null;
                }
            }
            else
            {
                while (timer < kvp.lookup.duration)
                {
                    var timeNormalized = timer / kvp.lookup.duration;
                    var val = kvp.lookup.animationCurve.Evaluate(timeNormalized);

                    SetPositionAndRotation(kvp.camera, Vector3.Lerp(from.position, to.position, val), Quaternion.Lerp(from.rotation, to.rotation, val), from.useWorldSpace);

                    timer += Time.deltaTime;
                    yield return null;
                }
            }
        }

        public void SetPositionAndRotation(DialogueCamera camera, CameraPosition pos)
        {
            Assert.IsNotNull(pos);
            SetPositionAndRotation(camera, pos.position, pos.rotation, pos.useWorldSpace);
        }

        public void SetPositionAndRotation(DialogueCamera camera, Vector3 pos, Quaternion rot, bool useWorldspace)
        {
            if (useWorldspace)
            {
                camera.transform.position = pos;
                camera.transform.rotation = rot;
            }
            else
            {
                camera.transform.localPosition = pos;
                camera.transform.localRotation = rot;
            }
        }

        public void ClearQueue()
        {
            queue.Clear();
        }

        public void Enqueue(CameraPositionLookupKeyValuePair lookup)
        {
            queue.Enqueue(lookup);
        }

        public void Enqueue(CameraPositionLookup lookup, DialogueCamera camera)
        {
            Enqueue(new CameraPositionLookupKeyValuePair() { camera = camera, lookup = lookup });
        }
    }
}