using Joystick;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float walkSpeed = 5f;
        public float jumpHeight = 2f;
        public float gravity = -9.81f;

        [Header("Look Settings")]
        public float lookSensitivity = 0.5f;

        [Header("References")]
        public Transform cameraTransform;
        public GameObject mobileControlsUI;
        public CustomJoystick movementJoystick;
        public CustomJoystick lookJoystick;

        [Header("Debug Settings")]
        public bool forceMobileControlInEditor;

        private CharacterController characterController;
        private Vector2 moveInput;
        private Vector2 lookInput;

        private float verticalVelocity;
        private float cameraRotationX;

        private bool isMobile;

        private PlayerInput playerInput;

        [SerializeField] private float speedX;
        [SerializeField] private float speedY;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            playerInput = new PlayerInput();
            
            playerInput.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            playerInput.Player.Move.canceled += ctx => moveInput = Vector2.zero;
            playerInput.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
            playerInput.Player.Look.canceled += ctx => lookInput = Vector2.zero;
            playerInput.Player.Jump.performed += ctx => OnJumpButton();

            isMobile = Application.isMobilePlatform || forceMobileControlInEditor;

            if (mobileControlsUI != null)
            {
                mobileControlsUI.SetActive(isMobile);
            }

            if (!isMobile)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        private void OnEnable()
        {
            playerInput.Enable();
        }

        private void OnDisable()
        {
            playerInput.Disable();
        }

        private void Update()
        {
            HandleMovement();
            HandleLook();
            HandleGravity();
        }

        private void HandleMovement()
        {
            Vector3 moveDirection;
            if (isMobile)
            {
                moveDirection = transform.forward * movementJoystick.Vertical + transform.right * movementJoystick.Horizontal;
            }
            else
            {
                moveDirection = transform.forward * moveInput.y + transform.right * moveInput.x;
            }

            characterController.Move(moveDirection * walkSpeed * Time.deltaTime);
        }

        private void HandleLook()
        {
            float lookX = 0f;
            float lookY = 0f;

            if (isMobile)
            {
                lookX = lookJoystick.Horizontal * lookSensitivity;
                lookY = lookJoystick.Vertical * lookSensitivity;
            }
            else
            {
                if (lookInput.sqrMagnitude > 0.01f) 
                {
                    lookX = lookInput.x * lookSensitivity;
                    lookY = lookInput.y * lookSensitivity;
                }
            }

            transform.Rotate(Vector3.up, lookX);

            cameraRotationX -= lookY;
            cameraRotationX = Mathf.Clamp(cameraRotationX, -90f, 90f);
            cameraTransform.localEulerAngles = new Vector3(cameraRotationX, 0f, 0f);
        }

        private void HandleGravity()
        {
            if (characterController.isGrounded)
            {
                if (verticalVelocity < 0)
                {
                    verticalVelocity = -1f;
                }
            }
            else
            {
                verticalVelocity += gravity * Time.deltaTime;
            }

            characterController.Move(Vector3.up * verticalVelocity * Time.deltaTime);
        }

        public void OnJumpButton()
        {
            if (characterController.isGrounded)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }
    }
}
