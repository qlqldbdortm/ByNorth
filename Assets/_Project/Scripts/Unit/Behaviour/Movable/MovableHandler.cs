using ByNorth.ActionSystem;
using ByNorth.SlotSystem.Data.ItemData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ByNorth.Unit.Behaviour.Movable {
    [RequireComponent(typeof(Unit))]
    public abstract class MovableHandler : MonoBehaviour {
        private static readonly int State = Animator.StringToHash("State");


        [Header("오디오 설정")]
        public AudioClip leftFootClip;
        public AudioClip rightFootClip;


        public AudioSource AudioSource { get; private set; } = null;
        public Animator Animator { get; private set; } = null;
        public Unit Unit { get; private set; } = null;
        protected AttackData CurrentAttackData { private get; set; } = null;

        protected abstract WeaponData WeaponData { get; }
        public bool HasCooled => cooledTime > Time.time;


        private float footstepCooldown = 0.2f; // 최소 간격 (초)
        private float lastFootstepTime = -1f;

        private float cooledTime = 0f;


        protected virtual void Awake()
        {
            Animator = GetComponent<Animator>();
            Unit = GetComponent<Unit>();
            AudioSource = GetComponent<AudioSource>();
        }

        private bool CanPlayFootstep()
        {
            return Time.time - lastFootstepTime >= footstepCooldown;
        }

        public void PlayAttackSound()
        {
            AudioSource.PlayOneShot(CurrentAttackData.attackAudio);
        }
        public void PlayLeftFoot()
        {
            if (!CanPlayFootstep()) return;

            lastFootstepTime = Time.time;
            AudioSource.PlayOneShot(leftFootClip);
        }
        public void PlayRightFoot()
        {
            if (!CanPlayFootstep()) return;

            lastFootstepTime = Time.time;
            AudioSource.PlayOneShot(rightFootClip);
        }

        public void OnAttack()
        {
            if (CurrentAttackData is null) return;
            ActionData actionData = CurrentAttackData.actionData;
            float damage = CurrentAttackData.damage;
            ActionExecutor.Spawn(actionData, damage, Unit);

            CurrentAttackData = null;
        }

        protected void ActionAnimation(AnimationType animationType) => _ = ActionAnimationAsync(animationType);
        private async UniTask ActionAnimationAsync(AnimationType animationType)
        {
            float startTime = Time.time;
            cooledTime = startTime + 0.5f;
            Animator.SetInteger(State, (int)animationType);
            Animator.SetFloat("XDir", 0);
            Animator.SetFloat("YDir", 0);

            await UniTask.WaitForSeconds(0.5f);

            cooledTime = startTime + Animator.GetCurrentAnimatorStateInfo(0).length;
            await UniTask.WaitForSeconds(0.1f);
            Animator.SetInteger(State, (int)AnimationType.None);
        }

        protected void Die()
        {
            Vector3 toTarget = (PlayerHandler.Player.Unit.transform.position - transform.position).normalized;
            float dot = Vector3.Dot(transform.forward, toTarget);
            if (dot > 0)
            {
                Animator.SetTrigger("FrontDie");
            }
            else
            {
                Animator.SetTrigger("BackDie");
            }
        }
    }
}
