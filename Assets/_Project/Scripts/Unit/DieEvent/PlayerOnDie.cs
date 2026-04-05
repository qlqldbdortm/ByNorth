using System.Collections;
using System.Collections.Generic;
using ByNorth.LifeCycle;
using ByNorth.Unit.Behaviour.Movable;
using ByNorth.Unit.Modifier;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ByNorth.Unit.DieEvent
{
    public class PlayerOnDie : MonoBehaviour,IRelease<Unit>
    {
        [Header("플레이어 사망 후 다음씬으로 넘어가기까지의 걸리는 시간")]
        public float sceneLoadDelay = 3f;
        
        public BloodEffectData bloodEffect;
        public void OnRelease(Unit unit)
        {
            _ = PlayerDie();
            PlayerHandler.Player.Animator.SetTrigger("FrontDie");
            bloodEffect.VFXEffectSpawn(unit);
            
        }

        private async UniTask PlayerDie()
        {
            await UniTask.WaitForSeconds(sceneLoadDelay);
            SceneManager.LoadScene("End");
        }
    }
}
