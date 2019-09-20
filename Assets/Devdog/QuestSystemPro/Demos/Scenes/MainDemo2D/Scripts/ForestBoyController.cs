using UnityEngine;
using System.Collections;

namespace Devdog.QuestSystemPro.Demo
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class ForestBoyController : MonoBehaviour
    {
        [Header("Player")]
        public KeyCode hitButton = KeyCode.C;
        public float walkSpeed = 1f;
        public float jumpSpeed = 4f;
        public LayerMask groundDetectionLayerMask = -1;

        [Header("Camera Controls")]
        public Camera playerCamera;
        public float cameraMovementSpeed = 1.0f;
        public float edgeMovementLocation = 0.45f;
        public float edgeMovementSpeedMultiplier = 35.0f;


        private Vector3 _cameraAimPosition;


        private Rigidbody2D _rigid;
        private Animator _animator;
        protected void Awake()
        {
            _rigid = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        protected void Update()
        {
            DoPlayerInput();
            DoCameraFollow();
        }

        private void DoCameraFollow()
        {
            Vector3 screenVector = playerCamera.WorldToScreenPoint(transform.position);
            screenVector.x /= Screen.width;
            screenVector.y /= Screen.height;

            if (screenVector.x > 1 - edgeMovementLocation)
            {
                float startPosition = 1.0f - edgeMovementLocation;
                float mult = screenVector.x - startPosition;

                // Avoids aliasing
                if (mult > 0.015f)
                {
                    _cameraAimPosition.x += 0.001f * edgeMovementSpeedMultiplier * mult;
                }
            }
            else if (screenVector.x < edgeMovementLocation)
            {
                float mult = edgeMovementLocation - screenVector.x;

                // Avoids aliasing
                if (mult > 0.015f)
                {
                    _cameraAimPosition.x -= 0.001f * edgeMovementSpeedMultiplier * mult;
                }
            }

            playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, _cameraAimPosition, Time.deltaTime * cameraMovementSpeed);
        }

        private void DoPlayerInput()
        {
            float x = Input.GetAxis("Horizontal");

            _rigid.velocity = new Vector2(x*walkSpeed, _rigid.velocity.y);

            _animator.SetFloat("WalkSpeed", x);
            _animator.SetInteger("FacingDirection", x >= 0 ? 0 : 1);
            _animator.SetBool("Hit", Input.GetKeyDown(hitButton));

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                var hit = Physics2D.Raycast(transform.position, Vector2.down, 0.15f, groundDetectionLayerMask);
                Debug.DrawRay(transform.position, Vector3.down*0.15f, Color.blue, 0.1f, false);
                if (hit.collider != null)
                {
                    Jump();
                }
            }
        }

        protected void Jump()
        {
            _rigid.AddForce(new Vector2(0f, jumpSpeed), ForceMode2D.Force);
        }
    }
}