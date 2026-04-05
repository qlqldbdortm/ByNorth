using ByNorth.SlotSystem;
using ByNorth.SlotSystem.Data;
using System.Collections.Generic;
using ByNorth.LifeCycle;
using UnityEngine;

namespace ByNorth.Unit.Behaviour.Structure {
    public class BuildingHandler : StructureHandler, IStoragable, IRelease<Unit> {
        [Header("오브젝트가 가지고 있는 아이템 데이터")]
        public List<SlotData> slotDatas;


        public Transform Transform => transform;
        public bool HasFull => slotDatas.Count >= 9;
        

        public IEnumerable<SlotData> OpenStorage()
        {
            hasInteracting = hasInteracted = true;
            
            return slotDatas;
        }
        public void CloseStorage(IEnumerable<SlotData> slotData)
        {
            hasInteracting = false;
            
            this.slotDatas.Clear();
            this.slotDatas.AddRange(slotData);
        }

        public void OnRelease(Unit unit)
        {
            StoragableManager.Instance.storagables.Add(this);
        }

        public void AddItem(SlotData item)
        {
            slotDatas.Add(item);
        }
    }
}
