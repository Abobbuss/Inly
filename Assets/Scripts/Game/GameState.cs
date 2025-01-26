using UnityEngine;

namespace Game
{
    public class GameState : MonoBehaviour
    {
        private int _currentTime;
        private int _totalBonuses;
        private float _averageBoost;

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

        public void SetBonuses(int bonuses, float average)
        {
            _totalBonuses = bonuses;
            _averageBoost = average;
        }
    }
}