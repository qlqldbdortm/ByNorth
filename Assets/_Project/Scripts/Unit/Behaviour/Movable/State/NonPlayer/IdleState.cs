using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ByNorth.Unit.Behaviour.Movable.State.NonPlayer
{
    public class IdleState: IState<NonPlayerHandler>
    {
        private const float SIGHT_CHECK_DELAY = 0.5f;
        
        
        private NonPlayerHandler handler = null;
        private CancellationTokenSource token = null;
        
        
        public void Init(NonPlayerHandler handler)
        {
            this.handler = handler;
        }

        public void OnEnter()
        {
            // TODO: 일상적인 Animation 시작
            token = new();
            _ = CheckSightAsync();
        }
        public void OnExit()
        {
            token.Cancel();
        }


        private async UniTask CheckSightAsync()
        {
            while (true)
            {
                await UniTask.WaitForSeconds(SIGHT_CHECK_DELAY, cancellationToken: token.Token);

                // 감지범위 내에 플레이어가 존재하는지 확인
                if (handler.CheckPlayerInSight())
                {
                    if (handler.Morale > 0)
                    {
                        handler.ChangeState(StateType.Chase);
                    }
                    else
                    {
                        handler.ChangeState(StateType.Runaway);
                    }

                    return;
                }

                if (handler.patrolData.HasPatrolDestination)
                {
                    handler.ChangeState(StateType.Patrol);
                    return;
                }
            }
        }
    }
}