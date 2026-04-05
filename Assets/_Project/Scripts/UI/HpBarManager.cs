using ByNorth.Core;
using Lean.Pool;
using UnityEngine;
using UnityEngine.Serialization;

namespace ByNorth.UI
{
    public class HpBarManager: Singleton<HpBarManager>
    {
        [SerializeField] public HpBar prefab;
        [SerializeField] public Transform objectGroup;
        
        
        public HpBar GetHpBar() => LeanPool.Spawn(prefab, objectGroup);
    }
}