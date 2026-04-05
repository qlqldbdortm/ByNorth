using ByNorth.Unit.DieEvent;
using Cysharp.Threading.Tasks;
using Lean.Pool;
using UnityEngine;

namespace ByNorth.Effect {
    public class EffectHandler : MonoBehaviour {
        public Vector3 offset;
        public float despawnTime = 0.9f;


        private Transform target;


        void Update()
        {
            if (target is null) return;

            transform.position = target.position + offset;
        }

        private async UniTask DespawnTimerAsync()
        {
            await UniTask.WaitForSeconds(despawnTime);

            LeanPool.Despawn(this);
        }


        public static void Spawn(EffectHandler prefab, Transform target)
        {
            if (target.GetComponent<BreakOnDie>()) return;
            EffectHandler effect = EffectManager.Instance.GetEffect(prefab);
            effect.target = target;
            _ = effect.DespawnTimerAsync();
        }

        public static void VFXSpawn(EffectHandler prefab, Transform target)
        {
            if (target.GetComponent<BreakOnDie>()) return;
            EffectHandler effect = EffectManager.Instance.GetVFXEffect(prefab);
            effect.target = target;
            _ = effect.DespawnTimerAsync();
        }
    }
}
