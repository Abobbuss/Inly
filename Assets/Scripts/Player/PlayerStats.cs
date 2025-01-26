using System;
using Game;
using Timers;
using UnityEngine;

namespace Player
{
    public class PlayerStats : MonoBehaviour
    {
        [SerializeField] private DeviceUIManager _deviceUIManager;
        
        private Timer _timer;
        private TMPro.TextMeshProUGUI _bonusCountText;
        private TMPro.TextMeshProUGUI _averageBoostText;
        private int totalBonusesCollected;
        private int totalBoostSum;

        private void OnEnable()
        {
            _deviceUIManager.OnOrientationChanged += DeviceUIManagerOnOnOrientationChanged;
        }

        private void OnDisable()
        {
            _deviceUIManager.OnOrientationChanged -= DeviceUIManagerOnOnOrientationChanged;
        }

        private void Start()
        {
            SetReferenceUI();
        }
        
        private void DeviceUIManagerOnOnOrientationChanged()
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

        public void AddBonus(int bonusValue)
        {
            totalBonusesCollected++;
            totalBoostSum += bonusValue;
            
            if (_timer != null)
                _timer.AddTime();
            
            UpdateUI();
        }

        private float GetAverageBoost()
            => totalBonusesCollected > 0 ? (float)totalBoostSum / totalBonusesCollected : 0f;

        private void UpdateUI()
        {
            _bonusCountText.text = $"Bonuses: {totalBonusesCollected}";
            _averageBoostText.text = $"Avg Boost: {GetAverageBoost():0.00}";
        }
    }
}