using ByNorth.Unit.Behaviour.Movable;
using UnityEngine;

namespace ByNorth.StateMachine {
    public class BossBehaviour : StateMachineBehaviour {
        public AnimationType animationType;
        public float duration = 0.5f;
        private float timer;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            timer = 0f;

            animator.SetFloat("XDir", 0);
            animator.SetFloat("YDir", 0);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            timer += Time.deltaTime;

            if (timer >= duration)
            {
                animator.SetInteger("State", (int)AnimationType.None);
            }
        }
    }
}
