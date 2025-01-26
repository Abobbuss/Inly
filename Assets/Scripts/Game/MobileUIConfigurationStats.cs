using Joystick;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    [System.Serializable]
    public class MobileUIConfigurationStats : UIConfigurationStats
    {
        [SerializeField] private CustomJoystick _movementJoystick;
        [SerializeField] private CustomJoystick _lookJoystick;
        [SerializeField] private Button _jumpButton;

        public CustomJoystick MovementJoystick => _movementJoystick;
        public CustomJoystick LookJoystick => _lookJoystick;
        public Button JumpButton => _jumpButton;
    }
}