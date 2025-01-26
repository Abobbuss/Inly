using System;
using UnityEngine;

namespace Game
{
    public class DeviceUIManager : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject pcUI;
        [SerializeField] private GameObject mobileUI;
        
        [Header("UI Configurations")]
        [SerializeField] private UIConfigurationStats pcUIConfig;
        [SerializeField] private MobileUIConfigurationStats mobilePortraitUIConfig;
        [SerializeField] private MobileUIConfigurationStats mobileLandscapeUIConfig;
        
        [Header("Debug Settings")] 
        [SerializeField] private bool forceMobileUIInEditor;

        private UIConfigurationStats _activeUIConfig;
        private ScreenOrientation _currentOrientation;
        
        public event Action OnOrientationChanged;
        
        public bool IsMobileUIActive { get; private set; }

        private void Awake()
        {
            if (IsMobilePlatform())
            {
                IsMobileUIActive = true;
                pcUI.SetActive(false);
                mobileUI.SetActive(true);
                SetActiveUI(Screen.width > Screen.height ? mobileLandscapeUIConfig : mobilePortraitUIConfig);
            }
            else
            {
                IsMobileUIActive = false;
                mobileUI.SetActive(false);
                pcUI.SetActive(true);
                SetActiveUI(pcUIConfig);
            }
        }
        
        private void Update()
        {
            if (!IsMobilePlatform())
                return;
            
            if (_currentOrientation == Screen.orientation) 
                return;
            
            _currentOrientation = Screen.orientation;
            UpdateActiveUI();
            OnOrientationChanged?.Invoke();
        }
        
        public UIConfigurationStats GetActiveUIConfiguration()
            => _activeUIConfig;
        
        private bool IsMobilePlatform()
            => Application.isMobilePlatform || forceMobileUIInEditor;
        
        private void UpdateActiveUI()
        {
            SetActiveUI(Screen.width > Screen.height ? mobileLandscapeUIConfig : mobilePortraitUIConfig);
        }
        
        private void SetActiveUI(UIConfigurationStats config)
        {
            pcUIConfig.UIRoot.gameObject.SetActive(false);
            mobilePortraitUIConfig.UIRoot.gameObject.SetActive(false);
            mobileLandscapeUIConfig.UIRoot.gameObject.SetActive(false);
            
            config.UIRoot.gameObject.SetActive(true);
            _activeUIConfig = config;
        }
    }
}
