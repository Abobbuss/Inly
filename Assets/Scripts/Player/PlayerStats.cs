using Game;
using Timers;
using UnityEngine;

namespace Player
{
    public class PlayerStats : MonoBehaviour
    {
        [SerializeField] private DeviceUIManager _deviceUIManager;
        [SerializeField] private GameState _gameState;
        
        private Timer _timer;
        private TMPro.TextMeshProUGUI _bonusCountText;
        private TMPro.TextMeshProUGUI _averageBoostText;
        private int _totalBonusesCollected;
        private int _totalBoostSum;

        private void OnEnable()
        {
            _deviceUIManager.SetActiveConfig += DeviceUIManagerOnSetActiveConfig;
        }

        private void OnDisable()
        {
            _deviceUIManager.SetActiveConfig -= DeviceUIManagerOnSetActiveConfig;
        }
        
        public void AddBonus(int bonusValue)
        {
            _totalBonusesCollected++;
            _totalBoostSum += bonusValue;
            
            if (_timer != null)
                _timer.AddTime();
            
            UpdateUI();
        }

        public int GetCountBonuses()
            => _totalBoostSum;
        
        public float GetAverageBoost()
            => _totalBonusesCollected > 0 ? (float)_totalBoostSum / _totalBonusesCollected : 0f;
        
        private void DeviceUIManagerOnSetActiveConfig()
        {
            SetReferenceUI();
        }
        
        private void SetReferenceUI()
        {
            UIConfigurationStats activeUIConfig = _deviceUIManager.GetActiveUIConfiguration();
            
            if (activeUIConfig == null)
            {
                Debug.LogError("Active UI Configuration is not set in DeviceUIManager!");
                return;
            }
            
            _timer = activeUIConfig.Timer;
            _bonusCountText = activeUIConfig.BonusCountText;
            _averageBoostText = activeUIConfig.AverageBoostText;

            UpdateUI();
        }

        private void UpdateUI()
        {
            _bonusCountText.text = $"Bonuses: {_totalBonusesCollected}";
            _averageBoostText.text = $"Avg Boost: {GetAverageBoost():0.00}";
        }
    }
}