using System.Threading;
using Cysharp.Threading.Tasks;

namespace ByNorth.Unit.Behaviour.Movable.State.Boss
{
    /// <summary>
    /// 대기 상태
    /// </summary>
    public class WaitState : IState<BossHandler>
    {
        private BossHandler handler;
        private CancellationTokenSource token = null;

        private float waitTime = 1;


        public WaitState(float waitTime = 1) => this.waitTime = waitTime;


        public void Init(BossHandler handler)
        {
            this.handler = handler;
        }

        public void OnEnter()
        {
            token = new();
            _ = WaitAsync();
        }

        public void OnExit()
        {
            token.Cancel();
        }


        private async UniTask WaitAsync()
        {
            await UniTask.WaitForSeconds(waitTime, cancellationToken: token.Token);

            handler.ChangeState();
        }
    }
}