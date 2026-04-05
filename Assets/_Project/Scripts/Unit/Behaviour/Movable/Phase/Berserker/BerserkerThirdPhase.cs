using System.Collections.Generic;
using ByNorth.Unit.Behaviour.Movable.State;
using ByNorth.Unit.Behaviour.Movable.State.Boss;

namespace ByNorth.Unit.Behaviour.Movable.Phase.Berserker
{
    public class BerserkerThirdPhase: PhaseBase
    {
        public override List<IState<BossHandler>> GetPattern() => new()
        {
            new SuperJumpState(3),
            new EarthquakeState(5),
            new WaitState(5),
        };
    }
}