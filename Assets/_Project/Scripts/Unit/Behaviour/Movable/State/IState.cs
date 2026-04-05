namespace ByNorth.Unit.Behaviour.Movable.State
{
    public interface IState<T> where T: MovableHandler
    {
        public void Init(T handler);

        public void OnEnter();
        public void OnExit();
    }
}