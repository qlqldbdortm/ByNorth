using UnityEngine.UI;

namespace ByNorth.UI
{
    public abstract class SelectableLine<T>: Selectable where T: Selectable
    {
        public abstract void OnClick();
        public abstract void OnFocus();
        public abstract void OnUnfocus();
        public abstract void Focus();
        public abstract void Unfocus();

        public T GetUp() => this.FindSelectableOnUp()?.GetComponent<T>();
        public T GetDown() => this.FindSelectableOnDown()?.GetComponent<T>();
        //public SelectableLine GetLeft() => this.FindSelectableOnLeft()?.GetComponent<SelectableLine>();
        //public SelectableLine GetRight() => this.FindSelectableOnRight()?.GetComponent<SelectableLine>();
    }
}