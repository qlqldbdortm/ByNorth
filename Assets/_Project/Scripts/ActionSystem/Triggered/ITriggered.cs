namespace ByNorth.ActionSystem.Triggered
{
    public interface ITriggered
    {
        /// <summary>
        /// Object와 TriggerEnter를 했을 때의 Event
        /// </summary>
        /// <param name="executor">Object</param>
        /// <param name="caster">Object 생성 주체</param>
        /// <param name="hit">부딪힌 상대</param>
        public void OnTrigger(ActionExecutor executor, Unit.Unit caster, Unit.Unit hit);
    }
}