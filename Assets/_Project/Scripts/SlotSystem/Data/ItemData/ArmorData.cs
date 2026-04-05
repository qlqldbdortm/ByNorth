using UnityEngine;

namespace ByNorth.SlotSystem.Data.ItemData {

    [CreateAssetMenu(fileName = "New ArmorData", menuName = "Scriptable Object/Item/ArmorData")]
    public class ArmorData : ItemData {
        [Header("내구도")]
        public int durability;
        [Header("피해감소"), Range(0, 1)]
        public float damageReduction;
    }
}
