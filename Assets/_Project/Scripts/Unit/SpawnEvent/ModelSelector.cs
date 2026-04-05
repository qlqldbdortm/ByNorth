using ByNorth.LifeCycle;
using UnityEngine;

namespace ByNorth.Unit.SpawnEvent
{
    public class ModelSelector: MonoBehaviour, ISpawn<Unit>
    {
        [SerializeField] private GameObject[] models;
        
        
        public void OnSpawn(Unit data)
        {
            int sel = Random.Range(0, models.Length);
            for (int i = 0; i < models.Length; i++)
            {
                models[i].SetActive(i == sel);
            }
        }
    }
}