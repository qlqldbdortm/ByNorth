namespace ByNorth.Unit.Behaviour.Movable.State.NonPlayer
{
    public class ScareState: IState<NonPlayerHandler>
    {
        private NonPlayerHandler handler = null;
        
        
        public void Init(NonPlayerHandler handler)
        {
            this.handler = handler;
        }

        public void OnEnter()
        {
            // TODO: 겁먹은 Animation 동작만
        }
        public void OnExit()
        {
            
        }
    }
}