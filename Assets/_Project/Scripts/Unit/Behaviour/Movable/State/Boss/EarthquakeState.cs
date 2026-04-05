using System.Threading;
using ByNorth.ActionSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ByNorth.Unit.Behaviour.Movable.State.Boss
{
    /// <summary>
    /// 범위 공격을 넣는 상태
    /// </summary>
    public class EarthquakeState: IState<BossHandler>
    {
        private const float CYCLE_TIME = 0.5f;
        
        
        private BossHandler handler;
        private CancellationTokenSource token = null;

        private int count = 1;
        
        
        public EarthquakeState(int count = 1) => this.count = count;
        
        
        public void Init(BossHandler handler)
        {
            this.handler = handler;
        }

        public void OnEnter()
        {
            token = new();
            _ = ChargeAsync();
        }

        public void OnExit()
        {
            token.Cancel();
        }


        // ReSharper disable Unity.PerformanceAnalysis
        private async UniTask ChargeAsync()
        {
            for (int i = 0; i < count; i++)
            {
                handler.Animator.SetInteger("State",(int)AnimationType.OverheadHeavySlash);
                await UniTask.WaitForSeconds(CYCLE_TIME);
                handler.AudioSource.PlayOneShot(BossDataManager.Instance.earthquakeSound);
                ActionData actionData = BossDataManager.Instance.earthquakeData;
                ActionExecutor.Spawn(actionData, actionData.strengthMultiplier, handler.Unit);
                await UniTask.WaitForSeconds(CYCLE_TIME, cancellationToken: token.Token);
            }
            handler.ChangeState();
        }
    }
}