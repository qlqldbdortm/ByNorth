using System.Threading;
using ByNorth.Core.VillageSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ByNorth.Unit.Behaviour.Movable.State.NonPlayer
{
    public class PatrolState: IState<NonPlayerHandler>
    {
        private const float LOCATION_CHANGE_DELAY = 3f;
        private const float SIGHT_CHECK_DELAY = 0.5f;
        
        
        private NonPlayerHandler handler = null;
        private CancellationTokenSource token = null;
        private CancellationTokenSource sightBrokenToken = null;

        private Transform destination = null;
        
        
        public void Init(NonPlayerHandler handler)
        {
            this.handler = handler;
        }

        public void OnEnter()
        {
            handler.Agent.speed = handler.Unit.CurrentData.moveSpeed;
            NextDestination();
            token = new();
            sightBrokenToken = new();
            _ = CheckPatrolAsync();
            _ = CheckPlayerInSight();
        }
        public void OnExit()
        {
            token.Cancel();
            sightBrokenToken.Cancel();
        }


        private void NextDestination()
        {
            destination = handler.patrolData.NextDestination();
            handler.Agent.SetDestination(destination.position);
        }

        private async UniTask CheckPatrolAsync()
        {
            while (true)
            {
                await UniTask.WaitForSeconds(SIGHT_CHECK_DELAY, cancellationToken: token.Token);

                if (handler.CheckPlayerInSight())
                {
                    handler.ChangeState(StateType.Chase);
                }

                // 목표 인근 도착했을 시 다음 목적지로 이동
                float distance = Vector3.Distance(destination.position, handler.transform.position);
                if (distance < 0.5f)
                {
                    await UniTask.WaitForSeconds(LOCATION_CHANGE_DELAY, cancellationToken: token.Token);
                    
                    NextDestination();
                }
            }
        }

        private async UniTask CheckPlayerInSight()
        {
            while (true)
            {
                for(int i = 0;i < VillageManager.BrokenObjects.Count;i++)
                {
                    var obj = VillageManager.BrokenObjects[i];
                    if (obj.IsBroken)
                    {
                        float distance = Vector3.Distance(handler.transform.position, obj.transform.position);
                        if (distance < handler.nonPlayerData.sightRange)
                        {
                            VillageManager.BrokenObjects.RemoveAt(i);
                            handler.patrolData.patrolPoints.Insert(0, obj.transform);
                            handler.ChangeState(StateType.Track);
                            return;
                        }
                    }

                    await UniTask.Yield(cancellationToken: sightBrokenToken.Token);
                }
                await UniTask.Yield(cancellationToken: sightBrokenToken.Token);
            }
        }
    }
}