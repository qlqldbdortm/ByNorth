using ByNorth.ActionSystem;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace ByNorth.Unit.Behaviour.Movable.State.Boss {
    public class ShotPatternState : IState<BossHandler> {
        private const float AFTER_DELAY = 0.5f;


        private BossHandler handler;
        private CancellationTokenSource token = null;

        private int count = 1;
        private float rotate = 0;
        private AnimationType type = AnimationType.None;

        public ShotPatternState(int count = 1, float rotate = 0, AnimationType type = AnimationType.None)
        {
            this.count = count;
            this.rotate = rotate;
            this.type = type;
        }


        public void Init(BossHandler handler)
        {
            this.handler = handler;
        }

        public void OnEnter()
        {
            token = new();
            _ = ShotAsync();
        }

        public void OnExit()
        {
            token.Cancel();
        }


        // ReSharper disable Unity.PerformanceAnalysis
        private async UniTask ShotAsync()
        {
            handler.Animator.SetInteger("State", (int)type);
            await UniTask.WaitForSeconds(0.2f);
            float perRotate = 360f / count;
            for (int i = 0; i < count; i++)
            {
                ActionData actionData = BossDataManager.Instance.shotData;
                ActionExecutor executor = ActionExecutor.Spawn(actionData, actionData.strengthMultiplier, handler.Unit);
                executor.transform.rotation = Quaternion.Euler(0, rotate + perRotate * i, 0);
            }
            handler.AudioSource.PlayOneShot(BossDataManager.Instance.shotSound);
            await UniTask.WaitForSeconds(AFTER_DELAY, cancellationToken: token.Token);
            handler.ChangeState();
        }
    }
}