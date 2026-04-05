using ByNorth.Core;
using ByNorth.Effect;
using ByNorth.SlotSystem.Data.ItemData;
using Lean.Pool;
using UnityEngine;

namespace ByNorth.ActionSystem {
    public class ActionExecutorManager : Singleton<ActionExecutorManager> {
        [SerializeField] public AttackData bareHandsAttackData;
        [SerializeField] public Transform objectGroup;


        // ReSharper disable Unity.PerformanceAnalysis
        public ActionExecutor GetActionExecutor(ActionExecutor prefab, Transform center) => LeanPool.Spawn(prefab, center.position, center.rotation, objectGroup);

    }
}