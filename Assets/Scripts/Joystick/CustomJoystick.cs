using UnityEngine;
using UnityEngine.EventSystems;

namespace Joystick
{
    public enum StickAxisControl { Both, Horizontal, Vertical }
    
    public class CustomJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform _baseCircle;
        [SerializeField] private RectTransform _stick;

        [SerializeField] private float _movementRange = 1f;
        [SerializeField] private float _deadZoneRadius;
        [SerializeField] private StickAxisControl _axisControl = StickAxisControl.Both;

        private Canvas _parentCanvas;
        private Camera _canvasCamera;
        private Vector2 _inputVector = Vector2.zero;
        
        public float Horizontal => _inputVector.x;
        public float Vertical => _inputVector.y;

        private void Awake()
        {
            InitializeJoystick();
        }

        private void InitializeJoystick()
        {
            _parentCanvas = GetComponentInParent<Canvas>();
            
            if (_parentCanvas == null)
                return;

            Vector2 centerPoint = new Vector2(0.5f, 0.5f);
            _baseCircle.pivot = centerPoint;
            _stick.anchorMin = centerPoint;
            _stick.anchorMax = centerPoint;
            _stick.pivot = centerPoint;
            _stick.anchoredPosition = Vector2.zero;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            HandleDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            HandleDrag(eventData);
        }

        private void HandleDrag(PointerEventData eventData)
        {
            _canvasCamera = _parentCanvas.renderMode == RenderMode.ScreenSpaceCamera ? _parentCanvas.worldCamera : null;

            Vector2 basePosition = RectTransformUtility.WorldToScreenPoint(_canvasCamera, _baseCircle.position);
            Vector2 radius = _baseCircle.sizeDelta / 2f;

            _inputVector = (eventData.position - basePosition) / (radius * _parentCanvas.scaleFactor);
            ConstrainInput();
            UpdateJoystickPosition(_inputVector.magnitude, _inputVector.normalized, radius);
        }

        private void ConstrainInput()
        {
            if (_axisControl == StickAxisControl.Horizontal)
            {
                _inputVector = new Vector2(_inputVector.x, 0f);
            }
            else if (_axisControl == StickAxisControl.Vertical)
            {
                _inputVector = new Vector2(0f, _inputVector.y);
            }
        }

        private void UpdateJoystickPosition(float magnitude, Vector2 normalized, Vector2 radius)
        {
            if (magnitude > _deadZoneRadius)
            {
                _inputVector = magnitude > 1f ? normalized : _inputVector;
            }
            else
            {
                _inputVector = Vector2.zero;
            }

            _stick.anchoredPosition = _inputVector * radius * _movementRange;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _inputVector = Vector2.zero;
            _stick.anchoredPosition = Vector2.zero;
        }
    } 
}
