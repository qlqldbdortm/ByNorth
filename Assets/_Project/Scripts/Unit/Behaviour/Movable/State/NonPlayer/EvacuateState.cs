using ByNorth.Extensions;
using ByNorth.Core.VillageSystem;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace ByNorth.Unit.Behaviour.Movable.State.NonPlayer {
    public class EvacuateState : IState<NonPlayerHandler> {
        private const float LOCATE_CHECK_DELAY = 0.5f;


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
            _ = CheckLocateAsync();
        }
        public void OnExit()
        {
            token.Cancel();
        }


        private async UniTask CheckLocateAsync()
        {
            Transform destination = null;
            float minDistance = float.MaxValue;
            foreach (Transform point in VillageManager.CurrentVillage.evacuatePoints)
            {
                float distance = Vector3.Distance(point.position, handler.transform.position);
                if (distance < minDistance)
                {
                    destination = point;
                    minDistance = distance;
                }
            }

            if (!destination)
            {
                handler.ChangeState(StateType.Scare);
                return;
            }
            handler.Agent.SetDestination(destination.position);
            while (true)
            {
                await UniTask.WaitForSeconds(LOCATE_CHECK_DELAY, cancellationToken: token.Token);


                float distance = handler.transform.position.DistanceXZ(destination.position);
                if (distance < 0.5f)
                {
                    handler.ChangeState(StateType.Scare);
                }
            }
        }
    }
}