using UnityEngine;

namespace Game
{
    public class GameState : MonoBehaviour
    {
        private int _currentTime;
        private int _totalBonuses;
        private float _averageBoost;
        private int _totalTime;

        public int GetCurrentTime() 
            => _currentTime;
        
        public int GetTotalBonuses()
            => _totalBonuses;
        
        public float GetAverageBoost() 
            => _averageBoost;
        
        public void SetTime(int time)
        {
            _currentTime = time;
        }
        
        public void SetTotalTime(int deltaTime)
        {
            _totalTime += deltaTime;
        }
        
        public int GetTotalTime()
            => _totalTime;

        public void SetBonuses(int bonuses, float average)
        {
            _totalBonuses = bonuses;
            _averageBoost = average;
        }
    }
}