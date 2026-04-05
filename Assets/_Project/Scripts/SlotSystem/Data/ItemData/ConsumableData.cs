using ByNorth.ActionSystem;
using ByNorth.Unit.Behaviour.Movable;
using ByNorth.Unit.Modifier;
using UnityEngine;

namespace ByNorth.SlotSystem.Data.ItemData {

    [CreateAssetMenu(fileName = "New ConsumableItem", menuName = "Scriptable Object/Item/ConsumableItem")]
    public class ConsumableData : ItemData, IUsable {
        [Header("포션 사용시 회복량")]
        public int amountOfRecovery;

        [Header("포션의 효과")]
        public ConsumeType consumeType;

        [Header("버프 지속 시간")]
        public float buffDuration; // TODO : 나중에 필요없어지면 삭제

        public bool IsUsable { get; set; } = true;

        [Header("회복아이템 사용 시 나오는 이펙트")]
        public ActionData actionData;

        [Header("버프효과")]
        public ModifierBase modifier;

        protected override void Reset()
        {
            itemType = ItemType.Consumable;
            icon = null;
            itemName = "새 소비아이템";
            description = "새 소비아이템의 설명을 입력하세요.";
            amountOfRecovery = 0;
            consumeType = ConsumeType.None;
        }

        public void Use()
        {
            //TODO : 아이템 소모시 체력 회복, 스태미나 회복 가능하게 변경 (Clear)
            if (IsUsable)
            {
                switch (consumeType)
                {
                    case ConsumeType.Hp:
                        PlayerHandler.Player.Unit.TakeHeal(amountOfRecovery);
                        Effect();
                        break;
                    case ConsumeType.Stamina:
                        PlayerHandler.Player.Unit.RecoverStamina(amountOfRecovery);
                        Effect();
                        break;
                    case ConsumeType.Modifier:
                        PlayerHandler.Player.Unit.AddModifier(modifier);
                        break;
                    case ConsumeType.None:
                        break;
                }
                SlotManager.Instance?.DeleteItem(this); // 아이템 사용
            }
        }
        public void Effect()
        {
            ActionExecutor.Spawn(actionData, 0, PlayerHandler.Player?.Unit);
        }
    }
}