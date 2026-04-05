using System;

namespace ByNorth.Unit.Modifier
{
    [Flags]
    public enum ConditionType
    {
        None        = 0,
        Stun        = 1 << 0,
        Slow        = 1 << 1,
    }
}