using System.Threading;
using ByNorth.ActionSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ByNorth.Unit.Behaviour.Movable.State.Boss
{
    public class CreateUnitState: IState<BossHandler>
    {
        private const float CYCLE_TIME = 1;
        
        
        private BossHandler handler;
        private CancellationTokenSource token = null;
        
        
        public void Init(BossHandler handler)
        {
            this.handler = handler;
        }

        public void OnEnter()
        {
            token = new();
            _ = CreateAsync();
        }

        public void OnExit()
        {
            token.Cancel();
        }


        // ReSharper disable Unity.PerformanceAnalysis
        private async UniTask CreateAsync()
        {
            await UniTask.WaitForSeconds(CYCLE_TIME, cancellationToken: token.Token);
            
            for (int i = 0; i < BossDataManager.Instance.createCount; i++)
            {
                Unit unit = Unit.Spawn(BossDataManager.Instance.createUnitData);
                Vector2 pos = Random.insideUnitCircle * BossDataManager.Instance.createUnitRange;
                unit.transform.position = new Vector3(pos.x, 0, pos.y);
            }
            
            handler.ChangeState();
        }
    }
}