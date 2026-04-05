namespace ByNorth.Unit.Modifier
{
    public interface ICondition
    {
        public ConditionType Condition { get; }
        public float Influence { get; }

        public void Modify(Unit unit);
        public void Undo(Unit unit);
    }
}