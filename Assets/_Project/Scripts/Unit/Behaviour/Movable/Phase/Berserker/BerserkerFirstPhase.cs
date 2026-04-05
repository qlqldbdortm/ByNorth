using System.Collections.Generic;
using ByNorth.Unit.Behaviour.Movable.State;
using ByNorth.Unit.Behaviour.Movable.State.Boss;

namespace ByNorth.Unit.Behaviour.Movable.Phase.Berserker
{
    public class BerserkerFirstPhase: PhaseBase
    {
        public override List<IState<BossHandler>> GetPattern() => new()
        {
            new ChargeState(),
            new WaitState(2),
            new EarthquakeState(5),
            new WaitState(2),
        };
    }
}