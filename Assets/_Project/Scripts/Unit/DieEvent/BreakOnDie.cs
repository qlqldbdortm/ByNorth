using ByNorth.LifeCycle;
using DG.Tweening;
using ByNorth.Core.VillageSystem;
using ByNorth.Unit.Behaviour.Movable;
using UnityEngine;
using UnityEngine.AI;

namespace ByNorth.Unit.DieEvent {
    // TODO: 리팩토링 해야 함
    public class BreakOnDie : MonoBehaviour, ISpawn<Unit>, IRelease<Unit> {
        [Header("자식 오브젝트")]
        public GameObject original;
        public GameObject broken;
        
        [Header("넘어지는 방향")]
        public bool front = false;

        public bool IsBroken { get; private set; } = false;

        public AudioClip brokenAudio;
        
        private NavMeshObstacle obstacle;
        private Collider colli;


        void Awake()
        {
            obstacle = GetComponent<NavMeshObstacle>();
            colli = GetComponent<Collider>();
        }

        public void OnSpawn(Unit unit)
        {
            IsBroken = false;
            
            colli.enabled = true;
            if (obstacle != null)
            {
                obstacle.enabled = true;
            }
            
            if (original != null && broken != null)
            {
                original.SetActive(true);
                broken.SetActive(false);
            }
        }
        public void OnRelease(Unit unit)
        {
            IsBroken = true;
            
            colli.enabled = false;
            if (obstacle != null)
            {
                obstacle.enabled = false;
            }
            VillageManager.BrokenObjects.Add(this);
            
            if (broken)
            {
                BrokenObject();
            }
            else
            {
                FallDown(front);
            }
            PlayerHandler.Player.AudioSource.PlayOneShot(brokenAudio);
        }

        private void BrokenObject()
        {
            Sequence seq = DOTween.Sequence();

            seq.Append(transform.DOShakeRotation(0.3f, 20f, 10, 90f, false));
            seq.Append(transform.DOMoveY(transform.position.y + 0.2f, 0.15f).SetEase(Ease.OutQuad));
            seq.Append(transform.DOMoveY(transform.position.y, 0.1f).SetEase(Ease.InQuad));

            seq.OnComplete(Break);
        }
        private void Break()
        {
            original.SetActive(false);
            broken.SetActive(true);
        }

        private void FallDown(bool frontBack)
        {
            int forward = front ? -1 : 1;

            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOShakeRotation(0.3f, new Vector3(0, 0, 10f), 8, 90f, false));
            Quaternion targetRot = Quaternion.AngleAxis(forward * 80f, transform.right) * transform.rotation;
            seq.Append(transform.DORotateQuaternion(targetRot, 0.8f).SetEase(Ease.InQuad));
            Quaternion reboundRot = Quaternion.AngleAxis(forward * 90f, transform.right) * transform.rotation;
            seq.Append(transform.DORotateQuaternion(reboundRot, 0.2f).SetEase(Ease.OutSine));
        }
    }
}
