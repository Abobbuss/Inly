using System;
using UnityEngine;

namespace Game
{
    public class DeviceUIManager : MonoBehaviour
    {
        [SerializeField] private GameScreenManager _gameScreenManager;
        [SerializeField] private GameState _gameState;
        
        [Header("Debug Settings")] 
        [SerializeField] private bool forceMobileUIInEditor;

        private GameObject _pcUI;
        private GameObject _mobileUI;
        private UIConfigurationStats _activeUIConfig;
        private UIConfigurationStats _pcUIConfig;
        private MobileUIConfigurationStats _mobileUIConfig;

        public event Action SetActiveConfig;
        
        public bool IsMobileUIActive { get; private set; }

        private void OnEnable()
        {
            _gameScreenManager.SetGameScreen += GameScreenManagerOnSetGameScreen;
        }

        private void OnDisable()
        {
            _gameScreenManager.SetGameScreen += GameScreenManagerOnSetGameScreen;
        }

        private void Start()
        {
            UpdateValues();
        }

        private void GameScreenManagerOnSetGameScreen(UIConfigurationStats pcConfig, MobileUIConfigurationStats mobileConfig)
        {
            SetUIConfiguration(pcConfig, mobileConfig);
            UpdateValues();
            
        }

        private void UpdateValues()
        {
            if (IsMobilePlatform())
            {
                IsMobileUIActive = true;
                _pcUI.SetActive(false);
                _mobileUI.SetActive(true);
                SetActiveUI(_mobileUIConfig);
            }
            else
            {
                IsMobileUIActive = false;
                _mobileUI.SetActive(false);
                _pcUI.SetActive(true);
                SetActiveUI(_pcUIConfig);
            }
        }

        public void SetUIConfiguration(UIConfigurationStats pcConfig, MobileUIConfigurationStats mobileConfig)
        {
            _pcUIConfig = pcConfig;
            _mobileUIConfig = mobileConfig;

            _pcUI = pcConfig.UIRoot;
            _mobileUI = mobileConfig.UIRoot;
        }
        
        public UIConfigurationStats GetActiveUIConfiguration()
            => _activeUIConfig;
        
        private bool IsMobilePlatform()
            => Application.isMobilePlatform || forceMobileUIInEditor;
        
        private void SetActiveUI(UIConfigurationStats config)
        {
            _pcUIConfig.UIRoot.gameObject.SetActive(false);
            _mobileUIConfig.UIRoot.gameObject.SetActive(false);
            
            config.UIRoot.gameObject.SetActive(true);
            config.Timer.Initialize(_gameState);
            _activeUIConfig = config;
            
            SetActiveConfig?.Invoke();
        }
    }
}
