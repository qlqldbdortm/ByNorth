using UnityEngine;

namespace ByNorth.Unit.Modifier.StatsModifier
{
    public abstract class StatsModifier: ModifierBase
    {
        [Header("스탯조정 정보")]
        [Tooltip("최대 체력")] public float hp;
        [Tooltip("최대 스태미나")] public float stamina;
        [Tooltip("힘")] public float strength = 0;
        [Tooltip("이동속도")] public float moveSpeed = 0;
    }
}