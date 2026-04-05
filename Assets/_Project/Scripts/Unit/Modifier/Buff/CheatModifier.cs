using System.Collections.Generic;
using System.Threading;
using ByNorth.Effect;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ByNorth.Unit.Modifier.Buff
{
    [CreateAssetMenu(fileName = "CheatModifier", menuName = "Modifier/StatsModifier/CheatModifier")]
    public class CheatModifier: ModifierBase
    {
        private static Dictionary<Unit, CancellationTokenSource> Tokens { get; } = new();
        
        
        public EffectHandler effect;
        public float Influence => time;
        public override void Modify(Unit unit)
        {
            EffectHandler.Spawn(effect, unit.transform);
            var token = new CancellationTokenSource();
            Tokens.Add(unit, token);
            _ = InfinityAsync(unit, token);

            unit.CurrentData.strength += 50;
        }

        public override void Undo(Unit unit)
        {
            if (Tokens.TryGetValue(unit, out CancellationTokenSource token))
            {
                token.Cancel();
            }
            unit.CurrentData.strength -= 50;
        }


        private async UniTask InfinityAsync(Unit target, CancellationTokenSource token)
        {
            while (true)
            {
                await UniTask.Yield(cancellationToken: token.Token);
                
                target.TakeHeal(target.CurrentData.hp, true);
                target.RecoverStamina(target.CurrentData.stamina);
            }
        }
    }
}