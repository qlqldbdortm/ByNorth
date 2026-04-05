using ByNorth.ActionSystem;
using ByNorth.SlotSystem.Data.ItemData;
using ByNorth.UI;
using TMPro;
using UnityEngine;

namespace ByNorth.Unit {
    [RequireComponent(typeof(Unit))]
    public class SpawnedUnit : MonoBehaviour {
        public UnitData data;
        private Unit unit;

        void Start()
        {
            unit = GetComponent<Unit>();
            unit.ApplyData(data);
        }
    }
}