using ByNorth.Unit.Behaviour.Movable;
using UnityEngine;

namespace ByNorth.Unit.DamageEvent
{
    public class MoraleOnDamaged: MonoBehaviour, IDamaged
    {
        private NonPlayerHandler handler = null;
        
        
        void Awake()
        {
            handler = GetComponent<NonPlayerHandler>();
        }
        
        
        public void OnDamaged(Unit unit, int damage, DamageType damageType)
        {
            if (damageType == DamageType.Miss) return;
           
            handler.Morale -= Random.Range(damage >> 1, damage);
        }
    }
}