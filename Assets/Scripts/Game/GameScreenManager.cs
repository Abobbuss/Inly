using System;
using Player;
using Timers;
using UnityEngine;

namespace Game
{
    public class GameScreenManager : MonoBehaviour
    {
        [Header("Screen Prefabs")]
        [SerializeField] private UIConfigurationGameScene gameplayScreenPrefab;
        [SerializeField] private GameObject pauseScreenPrefab;
        [SerializeField] private GameObject gameOverScreenPrefab;

        [SerializeField] private DeviceUIManager _deviceUIManager;
        [SerializeField] private PlayerStats _playerStats;
        [SerializeField] private GameState _gameState;
        
        private GameObject _currentScreen;
        private Timer _currentTimer;

        public event Action<UIConfigurationStats, MobileUIConfigurationStats> SetGameScreen;

        private void Awake()
        {
            ShowGameplayScreen();
            
            UIConfigurationGameScene currentGameScreen = _currentScreen.GetComponent<UIConfigurationGameScene>();
            _deviceUIManager.SetUIConfiguration(currentGameScreen.PCConfig, currentGameScreen.MobileConfig);
        }

        private void OnEnable()
        {
            _deviceUIManager.SetActiveConfig += DeviceUIManagerOnSetActiveConfig;
        }

        private void OnDisable()
        {
            _deviceUIManager.SetActiveConfig -= DeviceUIManagerOnSetActiveConfig;
        }

        private void DeviceUIManagerOnSetActiveConfig()
        {
            UIConfigurationStats activeUIConfiguration = _deviceUIManager.GetActiveUIConfiguration();
            Timer timer = activeUIConfiguration.Timer;
            _currentTimer = timer;
            
            _currentTimer.TimeISOut += TimerOnTimeISOut;
        }

        private void TimerOnTimeISOut()
        {
            ShowGameOverScreen();
            _currentTimer.TimeISOut -= TimerOnTimeISOut;
        }

        public void ShowGameplayScreen()
        {
            SwitchScreen(gameplayScreenPrefab.gameObject);
            
            UIConfigurationGameScene currentGameScreen = _currentScreen.GetComponent<UIConfigurationGameScene>();
            SetGameScreen?.Invoke(currentGameScreen.PCConfig, currentGameScreen.MobileConfig);
        }

        public void ShowPauseScreen()
        {
            SwitchScreen(pauseScreenPrefab);
        }

        public void ShowGameOverScreen()
        {
            SwitchScreen(gameOverScreenPrefab);
        }

        private void SwitchScreen(GameObject screenPrefab)
        {
            bool isWasGameScreen = _currentScreen != null && _currentScreen.TryGetComponent<UIConfigurationGameScene>(out _);
            bool isWillGameScreen = screenPrefab.TryGetComponent<UIConfigurationGameScene>(out _);
            
            if (isWasGameScreen)
                SaveCurrentState();
            
            if (_currentScreen != null)
                Destroy(_currentScreen);
            
            if (screenPrefab != null)
                _currentScreen = Instantiate(screenPrefab, transform);
            
            if (isWillGameScreen)
                RestoreState(screenPrefab);
        }
        
        private void SaveCurrentState()
        {
            UIConfigurationStats uiConfig = _deviceUIManager.GetActiveUIConfiguration();
            _gameState.SetTime(uiConfig.Timer.GetCurrentTime());
            _gameState.SetBonuses(_playerStats.GetCountBonuses(), _playerStats.GetAverageBoost());
        }

        private void RestoreState(GameObject screen)
        {
            
        }
    }
}