using UnityEngine;

namespace Game
{
    public class UIConfigurationGameScene : MonoBehaviour
    {
        [SerializeField] private UIConfigurationStats _pcConfigurationStats;
        [SerializeField] private MobileUIConfigurationStats _moblileConfigurationStats;

        public UIConfigurationStats PCConfig => _pcConfigurationStats;
        public MobileUIConfigurationStats MobileConfig => _moblileConfigurationStats;
    }
}