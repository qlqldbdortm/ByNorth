namespace ByNorth.Unit.Modifier.Event
{
    public interface IModifierEvent
    {
        public void OnAdd(ModifierBase modifier);
        public void OnRemove(ModifierBase modifier);
    }
}