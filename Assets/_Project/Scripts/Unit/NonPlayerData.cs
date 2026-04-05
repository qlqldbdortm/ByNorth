using ByNorth.Core;
using UnityEngine;

namespace ByNorth.Unit
{
    [CreateAssetMenu(menuName = "Unit/Non Player Data", fileName = "New Non Player Data")]
    public class NonPlayerData: ScriptableObject, ICloneable<NonPlayerData>
    {
        [Tooltip("감지 범위")] public float sightRange = 5;
        [Tooltip("감지 범위")] public float morale = 0;


        public NonPlayerData Clone() => new()
        {
            sightRange = sightRange,
            morale = morale,
        };
    }
}