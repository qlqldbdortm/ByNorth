using System;
using ByNorth.ActionSystem.Triggered;
using ByNorth.Effect;
using ByNorth.LifeCycle;
using Cysharp.Threading.Tasks;
using Lean.Pool;
using UnityEngine;

namespace ByNorth.ActionSystem
{
    public class ActionExecutor: MonoBehaviour
    {
        /// <summary>
        /// Object의 Data
        /// </summary>
        public ActionData ActionData { get; private set; } = null;
        /// <summary>
        /// 해당 Object를 생성한 주체
        /// </summary>
        public Unit.Unit Caster { get; private set; } = null;

        /// <summary>
        /// 데미지, 회복양 등 효과의 강도
        /// </summary>
        public float Influence { get; private set; } = 0;

        /// <summary>
        /// 해당 Object와 TriggerEnter시 하는 작업
        /// </summary>
        private Action<ActionExecutor, Unit.Unit, Unit.Unit> OnTrigger { get; set; } = null;
        /// <summary>
        /// Spawn Event<br/>
        /// Pool에서 Spawn되면 하는 작업
        /// </summary>
        private Action<ActionExecutor> OnSpawn { get; set; } = null;
        /// <summary>
        /// Release Event<br/>
        /// Pool로 Despawn되기 전에 하는 작업
        /// </summary>
        private Action<ActionExecutor> OnRelease { get; set; } = null;


        void Awake()
        {
            ISpawn<ActionExecutor>[] spawns = GetComponentsInChildren<ISpawn<ActionExecutor>>();
            foreach (var spawn in spawns)
            {
                OnSpawn += spawn.OnSpawn;
            }
            
            IRelease<ActionExecutor>[] releases = GetComponentsInChildren<IRelease<ActionExecutor>>();
            foreach (var release in releases)
            {
                OnRelease += release.OnRelease;
            }
            
            ITriggered[] triggered = GetComponentsInChildren<ITriggered>();
            foreach (var trigger in triggered)
            {
                OnTrigger += trigger.OnTrigger;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            Unit.Unit unit = other.GetComponent<Unit.Unit>();
            if (unit)
            {
                OnTrigger?.Invoke(this, Caster, unit);
            }
        }

        public void ApplyData(ActionData data, float influence, Unit.Unit unit)
        {
            Caster = unit;
            ActionData = data;
            //Influence = influence * (1 + data.strengthMultiplier); 
            Influence = influence * (1 + data.strengthMultiplier * unit.CurrentData.strength);
            OnSpawn?.Invoke(this);

            _ = LifetimeAsync();
        }

        private async UniTask LifetimeAsync()
        {
            await UniTask.WaitForSeconds(ActionData.lifetime);
            
            OnRelease?.Invoke(this);
            LeanPool.Despawn(this);
        }
        

        public static ActionExecutor Spawn(ActionData data, float influence, Unit.Unit unit)
        {
            if (!data?.prefab)
            {
                // TODO: ActionExcutor가 없음
                return null;
            }
            
            ActionExecutor executor = ActionExecutorManager.Instance.GetActionExecutor(data.prefab, unit.transform);
            executor.ApplyData(data, influence, unit);
            
            return executor;
        }
    }
}