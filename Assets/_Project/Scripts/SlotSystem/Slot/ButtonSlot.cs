using ByNorth.UI;

namespace ByNorth.SlotSystem.Slot {
    public class ButtonSlot : Slot {
        public override void OnSwap(Slot fromSlot)
        {
            // 버튼 네비게이션 용도로 만들어 둔 스크립트여서 기능은 추가 X
        }

        public override void Use()
        {
            TweenButton button = transform.GetComponentInChildren<TweenButton>();
            button.onClick.Invoke();
        }
    }
}
