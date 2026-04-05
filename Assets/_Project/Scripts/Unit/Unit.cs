using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ByNorth.LifeCycle;
using ByNorth.UI;
using ByNorth.Unit.Behaviour.Movable;
using ByNorth.Unit.DamageEvent;
using ByNorth.Unit.Modifier;
using ByNorth.Unit.Modifier.Event;
using Cysharp.Threading.Tasks;
using Lean.Pool;
using UnityEngine;

namespace ByNorth.Unit
{
    public class Unit: MonoBehaviour
    {
        private static readonly IDamaged[] DefaultDamagedEvent =
        {
            new TextOnDamaged(),
        };
        
        
        public bool IsAlive { get; private set; } = false;
        public bool CanUseStamina { get; private set; } = true;
        
        /// <summary>
        /// 현재 체력
        /// </summary>
        public int CurrentHp { get; private set; }

        /// <summary>
        /// 현재 스태미나
        /// </summary>
        public float CurrentStamina { get; private set; }

        /// <summary>
        /// Runtime 상에서 수정되지 않아야 하는 Unit의 기초 Data
        /// </summary>
        public UnitData BaseData { get; private set; } = null;
        /// <summary>
        /// 현재 상태를 반영한 Unit Data
        /// </summary>
        public UnitData CurrentData { get; set; } = null;

        public ConditionType Condition { get; private set; } = ConditionType.None;

        /// <summary>
        /// 공격을 하는 주체가 플레이어, NPC 인지를 구별하는 Type
        /// </summary>
        public EntityType EntityType { get; private set; } = EntityType.None;

        private Action<Unit, int, DamageType> OnDamaged { get; set; } = null;
        /// <summary>
        /// Spawn Event<br/>
        /// Pool에서 Spawn되면 하는 작업
        /// </summary>44
        private Action<Unit> OnSpawn { get; set; } = null;
        /// <summary>
        /// Release Event<br/>
        /// Pool로 Despawn되기 전에 하는 작업
        /// </summary>
        private Action<Unit> OnRelease { get; set; } = null;
        /// <summary>
        /// Modifier Event<br/>
        /// Modifier가 Add될 때 하는 작업
        /// </summary>
        private Action<ModifierBase> OnAddModifier { get; set; } = null;
        /// <summary>
        /// Modifier Event<br/>
        /// Modifier가 Remove될 때 하는 작업
        /// </summary>
        private Action<ModifierBase> OnRemoveModifier { get; set; } = null;
        
        
        private readonly Dictionary<ModifierBase, float> modifiers = new ();
        private HpBar hpBar = null;
        
        
        void Awake()
        {
            ISpawn<Unit>[] spawns = GetComponentsInChildren<ISpawn<Unit>>();
            foreach (var spawn in spawns)
            {
                OnSpawn += spawn.OnSpawn;
            }
            
            IRelease<Unit>[] releases = GetComponentsInChildren<IRelease<Unit>>();
            foreach (var release in releases)
            {
                OnRelease += release.OnRelease;
            }
            
            IDamaged[] damageds = GetComponentsInChildren<IDamaged>();
            foreach (var damaged in damageds)
            {
                OnDamaged += damaged.OnDamaged;
            }
            foreach (var damaged in DefaultDamagedEvent)
            {
                OnDamaged += damaged.OnDamaged;
            }
            
            IModifierEvent[] modifierEvents = GetComponentsInChildren<IModifierEvent>();
            foreach (var modifierEvent in modifierEvents)
            {
                OnAddModifier += modifierEvent.OnAdd;
                OnRemoveModifier += modifierEvent.OnRemove;
            }
        }

        /// <summary>
        /// 상태이상 추가
        /// </summary>
        /// <param name="modifier"></param>
        public void AddModifier(ModifierBase modifier)
        {
            if (modifiers.ContainsKey(modifier))
            {
                modifiers[modifier] = Time.time + modifier.time;
                modifier.Modify(this);
                return;
            }
            modifier.Modify(this);
            modifiers.Add(modifier, Time.time + modifier.time);
            
            ProcessCondition();
            
            OnAddModifier?.Invoke(modifier);
        }
        /// <summary>
        /// 상태이상 제거
        /// </summary>
        /// <param name="modifier"></param>
        public void RemoveModifier(ModifierBase modifier)
        {
            if (!modifiers.ContainsKey(modifier))
            {
                print($"뭔가, 뭔가 오류가 존재함. {modifier}가 없는데 제거를 시도하고 있음.");
                return;
            }
            modifier.Undo(this);
            modifiers.Remove(modifier);
            
            ProcessCondition();
            
            OnRemoveModifier?.Invoke(modifier);
        }
        /// <summary>
        /// 스턴, 슬로우 등의 디버프를 처리하는 곳3
        /// </summary>
        private void ProcessCondition()
        {
            // TODO: Condition형 상태이상 제어 기능 구현해야 함
            ConditionType conditionType = ConditionType.None;
            
            foreach (var modifier in modifiers.Keys)
            {
                if (modifier is ICondition condition)
                {
                    conditionType |= condition.Condition;
                }
            }
            
            Condition = conditionType;
        }
        private async UniTask CheckModifierAsync()
        {
            Stack<ModifierBase> removeList = new();
            while (IsAlive)
            {
                foreach (var modifier in modifiers.Keys.Where(modifier => modifiers[modifier] < Time.time))
                {
                    removeList.Push(modifier);
                }

                while (removeList.Count > 0)
                {
                    RemoveModifier(removeList.Pop());
                }

                await UniTask.WaitForSeconds(0.5f);
            }
        }
        

        /// <summary>
        /// Unit의 hp를 소모<br/>
        /// 방어력 연산 등은 TakeDamage에서 하는 게 아니라, 공격 시점에서 해야 함
        /// </summary>
        /// <param name="damage">소모시킬 체력</param>
        public void TakeDamage(int damage, DamageType damageType = DamageType.Normal)
        {
            if (!IsAlive) return;

            if (!hpBar && EntityType != EntityType.Player)
            {
                hpBar = HpBar.Spawn(transform, this);
            }
            
            CurrentHp -= damage;

            OnDamaged?.Invoke(this, damage, damageType);
            
            if (CurrentHp <= 0)
            {
                Die();
            }
        }

        public void TakeHeal(int plusHp, bool ignoreEvent = false)
        {
            CurrentHp += plusHp;

            if(CurrentHp > CurrentData.hp)
            {
                CurrentHp = CurrentData.hp;
            }
        }


        public void ConsumeStamina(float amount)
        {
            CurrentStamina -= amount;

            if (CurrentStamina < 0f)
            {
                CanUseStamina = false;
                CurrentStamina = 0f;
            }
        }

        public void RecoverStamina(float amount)
        {
            CurrentStamina += amount;

            if (CurrentStamina > CurrentData.stamina * 0.5f)
            {
                CanUseStamina = true;
            }
            if (CurrentStamina > CurrentData.stamina)
            {
                CurrentStamina = CurrentData.stamina;
            }
        }

        public void ApplyData(UnitData data)
        {
            IsAlive = true;
            
            modifiers.Clear();

            hpBar = null;
            BaseData = data;
            CurrentData = data.Clone();
            Condition = ConditionType.None;
            EntityType = data.entityType;
            CurrentHp = CurrentData.hp;
            CurrentStamina = CurrentData.stamina;
            OnSpawn?.Invoke(this);

            _ = CheckModifierAsync();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void Die()
        {
            IsAlive = false;
            hpBar?.Release();
            
            OnRelease?.Invoke(this);
        }


        /// <summary>
        /// 유닛 생성은 모두 해당 Method로 해야 함
        /// </summary>
        /// <param name="data">생성할 Unit의 Data</param>
        /// <returns>생성된 Unit</returns>
        public static Unit Spawn(UnitData data)
        {
            if (!data)
            {
                Debug.LogError("UnitData가 없음.\nUnitData가 없으면, Unit 생성이 불가능 함.");
                return null;
            }
            
            Unit result = LeanPool.Spawn(data.prefab);
            result.ApplyData(data);

            return result;
        }
    }
}