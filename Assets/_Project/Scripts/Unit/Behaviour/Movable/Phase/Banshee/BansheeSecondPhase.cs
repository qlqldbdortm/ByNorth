using System.Collections.Generic;
using ByNorth.Unit.Behaviour.Movable.State;
using ByNorth.Unit.Behaviour.Movable.State.Boss;

namespace ByNorth.Unit.Behaviour.Movable.Phase.Banshee
{
    public class BansheeSecondPhase: PhaseBase
    {
        public override List<IState<BossHandler>> GetPattern() => new()
        {
            new ChargeState()
        };
    }
}