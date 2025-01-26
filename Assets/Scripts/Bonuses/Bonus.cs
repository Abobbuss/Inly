using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Bonuses
{
    [RequireComponent(typeof(Renderer))]
    public class Bonus : MonoBehaviour
    {
        private int _minBoostValue = 1;
        private int _maxBoostValue = 100;
        private int _boostValue;
        
        public Renderer Renderer { get; private set; }

        private void Awake()
        {
            Renderer = GetComponentInChildren<Renderer>();
        }

        private void Start()
        {
            _boostValue = Random.Range(_minBoostValue, _maxBoostValue + 1);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerStats playerStats))
                CollectBonus(playerStats);
        }

        private void CollectBonus(PlayerStats player)
        {
            player.AddBonus(_boostValue);
            
            Destroy(gameObject);
        }
    }
}
