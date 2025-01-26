using Game;
using Joystick;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float _walkSpeed = 5f;
        [SerializeField] private float _jumpHeight = 2f;
        [SerializeField] private float _gravity = -9.81f;
        
        [Header("Look Settings")]
        [SerializeField] private float _lookSensitivity = 0.5f;
        
        [Header("References")]
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private DeviceUIManager _deviceUIManager;

        private CustomJoystick movementJoystick;
        private CustomJoystick lookJoystick;
        private CharacterController characterController;
        private Vector2 moveInput;
        private Vector2 lookInput;

        private float verticalVelocity;
        private float cameraRotationX;

        private bool isMobile;

        private PlayerInput playerInput;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            playerInput = new PlayerInput();
            
            playerInput.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            playerInput.Player.Move.canceled += ctx => moveInput = Vector2.zero;
            playerInput.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
            playerInput.Player.Look.canceled += ctx => lookInput = Vector2.zero;
            playerInput.Player.Jump.performed += ctx => OnJumpButton();

            isMobile = _deviceUIManager.IsMobileUIActive;

            if (isMobile)
                SetJoysticks();
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnEnable()
        {
            playerInput.Enable();
            _deviceUIManager.OnOrientationChanged += DeviceUIManagerOnOnOrientationChanged;
        }

        private void OnDisable()
        {
            playerInput.Disable();
            _deviceUIManager.OnOrientationChanged -= DeviceUIManagerOnOnOrientationChanged;
        }

        private void Update()
        {
            HandleMovement();
            HandleLook();
            HandleGravity();
        }
        
        private void DeviceUIManagerOnOnOrientationChanged()
        {
            SetJoysticks();
        }
        
        private void SetJoysticks()
        {
            if (_deviceUIManager.GetActiveUIConfiguration() is not MobileUIConfigurationStats mobileConfig) 
                return;
            
            movementJoystick = mobileConfig.MovementJoystick;
            lookJoystick = mobileConfig.LookJoystick;
        }

        private void HandleMovement()
        {
            Vector3 moveDirection;
            if (isMobile)
                moveDirection = transform.forward * movementJoystick.Vertical + transform.right * movementJoystick.Horizontal;
            else
                moveDirection = transform.forward * moveInput.y + transform.right * moveInput.x;

            characterController.Move(moveDirection * (_walkSpeed * Time.deltaTime));
        }

        private void HandleLook()
        {
            float lookX;
            float lookY;

            if (isMobile)
            {
                lookX = lookJoystick.Horizontal * _lookSensitivity;
                lookY = lookJoystick.Vertical * _lookSensitivity;
            }
            else
            {
                lookX = lookInput.x * _lookSensitivity;
                lookY = lookInput.y * _lookSensitivity;
            }

            transform.Rotate(Vector3.up, lookX);

            cameraRotationX -= lookY;
            cameraRotationX = Mathf.Clamp(cameraRotationX, -90f, 90f);
            _cameraTransform.localEulerAngles = new Vector3(cameraRotationX, 0f, 0f);
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
                verticalVelocity += _gravity * Time.deltaTime;
            }

            characterController.Move(Vector3.up * (verticalVelocity * Time.deltaTime));
        }

        public void OnJumpButton()
        {
            if (characterController.isGrounded)
                verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }
    }
}
