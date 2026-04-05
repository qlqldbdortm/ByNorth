using System.Threading;
using ByNorth.ActionSystem;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace ByNorth.Unit.Behaviour.Movable.State.Boss
{
    public class SuperJumpState: IState<BossHandler>
    {
        private BossHandler handler;
        private CancellationTokenSource token = null;
        
        
        private float flyingTime = 3;
        
        
        public SuperJumpState(float flyingTime = 3) => this.flyingTime = flyingTime;
        
        
        public void Init(BossHandler handler)
        {
            this.handler = handler;
        }

        public void OnEnter()
        {
            token = new();
            _ = JumpAsync();
        }

        public void OnExit()
        {
            token.Cancel();
        }


        // ReSharper disable Unity.PerformanceAnalysis
        private async UniTask JumpAsync()
        {
            //CharacterController가 Enabled 되어있으면 포지션 변경이 안된다고 하여 일시적으로 false하게 추가
            CharacterController cc = handler.GetComponent<CharacterController>();
            if (cc != null)
            {
                cc.enabled = false;
            }
            
            // TODO: 날아가는 모션 & 이펙트 필요
            handler.Animator.SetInteger("State", (int)AnimationType.Jump);
            await UniTask.WaitForSeconds(0.2f);
            handler.Animator.SetInteger("State", (int)AnimationType.None);
            
            Vector3 playerPos = PlayerHandler.Player.transform.position;
            handler.transform.position += Vector3.up * 20;
            
            await UniTask.WaitForSeconds(flyingTime, cancellationToken: token.Token);

            // TODO: 착지 모션 & 전용 공격 필요, 현재는 지진공격을 그대로 사용함
            handler.Animator.SetInteger("State", (int)AnimationType.JumpAttack);
            handler.transform.position = playerPos;

            if (cc != null)
            {
                cc.enabled = true;
            }

            await UniTask.WaitForSeconds(0.3f);
            
            
            ActionData actionData = BossDataManager.Instance.earthquakeData;
            ActionExecutor.Spawn(actionData, actionData.strengthMultiplier, handler.Unit);
            
            await UniTask.WaitForSeconds(1.5f);
            handler.ChangeState();
        }


    }
}