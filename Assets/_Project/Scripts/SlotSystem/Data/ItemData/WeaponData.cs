using UnityEngine;
namespace ByNorth.SlotSystem.Data.ItemData {
    [CreateAssetMenu(fileName = "New WeaponData", menuName = "Scriptable Object/Item/WeaponData")]
    public class WeaponData : ItemData {
        public WeaponType weaponType;
        public AttackData normalAttackData;
        public AttackData hardAttackData;
        public float range = 2f;

        public SkillData.SkillData skillData;

        protected override void Reset()
        {
            itemType = ItemType.Weapon;
            weaponType = WeaponType.None;
            icon = null;
            itemName = "새 무기";
            description = "새 무기 아이템의 정보를 입력하세요.";
            skillData = null;
        }
    }
}