using System.Collections.Generic;
using ByNorth.SlotSystem.Data;
using UnityEngine;

namespace ByNorth.Unit.Behaviour.Structure
{
    public interface IStoragable
    {
        public Transform Transform { get; }
        public bool HasFull { get; }
        
        
        public IEnumerable<SlotData> OpenStorage();
        public void CloseStorage(IEnumerable<SlotData> slotData);
        public void AddItem(SlotData item);
    }
}