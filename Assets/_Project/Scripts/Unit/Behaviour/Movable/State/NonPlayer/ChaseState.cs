using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ByNorth.Unit.Behaviour.Movable.State.NonPlayer
{
    public class ChaseState: IState<NonPlayerHandler>
    {
        private NonPlayerHandler handler = null;
        private CancellationTokenSource token = null;
        
        
        
        public void Init(NonPlayerHandler handler)
        {
            this.handler = handler;
        }

        public void OnEnter()
        {
            handler.Agent.speed = handler.Unit.CurrentData.moveSpeed * handler.Unit.CurrentData.sprintSpeedMultiply;
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
                await UniTask.Yield(cancellationToken: token.Token);

                if (handler.Morale <= 0)
                {
                    handler.ChangeState(StateType.Runaway);
                    return;
                }

                float distance = Vector3.Distance(handler.transform.position, PlayerHandler.Player.transform.position);
                if (distance < handler.FightRange + 0.5f)
                {
                    handler.ChangeState(StateType.Fight);
                    return;
                }
                
                handler.Agent.SetDestination(PlayerHandler.Player.transform.position);
            }
        }
    }
}