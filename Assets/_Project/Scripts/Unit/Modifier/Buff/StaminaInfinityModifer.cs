using System.Collections.Generic;
using System.Threading;
using ByNorth.Effect;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ByNorth.Unit.Modifier.Buff
{
    [CreateAssetMenu(fileName = "StaminaInfinity", menuName = "Modifier/StatsModifier/StaminaInfinity")]
    public class StaminaInfinityModifer: ModifierBase
    {
        private static Dictionary<Unit, CancellationTokenSource> Tokens { get; } = new();
        
        
        public EffectHandler effect;


        public float Influence => time;


        public override void Modify(Unit unit)
        {
            EffectHandler.Spawn(effect, unit.transform);
            var token = new CancellationTokenSource();
            Tokens.Add(unit, token);
            _ = StaminaInfinityAsync(unit, token);
        }

        public override void Undo(Unit unit)
        {
            if (Tokens.TryGetValue(unit, out CancellationTokenSource token))
            {
                token.Cancel();
            }
        }


        private async UniTask StaminaInfinityAsync(Unit target, CancellationTokenSource token)
        {
            while (true)
            {
                await UniTask.Yield(cancellationToken: token.Token);
                
                target.RecoverStamina(target.CurrentData.stamina);
            }
        }
    }
}