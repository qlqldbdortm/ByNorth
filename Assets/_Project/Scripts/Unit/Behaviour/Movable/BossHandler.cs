using ByNorth.SlotSystem.Data.ItemData;
using UnityEngine;
using System.Collections.Generic;
using ByNorth.Core.GameFlow;
using ByNorth.LifeCycle;
using ByNorth.Unit.Behaviour.Movable.Phase;
using ByNorth.Unit.Behaviour.Movable.State;
using Cysharp.Threading.Tasks;
using Lean.Pool;

namespace ByNorth.Unit.Behaviour.Movable
{
    public class BossHandler: MovableHandler, IRelease<Unit>
    {
        public List<PhaseData> phases;

        public float strongPerNormalKarma = 0;
        public float strongPerChildKarma = 0;
        
        
        public CharacterController Controller { get; private set; } = null;
        public float moveStageDelay = 3f;
        protected override WeaponData WeaponData => null;

        private List<IState<BossHandler>> pattern = null;
        private IState<BossHandler> currentState = null;

        private Vector3 lastPos;
        private float gravityVelocity = 0;
        

        protected override void Awake()
        {
            base.Awake();
            
            Controller = GetComponent<CharacterController>();
        }

        void Start()
        {
            NextPhase(Unit);
        }

        void Update()
        {
            Gravity();
            Move();
        }
        

        private void Gravity()
        {
            // 중력 구현
            if (Controller is null) return;
            if (Controller.isGrounded)
            {
                gravityVelocity = 0;
            }
            else
            {
                gravityVelocity += Time.deltaTime * Physics.gravity.y;
            }
            Controller.Move(gravityVelocity * Time.deltaTime * Vector3.up);
        }
        
        public void OnRelease(Unit unit)
        {
            if (phases.Count > 0)
            {
                NextPhase(unit);
            }
            else
            {
                PlayerHandler.Player.Unit.CurrentData.exp += 2000;
                currentState.OnExit();
                Die();
                Destroy(gameObject,moveStageDelay);
                _ = NextStage();
            }
        }
        
        /// <summary>
        /// 보스가 죽고나서 다음 스테이지로 넘어가도록 하는 메서드
        /// </summary>
        private async UniTask NextStage()
        {
            await UniTask.WaitForSeconds(moveStageDelay);
            InGameFlowManager.Instance.CreateVillage();
        }
        
        public void Move()
        {
            Vector3 delta = transform.position - lastPos;
            delta.y = 0;

            Vector3 velocity = delta / Time.deltaTime;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            float xDir = Mathf.Clamp(localVelocity.x / Unit.CurrentData.moveSpeed, 0, 1f);
            float yDir = Mathf.Clamp(localVelocity.z / Unit.CurrentData.moveSpeed, 0, 1f);

            Animator.SetFloat("XDir", xDir);
            Animator.SetFloat("YDir", yDir);

            lastPos = transform.position;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void ChangeState()
        {
            currentState?.OnExit();
            currentState = pattern[0];
            pattern.Add(pattern[0]);
            pattern.RemoveAt(0);
            currentState.OnEnter();
        }

        private void NextPhase(Unit unit)
        {
            PhaseData phase = phases[0];
            phases.RemoveAt(0);
            unit.ApplyData(phase.phaseUnitData);
            
            int normalKarmaHp = Mathf.RoundToInt(unit.BaseData.hp * strongPerNormalKarma * PlayerInfo.Instance.NormalKarma);
            int normalKarmaStrength = Mathf.RoundToInt(unit.BaseData.strength * strongPerNormalKarma * PlayerInfo.Instance.NormalKarma);
            int childKarmaHp = Mathf.RoundToInt(unit.BaseData.hp * strongPerNormalKarma * PlayerInfo.Instance.NormalKarma);
            int childKarmaStrength = Mathf.RoundToInt(unit.BaseData.strength * strongPerNormalKarma * PlayerInfo.Instance.NormalKarma);
            unit.CurrentData.hp += normalKarmaHp + childKarmaHp;;
            unit.CurrentData.strength += normalKarmaStrength + childKarmaStrength;
            unit.TakeHeal(normalKarmaHp + childKarmaHp);

            pattern = phase.phase.GetPattern();
            foreach (var state in pattern)
            {
                state.Init(this);
            }
            ChangeState();
        }
    }
}