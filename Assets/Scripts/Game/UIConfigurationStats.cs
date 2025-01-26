using Timers;
using TMPro;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class UIConfigurationStats
    {
        [SerializeField] private GameObject uiRoot;
        [SerializeField] private Timer timer;
        [SerializeField] private TextMeshProUGUI averageBoostText;
        [SerializeField] private TextMeshProUGUI bonusCountText;

        public GameObject UIRoot => uiRoot;
        public Timer Timer => timer;
        public TextMeshProUGUI AverageBoostText => averageBoostText;
        public TextMeshProUGUI BonusCountText => bonusCountText;
    }
}