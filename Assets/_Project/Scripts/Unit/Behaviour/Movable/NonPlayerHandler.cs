using System;
using System.Collections;
using ByNorth.LifeCycle;
using ByNorth.SlotSystem.Data.ItemData;
using ByNorth.Unit.Behaviour.Movable.State;
using ByNorth.Unit.Behaviour.Movable.State.NonPlayer;
using EPOOutline;
using System.Collections.Generic;
using ByNorth.ActionSystem;
using ByNorth.Core.VillageSystem;
using ByNorth.Core.GameFlow;
using ByNorth.Unit.Modifier;
using ByNorth.Unit.Modifier.Event;
using UnityEngine;
using UnityEngine.AI;
using static ByNorth.Core.VillageSystem.VillageManager;

namespace ByNorth.Unit.Behaviour.Movable {
    [RequireComponent(typeof(NavMeshAgent))]
    public class NonPlayerHandler : MovableHandler, ISpawn<Unit>, IRelease<Unit>, IModifierEvent {
        public static LayerMask SightLayerMask => LayerMask.GetMask("Ground", "Unit", "Obstacle", "Building");


        [SerializeField] public UnitData unitData;
        [SerializeField] public WeaponData weaponData = null;
        [SerializeField] public NonPlayerData nonPlayerData;
        [SerializeField] public Transform sightPoint;
        [SerializeField] public PatrolData patrolData;

        [SerializeField] public NonPlayerType type = NonPlayerType.Common;


        public float FightRange => weaponData?.range ?? 1f;
        public float Morale { get; set; } = 0;
        public NavMeshAgent Agent { get; private set; } = null;
        protected override WeaponData WeaponData => weaponData;


        private StateType currentStateType = StateType.Idle;
        private IState<NonPlayerHandler> currentState = null;

        private Outlinable outline;

        private readonly Dictionary<StateType, IState<NonPlayerHandler>> states = new()
        {
            { StateType.Idle, new IdleState() },
            { StateType.Runaway, new RunawayState() },
            { StateType.Evacuate, new EvacuateState() },
            { StateType.Scare, new ScareState() },
            { StateType.Patrol, new PatrolState() },
            { StateType.Track, new TrackState() },
            { StateType.Chase, new ChaseState() },
            { StateType.Fight, new FightState() },
            { StateType.Stun, new StunState() },
        };


        protected override void Awake()
        {
            base.Awake();

            Agent = GetComponent<NavMeshAgent>();
            Morale = nonPlayerData.morale;
            outline = GetComponent<Outlinable>();

            foreach (var state in states.Values)
            {
                state.Init(this);
            }
        }
        IEnumerator Start()
        {
            yield return new WaitUntil(() => VillageManager.CurrentVillage);

            if (Unit.IsAlive) yield break;
            Unit.ApplyData(unitData);
        }

        void OnDestroy()
        {
            currentState?.OnExit();
        }

        void Update()
        {
            float speed = Agent.velocity.magnitude;
            Animator.SetFloat("YDir", speed);
            Animator.SetFloat("Speed", speed);
        }


        public void OnSpawn(Unit unit)
        {
            ChangeState(StateType.Idle);
            VillageManager.CurrentVillage.VillageUnitSpawn(this);
        }
        public void OnRelease(Unit data)
        {
            currentState.OnExit();
            Agent.ResetPath();
            
            // 플레이어와 관련된 동작
            switch (type)
            {
                case NonPlayerType.Common:
                    PlayerInfo.Instance.AddNormalKarma(1);
                    break;
                case NonPlayerType.Child:
                    PlayerInfo.Instance.AddChildKarma(1);
                    PlayerInfo.Instance.AddNormalKarma(2);
                    break;
            }
            
            PlayerHandler.Player.Unit.CurrentData.exp += Unit.CurrentData.exp;
            
            VillageManager.CurrentVillage.VillageUnitDespawn(this);
            Die();
        }


        /// <summary>
        /// 플레이어가 감지범위 내에 존재하는지 확인
        /// </summary>
        /// <param name="rangeMultiplier">감지 범위 배율</param>
        /// <returns></returns>
        public bool CheckPlayerInSight(float rangeMultiplier = 1)
        {
            Vector3 origin = sightPoint?.position ?? transform.position;
            foreach (Transform point in PlayerHandler.Player.rayPoints)
            {
                Vector3 direction = point.position - origin;
                Ray ray = new Ray(origin, direction);
                if (Physics.Raycast(ray, out RaycastHit hit, nonPlayerData.sightRange * rangeMultiplier, NonPlayerHandler.SightLayerMask))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void ChangeState(StateType newStateType)
        {
            currentState?.OnExit();
            currentState = states[newStateType];
            ChangeOutlineColor(newStateType);
            currentState?.OnEnter();

            currentStateType = newStateType;
        }

        public void DoAttack()
        {
            if (HasCooled) return;

            AttackData attackData = WeaponData?.normalAttackData;

            if (attackData is null)
            {
                attackData = ActionExecutorManager.Instance?.bareHandsAttackData;
            }

            if (attackData is not null)
            {
                CurrentAttackData = attackData;
                Unit.ConsumeStamina(attackData.stamina);
                ActionAnimation(attackData.animationType);
            }
        }

        /// <summary>
        /// NPC의 StateType 따라 outline의 색상을 변경 
        /// <br>기본, 추적, 의심 => 기본상태 (노랑)</br>
        /// <br>도망, 공포, 추적, 싸움, 대피 => 전투상태 (빨강)</br>
        /// </summary>
        /// <param name="currentState">현재 상태</param>
        private void ChangeOutlineColor(StateType currentState)
        {
            switch (currentState)
            {
                case StateType.Idle:
                case StateType.Track:
                case StateType.Patrol:
                    outline.OutlineParameters.Color = Color.yellow;
                    break;
                case StateType.Scare:
                case StateType.Stun:
                case StateType.Chase:
                case StateType.Fight:
                case StateType.Runaway:
                case StateType.Evacuate:
                    outline.OutlineParameters.Color = Color.red;
                    break;
            }
        }

        public void OnAdd(ModifierBase modifier)
        {
            ProcessCondition();
        }
        public void OnRemove(ModifierBase modifier)
        {
            
        }

        private void ProcessCondition()
        {
            if (Unit.Condition.HasFlag(ConditionType.Stun))
            {
                if (currentStateType == StateType.Stun) return;
                
                ChangeState(StateType.Stun);
            }
        }
    }
}