using System;
using Game;
using UnityEngine;
using TMPro;

namespace Timers
{
    public class Timer : MonoBehaviour
    {
        private GameState _gameState;
        
        [Header("Timer Settings")]
        [SerializeField] private int _startTimeValue = 30;
        [SerializeField] private int _valueToAddTime = 5;

        [Header("UI Settings")]
        [SerializeField] private TextMeshProUGUI _timerText;
        
        private int _currentTime;
        private float _internalTimer;
        private bool _isRunning;

        public event Action TimeIsOut;

        private void Start()
        {
            StartTimer();
            UpdateTimerUI();
        }

        private void Update()
        {
            if (!_isRunning || _currentTime <= 0) 
                return;
            
            _internalTimer += Time.deltaTime;
            
            if (_internalTimer >= 1f)
            {
                _gameState.SetTotalTime((int)_internalTimer);
                
                _currentTime--;
                _internalTimer = 0f;
                
                UpdateTimerUI();
            }

            if (_currentTime <= 0)
                OnGameOver();
        }

        public void Initialize(GameState gameState)
        {
            _gameState = gameState;
            
            if (_gameState.GetCurrentTime() != 0)
                _currentTime = _gameState.GetCurrentTime();
            
            UpdateTimerUI();
        }

        public int GetCurrentTime()
            => _currentTime;

        private void StartTimer()
        {
            if (_currentTime == 0)
                _currentTime = _startTimeValue;
            
            _isRunning = true;
        }

        public void AddTime()
        {
            _currentTime += _valueToAddTime;
            UpdateTimerUI();
        }

        private void OnGameOver()
        {
            _isRunning = false;
            TimeIsOut?.Invoke();
        }

        private void UpdateTimerUI()
        {
            _timerText.text = $"Time: {_currentTime}";
        }
    }
}