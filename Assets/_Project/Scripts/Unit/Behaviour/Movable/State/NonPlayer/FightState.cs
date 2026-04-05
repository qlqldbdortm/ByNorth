using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ByNorth.Unit.Behaviour.Movable.State.NonPlayer
{
    // TODO: 코드 수정 필요함
    public class FightState: IState<NonPlayerHandler>
    {
        private const float FIGHT_CYCLE_DELAY = 1f;
        
        
        private NonPlayerHandler handler = null;
        private CancellationTokenSource token = null;
        
        
        public void Init(NonPlayerHandler handler)
        {
            this.handler = handler;
        }

        public void OnEnter()
        {
            handler.Agent.ResetPath();
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
                float distance = Vector3.Distance(handler.transform.position, PlayerHandler.Player.transform.position);
                if (distance > handler.FightRange)
                {
                    handler.ChangeState(StateType.Chase);
                    return;
                }

                
                handler.transform.LookAt(PlayerHandler.Player.transform, Vector3.up);
                handler.DoAttack();
                
                await UniTask.WaitForSeconds(FIGHT_CYCLE_DELAY, cancellationToken: token.Token);
            }
        }
    }
}