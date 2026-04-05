using ByNorth.ActionSystem;
using ByNorth.Core.GameFlow;
using ByNorth.InputHandler;
using ByNorth.LifeCycle;
using ByNorth.SlotSystem;
using ByNorth.SlotSystem.Data.ItemData;
using ByNorth.Unit.Modifier;
using ByNorth.Unit.Modifier.Event;
using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ByNorth.Unit.Behaviour.Movable
{
    public class PlayerHandler : MovableHandler, IModifierEvent, IRelease<Unit>
    {
        private static readonly int XDir = Animator.StringToHash("XDir");
        private static readonly int YDir = Animator.StringToHash("YDir");
        private static readonly int Speed = Animator.StringToHash("Speed");
        private const float STAMINA_RECOVER_DELAY = 2;
        
        
        [SerializeField] public Transform[] rayPoints; // TODO: 테스트를 위해서 존재함

        [Header("입력 부드럽게 처리")]
        [SerializeField] private float inputSmoothTime = 0.1f;

        [Header("카메라 설정")]
        public CinemachineVirtualCamera virtualCamera;


        public static PlayerHandler Player { get; private set; }
        
        
        public CharacterController Controller { get; private set; } = null;
        public Vector2 LookDirection { private get; set; } = Vector2.zero;
        public Vector2 MoveDirection { private get; set; } = Vector2.zero;
        public float SprintRate { private get; set; } = 0;

        private bool IsSprinting => SprintRate > 0.1f;  // 달리기 판정 (임계값 조정 가능)
        protected override WeaponData WeaponData => SlotManager.Instance.equipmentGroup.slotsList[0].slotData as WeaponData;


        private float rawSpeed;  // Speed 값 저장용

        // 입력 부드럽게 처리
        private Vector3 smoothedInput;
        private Vector3 inputVelocity;
        
        // 스태미너 관련 필드
        private float staminaUseTime = -STAMINA_RECOVER_DELAY;

        private float gravityVelocity = 0;


        protected override void Awake()
        {
            base.Awake();
            Controller = GetComponent<CharacterController>();
            Player = this;
        }

        void Update()
        {
            Gravity();

            if (Unit.Condition.HasFlag(ConditionType.Stun)) return;
            if (HasCooled) return;

            Move();
            Look();
            
        }
        

        #region ◇ 기본 동작 처리(중력, Move, Look) ◇
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

        private void Move()
        {
            Vector3 moveDirection = new Vector3(MoveDirection.x, 0, MoveDirection.y);

            // 이동값 부드럽게 적용
            smoothedInput = Vector3.SmoothDamp(smoothedInput, moveDirection, ref inputVelocity, inputSmoothTime);

            // 스태미너를 사용하지 못하는 상태면 달리기가 안 되게 함
            if (!Unit.CanUseStamina)
            {
                SprintRate = 0;
            }
            // 스프린트 비율 0~1로 제한
            float sprintValue = Mathf.Clamp01(SprintRate);

            // 속도 계산
            rawSpeed = smoothedInput.magnitude * (1f + sprintValue);

            float moveSpeed = Unit.CurrentData.moveSpeed;
            float sprintSpeed = Unit.CurrentData.sprintSpeedMultiply;

            // transform.TransformDirection 제거 -> 월드 기준 이동 방향
            Vector3 moveDir = smoothedInput *
                          ((smoothedInput.magnitude * moveSpeed) + (sprintSpeed - 1) * moveSpeed * sprintValue);

            SprintStamina(moveDir.magnitude);
            
            // 이동 적용
            Controller.Move(moveDir * Time.deltaTime);

            Vector3 localMoveDir = transform.InverseTransformDirection(smoothedInput);

            Animator.SetFloat(XDir, localMoveDir.x);
            Animator.SetFloat(YDir, localMoveDir.z);
            Animator.SetFloat(Speed, rawSpeed);
        }
        private void Look()
        {
            Vector3 lookDir = new Vector3(LookDirection.x, 0, LookDirection.y);
            if (LookDirection.magnitude < 0.05f)
            {
                lookDir.x = MoveDirection.x;
                lookDir.z = MoveDirection.y;
            }
            
            if (lookDir.magnitude > 0.1f)
            {
                transform.rotation = Quaternion.LookRotation(lookDir);
            }
        }
        #endregion
        
        #region ◇ Modifer 추가/제거 Event ◇
        public void OnAdd(ModifierBase modifier)
        {
            ModifierIconManager.Instance.AddModifierIcon(modifier);
        }
        public void OnRemove(ModifierBase modifier)
        {
            ModifierIconManager.Instance.RemoveModifierIcon(modifier);
        }
        #endregion
        
        public void ControlBehaviour(ControlType controlType)
        {
            if (HasCooled) return;

            AttackData attackData = null;
            switch (controlType)
            {
                case ControlType.NormalAttack:
                    attackData = WeaponData?.normalAttackData;
                    break;
                case ControlType.HardAttack:
                    attackData = WeaponData?.hardAttackData;
                    break;
                case ControlType.Skill: 
                    attackData = WeaponData?.skillData.primaryAttackData;
                    break;
            }

            if (attackData is null && controlType is ControlType.HardAttack or ControlType.NormalAttack)
            {
                attackData = ActionExecutorManager.Instance?.bareHandsAttackData;
            }

            UseAttackData(attackData);
        }

        public void UseAttackData(AttackData attackData)
        {
            if (attackData == null) return;
            if (attackData.stamina > Unit.CurrentStamina) return;
                
            CurrentAttackData = attackData;
            ConsumeStamina(attackData.stamina);
            ActionAnimation(attackData.animationType);
        }

        public void OnRelease(Unit data)
        {
            SetMove(Vector2.zero);
            SetLook(Vector2.zero);
            InputManager.ChangeActionMap(ActionMapType.Option);
            Animator.SetFloat("XDir", 0);
            Animator.SetFloat("YDir", 0);
        }

        private void SprintStamina(float nowSpeed)  // 스테미나
        {
            float sprintMaxSpeed = Unit.CurrentData.moveSpeed * (Unit.CurrentData.sprintSpeedMultiply - 1);
            float consumeRate = (nowSpeed - Unit.CurrentData.moveSpeed) / sprintMaxSpeed;

            if (consumeRate > 0.1f)
            {
                ConsumeStamina(consumeRate * Time.deltaTime);
            }
            else if(staminaUseTime + STAMINA_RECOVER_DELAY < Time.time)
            {
                Unit.RecoverStamina(Unit.CurrentData.stamina * 0.1f * Time.deltaTime);
            }
        }

        private void ConsumeStamina(float stamina)
        {
            staminaUseTime = Time.time;
            Unit.ConsumeStamina(stamina);
        }

        public void SetMove(Vector2 moveInput)
        {
            MoveDirection = moveInput;
        }
        public void SetLook(Vector2 lookInput)
        {
            LookDirection = lookInput;
        }
        public void SetSprint(float sprint)
        {
            SprintRate = sprint;
        }
    }
}
