using Joystick;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class MobileUIConfigurationStats : UIConfigurationStats
    {
        [SerializeField] private CustomJoystick _movementJoystick;
        [SerializeField] private CustomJoystick _lookJoystick;

        public CustomJoystick MovementJoystick => _movementJoystick;
        public CustomJoystick LookJoystick => _lookJoystick;
    }
}