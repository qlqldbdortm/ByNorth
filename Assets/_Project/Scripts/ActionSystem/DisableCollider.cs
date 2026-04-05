using ByNorth.ActionSystem;
using ByNorth.LifeCycle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ByNorth {
    public class DisableCollider : MonoBehaviour, ISpawn<ActionExecutor> {
        public float disableTime = 0.2f;

        private Collider Collider
        {
            get
            {
                if (collide is null)
                {
                    collide = GetComponent<Collider>();
                }

                return collide;
            }
        }
        private Collider collide = null;
        
        
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        public void OnSpawn(ActionExecutor data)
        {
            _ = DisableColl();
        }
        
        private async UniTask DisableColl()
        {
            Collider.enabled = true;
            await UniTask.WaitForSeconds(disableTime);
            Collider.enabled = false;
        }
    }
}
