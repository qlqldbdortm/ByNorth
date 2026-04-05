using System;
using UnityEngine;

namespace ByNorth.Unit.Behaviour.Movable.Phase
{
    [Serializable]
    public class PhaseData
    {
        [Tooltip("Phase에서 유닛 데이터")] public UnitData phaseUnitData;

        [Tooltip("Phase 정보")] public PhaseBase phase;
    }
}