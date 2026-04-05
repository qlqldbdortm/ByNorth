using ByNorth.Core;
using UnityEngine;

namespace ByNorth.Unit {
    [CreateAssetMenu(menuName = "Unit/Unit Data", fileName = "New Unit Data")]
    public class UnitData : ScriptableObject, ICloneable<UnitData> {
        [Tooltip("프리팹")] public Unit prefab;
        [Tooltip("오브젝트 타입")] public EntityType entityType;
        [Header("기본 데이터")]
        [Tooltip("최대 체력")] public int hp;
        [Tooltip("최대 스태미나")] public float stamina;
        [Tooltip("힘")] public int strength = 0;
        [Tooltip("이동속도")] public float moveSpeed = 2;
        [Tooltip("달리기 시, 이동속도 배율")] public float sprintSpeedMultiply = 2;
        [Tooltip("경험치")] public int exp = 0;

        public UnitData Clone() => new()
        {
            prefab = prefab,
            hp = hp,
            stamina = stamina,
            moveSpeed = moveSpeed,
            exp = exp,
            strength = strength,
            sprintSpeedMultiply = sprintSpeedMultiply
        };

        public void Reset()
        {
            hp = 0;
            stamina = 0;
            strength = 0;
            sprintSpeedMultiply = 0;
            moveSpeed = 0;
            exp = 0;
        }
    }
}