using UnityEngine;
using System.Collections;
using Devdog.General;

namespace Devdog.QuestSystemPro.Demo
{
    public class CameraMovement : MonoBehaviour
    {
        [Header("References")]
        public CharacterWalker target;

        [Header("Config")]
        public float targetHeight = 1.7f;
        public float distance = 12.0f;
        public float offsetFromWall = 0.1f;
        public int maxDistance = 20;
        public float minDistance = 0.6f;
        public float xSpeed = 200.0f;
        public float ySpeed = 200.0f;
        public int yMinLimit = -80;
        public int yMaxLimit = 80;
        public int zoomRate = 40;

        [Header("Advanced config")]
        public float rotationDampening = 3.0f;
        public float zoomDampening = 5.0f;
        public LayerMask collisionLayers = -1;
        public bool lockToRearOfTarget = false;
        public bool allowMouseInputX = true;
        public bool allowMouseInputY = true;

        private Quaternion _rotOffset;

        private float _xDeg = 0.0f;
        private float _yDeg = 0.0f;
        [InspectorReadOnly]
        public float currentDistance;

        [InspectorReadOnly]
        private float _desiredDistance;
        [InspectorReadOnly]
        private float _correctedDistance;
        private bool _rotateBehind = false;

        protected virtual void Start()
        {
            Vector3 angles = transform.eulerAngles;
            _xDeg = angles.x;
            _yDeg = angles.y;
            currentDistance = distance;
            _desiredDistance = distance;
            _correctedDistance = distance;

            var r = GetComponent<Rigidbody>();
            if (r != null)
            {
                r.freezeRotation = true;
            }

            if (lockToRearOfTarget)
            {
                _rotateBehind = true;
            }
        }

        private void LateUpdate()
        {
            if (Input.GetMouseButton(1))
            {
                if (allowMouseInputX == true)
                {
                    _xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                }
                else
                {
                    RotateBehindTarget();
                }

                if (allowMouseInputY == true)
                {
                    _yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
                }

                if (!lockToRearOfTarget)
                {
                    _rotateBehind = false;
                }

                _xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                _yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
                _yDeg = ClampAngle(_yDeg, yMinLimit, yMaxLimit);
            }
            else if (Mathf.Approximately(target.GetVertical(), 0f) == false && Mathf.Approximately(target.GetHorizontal(), 0f) == false && _rotateBehind)
            {
                RotateBehindTarget();
            }

            var rotation = Quaternion.Euler(_yDeg, _xDeg, 0);

            // Calculate the desired distance 
            _desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate; //  * Mathf.Abs(newDesDistance) 
            _desiredDistance = Mathf.Clamp(_desiredDistance, minDistance, maxDistance);
            _correctedDistance = _desiredDistance;

            // Calculate desired camera position
            var vTargetOffset = new Vector3(0, -targetHeight, 0);
            var position = target.transform.position - (rotation * Vector3.forward * _desiredDistance + vTargetOffset);

            // Check for collision using the true target's desired registration point as set by user using height 
            Vector3 trueTargetPosition = new Vector3(target.transform.position.x, target.transform.position.y + targetHeight, target.transform.position.z);

            bool isCorrected = false;
            RaycastHit collisionHit;
            if (Physics.Linecast(trueTargetPosition, position, out collisionHit, collisionLayers))
            {
                _correctedDistance = Vector3.Distance(trueTargetPosition, collisionHit.point) - offsetFromWall;
                isCorrected = true;
            }

            // For smoothing, lerp distance only if either distance wasn't corrected, or correctedDistance is more than currentDistance 
            currentDistance = !isCorrected || _correctedDistance > currentDistance ? Mathf.Lerp(currentDistance, _correctedDistance, Time.deltaTime * zoomDampening) : _correctedDistance;
            currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);

            position = target.transform.position - (rotation * Vector3.forward * currentDistance + vTargetOffset);

            transform.position = position;
            transform.rotation = rotation;
        }

        public void RotateBehindTarget()
        {
            float targetRotationAngle = target.transform.eulerAngles.y;
            float currentRotationAngle = transform.eulerAngles.y;
            _xDeg = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, rotationDampening * Time.deltaTime);

            if (Mathf.Approximately(targetRotationAngle, currentRotationAngle))
            {
                if (!lockToRearOfTarget)
                {
                    _rotateBehind = false;
                }
            }
            else
            {
                _rotateBehind = true;
            }
        }

        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360)
            {
                angle += 360;
            }

            if (angle > 360)
            {
                angle -= 360;
            }

            return Mathf.Clamp(angle, min, max);
        }
    }
}