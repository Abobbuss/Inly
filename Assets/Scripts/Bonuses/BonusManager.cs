using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Bonuses
{
    public class BonusManager : MonoBehaviour
    {
        [Header("Bonus Settings")]
        [SerializeField] private Bonus bonusPrefab;
        [SerializeField] private int minBonuses = 5; 
        [SerializeField] private int maxBonuses = 10;
        
        [Header("Spawn Area Settings")]
        [SerializeField] private Renderer _spawnAreaRenderer;

        private List<Bonus> _spawnedBonuses = new ();

        private void Start()
        {
            SpawnBonuses();
        }

        private void SpawnBonuses()
        {
            Vector3 spawnAreaSize = _spawnAreaRenderer.bounds.size;
        
            int bonusCount = Random.Range(minBonuses, maxBonuses + 1);

            for (int i = 0; i < bonusCount; i++)
            {
                Vector3 spawnPosition = new Vector3(
                    Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
                    0f,
                    Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
                );
            
                spawnPosition += _spawnAreaRenderer.transform.position;
            
                Bonus bonus = Instantiate(bonusPrefab, spawnPosition, Quaternion.identity).GetComponent<Bonus>();

                var bonusRenderer = bonus.Renderer;
                
                if (bonusRenderer != null)
                {
                    spawnPosition.y = _spawnAreaRenderer.transform.position.y + bonusRenderer.bounds.extents.y;
                    bonus.transform.position = spawnPosition;
                }
                else
                {
                    Debug.LogError("Renderer is not found on the bonus prefab");
                }
                
                _spawnedBonuses.Add(bonus);
            }
        }
    }
}
