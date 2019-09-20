using UnityEngine;
using System.Collections;
using Devdog.General;
using Devdog.General.UI;

namespace Devdog.QuestSystemPro.Demo
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterWalker : MonoBehaviour, IPlayerInputCallbacks
    {
        [InspectorReadOnly]
        public bool isGrounded;

        [Header("Config")]
        public float walkSpeed = 5f;
        public float turnSpeed = 4f;
        public float jumpSpeed = 40f;
        public float gravity = 9.81f;

        public float rotationSpeed = 1.0f;

        public float heightModifier = 1.45f;

        [SerializeField]
        [Required]
        private Camera _cam;
        private CharacterController _controller;

        private Vector3 _moveDirection = Vector3.zero;


        protected virtual void Start()
        {
            _controller = GetComponent<CharacterController>();
        }

        public void SetInputActive(bool active)
        {
            this.enabled = active;
        }

        protected void Update()
        {
            if (UIUtility.isFocusedOnInput)
            {
                return;
            }

            RotateToCamera(false); // Don't force rotation only if we move
            if (isGrounded)
            {
                _moveDirection = CalcDirection();
                if (Input.GetButtonDown("Jump"))
                {
                    _moveDirection.y = jumpSpeed * heightModifier;
                }
            }

            _moveDirection.y -= gravity * Time.deltaTime;
            var flags = _controller.Move(_moveDirection * Time.deltaTime);
            isGrounded = (flags & CollisionFlags.CollidedBelow) != 0;

            float x = 0;
            if (Input.GetMouseButton(1))
            {
                x = Input.GetAxis("Mouse X") * turnSpeed;
            }
            else if (GetVertical() != 0 || GetHorizontal() != 0f)
            {
                x = Input.GetAxis("Mouse X") * turnSpeed;
            }

            transform.Rotate(0, x * rotationSpeed, 0);
        }

        public Vector3 CalcDirection()
        {
            _moveDirection = new Vector3(GetHorizontal(), 0, GetVertical());
            _moveDirection = _cam.transform.TransformDirection(_moveDirection);

            _moveDirection *= walkSpeed;
            _moveDirection.y = 0;

            return _moveDirection;
        }

        public void RotateToCamera(bool force)
        {
            Vector3 t = new Vector3(GetHorizontal(), 0, GetVertical());
            if (t != Vector3.zero || force == true)
            {
                Quaternion nRot = _cam.transform.rotation;
                nRot.x = 0;
                nRot.z = 0;

                transform.rotation = Quaternion.Lerp(transform.rotation, nRot, Time.deltaTime * rotationSpeed);
            }
        }

        public float GetVertical()
        {
            float vert = Input.GetAxis("Vertical");
            if (vert < 0)
            {
                vert = (vert / 2f); // Twice as slow backwards
            }

            return vert;
        }

        public float GetHorizontal()
        {
            return Input.GetAxis("Horizontal");
        }
    }
}