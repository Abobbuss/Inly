using Timers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    [System.Serializable]
    public class UIConfigurationStats
    {
        [SerializeField] private GameObject _uiRoot;
        [SerializeField] private Timer _timer;
        [SerializeField] private TextMeshProUGUI _averageBoostText;
        [SerializeField] private TextMeshProUGUI _bonusCountText;

        public GameObject UIRoot => _uiRoot;
        public Timer Timer => _timer;
        public TextMeshProUGUI AverageBoostText => _averageBoostText;
        public TextMeshProUGUI BonusCountText => _bonusCountText;
    }
}