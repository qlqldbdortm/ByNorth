using ByNorth.Effect;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace ByNorth.Unit.Modifier {
    [CreateAssetMenu(fileName = "Dot", menuName = "Modifier/State/Dot Modifier")]
    public class DotModifier : ModifierBase {
        private const float DAMAGE_TICK = 1f; // TODO : 나중에 조정
        private static Dictionary<Unit, CancellationTokenSource> Tokens { get; } = new();

        [Header("게임상에 나타낼 효과 프리팹")]
        public EffectHandler effect;
        [Header("한 틱당 데미지")]
        public int damagePerTick = 1;

        public float Influence => damagePerTick;


        public override void Modify(Unit unit)
        {
            EffectHandler.Spawn(effect, unit.transform);
            var token = new CancellationTokenSource();
            Tokens.Add(unit, token);
            _ = DamagePerTickAsync(unit, token);
        }

        public override void Undo(Unit unit)
        {
            if (Tokens.TryGetValue(unit, out CancellationTokenSource token))
            {
                token.Cancel();
            }
        }


        private async UniTask DamagePerTickAsync(Unit target, CancellationTokenSource token)
        {
            while (true)
            {
                await UniTask.WaitForSeconds(DAMAGE_TICK, cancellationToken: token.Token);

                target.TakeDamage((int)damagePerTick);
            }
        }
    }
}