using ByNorth.SlotSystem;
using ByNorth.SlotSystem.Data;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using ByNorth.Unit.Behaviour.Movable;
using UnityEngine;

namespace ByNorth.Unit.Behaviour.Structure {
    public class BoxHandler : StructureHandler, IStoragable {
        [Header("오브젝트가 가지고 있는 아이템 데이터")]
        public List<SlotData> slotDatas;
        [Header("상자 오브젝트 뚜껑")]
        public Transform lid;
        [Header("상자가 열리는 각도")]
        public float openAngle = -90f;
        [Header("상자가 열리는 시간")]
        public float openDuration = 1f;
        
        public AudioClip openSound;

        public Transform Transform => transform;
        public bool HasFull => slotDatas.Count >= 9;
        

        public IEnumerable<SlotData> OpenStorage()
        {
            hasInteracting = hasInteracted = true;
            lid.DOLocalRotate(new Vector3(openAngle, 0, 0), openDuration).SetEase(Ease.InOutQuad);
            PlayerHandler.Player.AudioSource.PlayOneShot(openSound);
            return slotDatas;
        }

        public void CloseStorage(IEnumerable<SlotData> slotData)
        {
            slotDatas.Clear();
            slotDatas.AddRange(slotData);
            
            hasInteracting = false;
            lid.DOLocalRotate(Vector3.zero, openDuration).SetEase(Ease.InOutQuad);
        }

        public void AddItem(SlotData item)
        {
            slotDatas.Add(item);
        }
    }
}
