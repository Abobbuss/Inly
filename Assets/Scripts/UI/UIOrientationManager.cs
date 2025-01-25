using UnityEngine;

namespace UI
{
    public class UIOrientationManager : MonoBehaviour
    {
        [Header("References")]
        public RectTransform portraitLayout; 
        public RectTransform landscapeLayout;

        private void Update()
        {
            ActivateLayout(Screen.width > Screen.height ? landscapeLayout : portraitLayout);
        }

        private void ActivateLayout(RectTransform activeLayout)
        {
            portraitLayout.gameObject.SetActive(activeLayout == portraitLayout);
            landscapeLayout.gameObject.SetActive(activeLayout == landscapeLayout);
        }
    }
}
