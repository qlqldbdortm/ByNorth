using System.Collections.Generic;
using ByNorth.Unit.Behaviour.Movable.State;
using ByNorth.Unit.Behaviour.Movable.State.Boss;

namespace ByNorth.Unit.Behaviour.Movable.Phase.Berserker
{
    public class BerserkerSecondPhase: PhaseBase
    {
        public override List<IState<BossHandler>> GetPattern() => new()
        {
            new ChargeState(),
            new WaitState(1),
            new ShotPatternState(16, 0, AnimationType.LeftOneHandSlash),
            new ShotPatternState(16, 11.25f, AnimationType.RightOneHandSlash),
            new WaitState(1),
        };
    }
}