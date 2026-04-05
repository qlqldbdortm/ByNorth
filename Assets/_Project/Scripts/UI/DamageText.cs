using System;
using ByNorth.Effect;
using ByNorth.LifeCycle;
using ByNorth.Unit;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Lean.Pool;
using TMPro;
using UnityEngine;

namespace ByNorth.UI
{
    public class DamageText: MonoBehaviour 
    {
        private static Camera MainCamera
        {
            get
            {
                if (!mainCamera)
                {
                    mainCamera = Camera.main;
                }
                return mainCamera;
            }
        }
        private static Camera mainCamera = null;
        
        
        [SerializeField] private TextMeshProUGUI text;


        // ReSharper disable Unity.PerformanceAnalysis
        private async UniTask ReleaseAsync(float duration)
        {
            // TODO: 이펙트를 줘야 할 것 같음
            
            await UniTask.WaitForSeconds(duration);
            
            LeanPool.Despawn(this);
        }

        
        public static void Spawn(Unit.Unit unit, int damage, DamageType damageType)
        {
            DamageText damageText = DamageTextManager.Instance.GetDamageText();
            damageText.text.text = $"{damage}";

            damageText.transform.position = unit.transform.position;
            
            damageText.text.fontSize = 36;
            damageText.text.fontStyle = FontStyles.Bold;
            damageText.text.color = Color.white;
            damageText.text.outlineWidth = 0.3f;
            damageText.text.outlineColor = new Color32(32, 32, 32, 255);
            
            switch (damageType)
            {
                case DamageType.Critical:
                    damageText.text.fontSize = 60;
                    damageText.text.color = new Color32(192, 192, 32, 255);
                    damageText.text.outlineWidth = 0.5f;
                    damageText.text.outlineColor = new Color32(96, 96, 16, 255);
                    break;
                case DamageType.Miss:
                    damageText.text.text = "MISS";
                    damageText.text.fontStyle = FontStyles.Bold | FontStyles.Italic;
                    damageText.text.color = new Color32(128, 128, 192, 255);
                    damageText.text.outlineColor = new Color32(16, 16, 64, 255);
                    break;
                case DamageType.Normal:
                default:
                    break;
            }
            damageText.transform.DOScale(1.5f, 1.5f).SetEase(Ease.InOutCirc);
            _ = damageText.ReleaseAsync(2);
        }
    }
}