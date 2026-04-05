using ByNorth.Unit.Modifier;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace ByNorth.Unit.Behaviour.Movable.State.NonPlayer {
    public class StunState : IState<NonPlayerHandler> {
        private const float STUN_CHECK_DELAY = 0.5f;


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
                await UniTask.WaitForSeconds(STUN_CHECK_DELAY, cancellationToken: token.Token);
                if (!handler.Unit.Condition.HasFlag(ConditionType.Stun))
                {
                    handler.ChangeState(StateType.Idle);
                    return;
                }
            }
        }
    }
}