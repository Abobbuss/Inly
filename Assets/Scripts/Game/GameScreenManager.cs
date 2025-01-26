using System;
using Game.Data;
using Game.UI;
using Player;
using Timers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    public class GameScreenManager : MonoBehaviour
    {
        [Header("Screen Prefabs")]
        [SerializeField] private UIConfigurationGameScene _gameplayScreenPrefab;
        [SerializeField] private GameObject _pauseScreenPrefab;
        [SerializeField] private GameOverScreen _gameOverScreenPrefab;

        [SerializeField] private DeviceUIManager _deviceUIManager;
        [SerializeField] private PlayerStats _playerStats;
        [SerializeField] private GameState _gameState;
        
        private GameObject _currentScreen;
        private Timer _currentTimer;
        private GameData _gameData;
        private SaveManager _saveManager;
        
        public bool CanUsePause => _currentScreen != null && !_currentScreen.TryGetComponent<GameOverScreen>(out _);

        public event Action<UIConfigurationStats, MobileUIConfigurationStats> SetGameScreen;

        private void Awake()
        {
            _saveManager = new SaveManager();
            _gameData = _saveManager.Load();
            
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
            
            _currentTimer.TimeIsOut += TimerOnTimeISOut;
        }

        private void TimerOnTimeISOut()
        {
            ShowGameOverScreen();
            _currentTimer.TimeIsOut -= TimerOnTimeISOut;
        }

        public void ShowGameplayScreen()
        {
            SwitchScreen(_gameplayScreenPrefab.gameObject);
            
            UIConfigurationGameScene currentGameScreen = _currentScreen.GetComponent<UIConfigurationGameScene>();
            SetGameScreen?.Invoke(currentGameScreen.PCConfig, currentGameScreen.MobileConfig);
        }

        public void ShowPauseScreen()
        {
            SwitchScreen(_pauseScreenPrefab);
        }

        public void ShowGameOverScreen()
        {
            if (_gameState.GetTotalTime() > _gameData.HighScore)
            {
                _gameData.HighScore = _gameState.GetTotalTime();
                _saveManager.Save(_gameData);
            }
            
            _gameOverScreenPrefab.SetHighScore(_gameData.HighScore);
            _gameOverScreenPrefab.SetCurrentScore(_gameState.GetTotalTime());
            
            SwitchScreen(_gameOverScreenPrefab.gameObject);
        }

        private void SwitchScreen(GameObject screenPrefab)
        {
            bool isWasGameScreen = _currentScreen != null && _currentScreen.TryGetComponent<UIConfigurationGameScene>(out _);
            
            if (isWasGameScreen)
                SaveCurrentState();
            
            if (_currentScreen != null)
                Destroy(_currentScreen);
            
            if (screenPrefab != null)
                _currentScreen = Instantiate(screenPrefab, transform);
        }
        
        private void SaveCurrentState()
        {
            UIConfigurationStats uiConfig = _deviceUIManager.GetActiveUIConfiguration();
            _gameState.SetTime(uiConfig.Timer.GetCurrentTime());
            _gameState.SetBonuses(_playerStats.GetCountBonuses(), _playerStats.GetAverageBoost());
        }
    }
}