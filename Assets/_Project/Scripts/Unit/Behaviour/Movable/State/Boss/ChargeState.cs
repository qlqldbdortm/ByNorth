using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ByNorth.Unit.Behaviour.Movable.State.Boss
{
    /// <summary>
    /// 플레이어 위치까지 다가가는 상태
    /// </summary>
    public class ChargeState: IState<BossHandler>
    {
        private const float CHARGE_MAX_TIME = 60;
        
        
        private BossHandler handler;
        private CancellationTokenSource token = null;
        
        
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


        private async UniTask ChargeAsync()
        {
            float startTime = Time.time;
            
            while (true)
            {
                await UniTask.Yield(cancellationToken: token.Token);
                
                float distance = Vector3.Distance(handler.transform.position, PlayerHandler.Player.transform.position);

                if (distance < 2) // 플레이어 근처에 도착
                {
                    handler.ChangeState();
                    return;
                }

                if (Time.time > startTime + CHARGE_MAX_TIME) // 시간 제한 종료
                {
                    handler.ChangeState();
                    return;
                }

                float speed = handler.Unit.CurrentData.moveSpeed;
                
                handler.transform.LookAt(PlayerHandler.Player.transform, Vector3.up);
                handler.Controller.Move(speed * Time.deltaTime * handler.transform.forward);
            }
        }
    }
}