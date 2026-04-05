using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace ByNorth.Unit.Behaviour.Movable.State.NonPlayer
{
    public class RunawayState: IState<NonPlayerHandler>
    {
        private const float MORALE_REGEN_DELAY = 0.5f;
        
        
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
            _ = RegenMoraleAsync();
        }
        public void OnExit()
        {
            token.Cancel();
        }


        private async UniTask RegenMoraleAsync()
        {
            while (true)
            {
                await UniTask.WaitForSeconds(MORALE_REGEN_DELAY, cancellationToken: token.Token);

                if (handler.Morale > 0)
                {
                    handler.ChangeState(StateType.Chase);
                    return;
                }
                else if (!handler.weaponData) // TODO: 단순 무기가 없을 때 도주판정은 생각해봐야 할 것 같음
                {
                    handler.ChangeState(StateType.Evacuate);
                    return;
                }

                Vector3 runToPlayer = handler.transform.position - PlayerHandler.Player.transform.position;
                runToPlayer = runToPlayer.normalized * 10;
                if(NavMesh.SamplePosition(runToPlayer, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                {
                    // TODO: 플레이어에게서 약간 도망침, 테스트 필요
                    handler.Agent.SetDestination(hit.position);
                }
            }
        }
    }
}