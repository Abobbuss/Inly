using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.UI
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _currentHighScoreText;
        [SerializeField] private TextMeshProUGUI _currentScoreText;
        [SerializeField] private Button _restartButton;

        private void Start()
        {
            _restartButton.onClick.AddListener(RestartGame);
        }

        private void OnDestroy()
        {
            _restartButton.onClick.RemoveListener(RestartGame);
        }

        public void SetHighScore(int highScore)
        {
            _currentHighScoreText.text = $"High Score: {highScore}";
        }
        
        public void SetCurrentScore(int currentScore)
        {
            _currentScoreText.text = $"Score: {currentScore}";
        }
        
        private void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}