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
        [SerializeField] private GameScreenManager _gameScreenManager;
        
        [SerializeField] private CustomJoystick _movementJoystick;
        private CustomJoystick _lookJoystick;
        private CharacterController _characterController;
        private Vector2 _moveInput;
        private Vector2 _lookInput;

        private float verticalVelocity;
        private float cameraRotationX;

        private bool _isMobile;

        private PlayerInput _playerInput;
        private UnityEngine.UI.Button _jumpButton;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _playerInput = new PlayerInput();
            
            _playerInput.Player.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
            _playerInput.Player.Move.canceled += ctx => _moveInput = Vector2.zero;
            _playerInput.Player.Look.performed += ctx => _lookInput = ctx.ReadValue<Vector2>();
            _playerInput.Player.Look.canceled += ctx => _lookInput = Vector2.zero;
            _playerInput.Player.Jump.performed += ctx => OnJumpButton();
            _playerInput.Player.Pause.performed += _ => TogglePause();

            _isMobile = _deviceUIManager.IsMobileUIActive;
        }

        private void OnEnable()
        {
            _playerInput.Enable();
            _deviceUIManager.SetActiveConfig += DeviceUIManagerOnSetActiveConfig;
        }

        private void OnDisable()
        {
            _playerInput.Disable();
            _deviceUIManager.SetActiveConfig -= DeviceUIManagerOnSetActiveConfig;
        }

        private void Start()
        {
            _isMobile = _deviceUIManager.IsMobileUIActive;

            if (_isMobile)
            {
                SetUIButtons();
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;   
            }
        }

        private void Update()
        {
            HandleMovement();
            HandleLook();
            HandleGravity();
        }
        
        private void DeviceUIManagerOnSetActiveConfig()
        {
            if (_isMobile)
                SetUIButtons();
        }
        
        private void TogglePause()
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                _gameScreenManager.ShowGameplayScreen();
            }
            else
            {
                Time.timeScale = 0;
                _gameScreenManager.ShowPauseScreen();
            }
        }
        
        private void SetUIButtons()
        {
            if (_deviceUIManager.GetActiveUIConfiguration() is not MobileUIConfigurationStats mobileConfig) 
                return;
            
            _movementJoystick = mobileConfig.MovementJoystick;
            _lookJoystick = mobileConfig.LookJoystick;
            _jumpButton = mobileConfig.JumpButton;
            
            _jumpButton.onClick.AddListener(OnJumpButton);
        }

        private void HandleMovement()
        {
            Vector3 moveDirection;
            if (_isMobile)
                moveDirection = transform.forward * _movementJoystick.Vertical + transform.right * _movementJoystick.Horizontal;
            else
                moveDirection = transform.forward * _moveInput.y + transform.right * _moveInput.x;

            _characterController.Move(moveDirection * (_walkSpeed * Time.deltaTime));
        }

        private void HandleLook()
        {
            float lookX;
            float lookY;

            if (_isMobile)
            {
                lookX = _lookJoystick.Horizontal * _lookSensitivity;
                lookY = _lookJoystick.Vertical * _lookSensitivity;
            }
            else
            {
                lookX = _lookInput.x * _lookSensitivity;
                lookY = _lookInput.y * _lookSensitivity;
            }

            transform.Rotate(Vector3.up, lookX);

            cameraRotationX -= lookY;
            cameraRotationX = Mathf.Clamp(cameraRotationX, -90f, 90f);
            _cameraTransform.localEulerAngles = new Vector3(cameraRotationX, 0f, 0f);
        }

        private void HandleGravity()
        {
            if (_characterController.isGrounded)
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

            _characterController.Move(Vector3.up * (verticalVelocity * Time.deltaTime));
        }

        public void OnJumpButton()
        {
            if (_characterController.isGrounded)
                verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }
        
        private void OnDestroy()
        {
            if (_jumpButton != null)
                _jumpButton.onClick.RemoveListener(OnJumpButton);
        }
    }
}
