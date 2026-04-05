using System.Threading;
using Cysharp.Threading.Tasks;
using Lean.Pool;
using UnityEngine;

namespace ByNorth.UI
{
    public class HpBar: MonoBehaviour
    {
        [Header("미리 HpBar를 주는 경우 세팅")]
        [SerializeField] public Unit.Unit traceUnit;
        
        [Header("UI")]
        [SerializeField] public TextSlider slider;


        private int CurrentHp => traceUnit?.CurrentHp ?? 0;

        
        private CancellationTokenSource token = null;

        private int latestHp = -1;
        private Transform tracePoint;
        private Camera mainCamera = null;


        void Start()
        {
            mainCamera = Camera.main;
        }

        void LateUpdate()
        {
            if (!traceUnit)
            {
                Release();
                return;
            }
            if (tracePoint)
            {
                transform.position = mainCamera.WorldToScreenPoint(tracePoint.position);
            }
        }


        public void OnSpawn(Unit.Unit target, Transform tracePoint)
        {
            this.traceUnit = target;
            this.tracePoint = tracePoint;
            slider.maxValue = traceUnit.CurrentData.hp;
            slider.SetValue(CurrentHp);
            latestHp = CurrentHp;
            
            print($":{target}, {tracePoint}");

            token = new();
            _ = CheckHpAsync();
        }
        // ReSharper disable Unity.PerformanceAnalysis
        public void Release()
        {
            token.Cancel();
            slider.value = 0;
            
            print($"HP Bar Release");
            
            LeanPool.Despawn(this);
        }
        
        private async UniTask CheckHpAsync()
        {
            while (true)
            {
                await UniTask.WaitUntil(IsNotEqualsHp, cancellationToken: token.Token);

                latestHp = CurrentHp;
                slider.SetValueWithAnimation(CurrentHp);//(CurrentHp, 0.5f);
            }
        }

        private bool IsNotEqualsHp() => CurrentHp != latestHp;


        public static HpBar Spawn(Transform target, Unit.Unit traceUnit)
        {
            HpBar hpBar = HpBarManager.Instance.GetHpBar();
            hpBar.OnSpawn(traceUnit, target);

            return hpBar;
        }
    }
}