using ByNorth.ActionSystem;
using ByNorth.Core;
using UnityEngine;

namespace ByNorth.Unit.Behaviour
{
    public class BossDataManager:Singleton<BossDataManager>
    {
        [Tooltip("충격파 공격 데이터")] public ActionData earthquakeData;
        [Tooltip("충격파 공격 데이터")] public ActionData shotData;
        
        [Tooltip("충격파 공격 데이터")] public UnitData createUnitData;
        [Tooltip("충격파 공격 데이터")] public int createCount = 3;
        [Tooltip("충격파 공격 데이터")] public float createUnitRange = 10;
        
        public AudioClip earthquakeSound;
        public AudioClip shotSound;
        
    }
}