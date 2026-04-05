using ByNorth.LifeCycle;
using ByNorth.Unit.Behaviour.Movable;
using ByNorth.Unit.Modifier;
using Cysharp.Threading.Tasks;
using Lean.Pool;
using UnityEngine;
using UnityEngine.VFX;

namespace ByNorth.Unit.DieEvent
{
    public class DespawnOnDie: MonoBehaviour, IRelease<Unit>
    {
        [SerializeField] private float despawnDelay = 5;
        public BloodEffectData bloodEffect;
        public AudioClip dieSound;
        public void OnRelease(Unit unit)
        {
            _ = DespawnAsync(unit);
            bloodEffect.VFXEffectSpawn(unit);
            PlayerHandler.Player.AudioSource.PlayOneShot(dieSound);
        }

        private async UniTask DespawnAsync(Unit unit)
        {
            await UniTask.WaitForSeconds(despawnDelay);
            
            LeanPool.Despawn(unit);
        }
    }
}