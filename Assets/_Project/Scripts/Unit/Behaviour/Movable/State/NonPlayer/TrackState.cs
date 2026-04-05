using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ByNorth.Unit.Behaviour.Movable.State.NonPlayer
{
    // TODO: 코드 수정 필요함
    public class TrackState: IState<NonPlayerHandler>
    {
        private const float SIGHT_CHECK_DELAY = 0.5f;
        
        
        private NonPlayerHandler handler = null;
        private CancellationTokenSource token = null;
        
        private Transform destination = null;
        
        
        public void Init(NonPlayerHandler handler)
        {
            this.handler = handler;
        }

        public void OnEnter()
        {
            handler.Agent.speed = handler.Unit.CurrentData.moveSpeed;
            destination = handler.patrolData.patrolPoints[0];
            handler.patrolData.patrolPoints.RemoveAt(0);
            handler.Agent.SetDestination(destination.position);
            
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

                if (handler.CheckPlayerInSight())
                {
                    handler.ChangeState(StateType.Chase);
                    return;
                }

                float distance = Vector3.Distance(destination.position, handler.transform.position);
                if (distance < 1f)
                {
                    if (handler.CheckPlayerInSight(2))
                    {
                        handler.ChangeState(StateType.Chase);
                    }
                    else
                    {
                        handler.ChangeState(StateType.Patrol);
                    }
                    return;
                }
            }
        }
    }
}