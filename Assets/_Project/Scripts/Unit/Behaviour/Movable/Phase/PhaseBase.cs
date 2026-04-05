using System.Collections.Generic;
using ByNorth.Unit.Behaviour.Movable.State;
using UnityEngine;

namespace ByNorth.Unit.Behaviour.Movable.Phase
{
    public abstract class PhaseBase:MonoBehaviour
    {
        public abstract List<IState<BossHandler>> GetPattern();
    }
}