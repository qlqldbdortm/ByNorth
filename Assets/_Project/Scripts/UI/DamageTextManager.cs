using System.Collections;
using ByNorth.Core;
using ByNorth.Effect;
using ByNorth.Unit;
using Lean.Pool;
using UnityEngine;

namespace ByNorth.UI
{
    public class DamageTextManager: Singleton<DamageTextManager>
    {
        [SerializeField] public DamageText textPrefab;
        [SerializeField] public Transform textGroup;


        // ReSharper disable Unity.PerformanceAnalysis
        public DamageText GetDamageText() => LeanPool.Spawn(textPrefab, textGroup);
    }
}