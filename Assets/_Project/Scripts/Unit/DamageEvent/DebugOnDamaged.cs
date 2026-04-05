using UnityEngine;

namespace ByNorth.Unit.DamageEvent
{
    public class DebugOnDamaged: MonoBehaviour, IDamaged
    {
        public void OnDamaged(Unit unit, int damage, DamageType damageType)
        {
            print($"HP: {unit.CurrentHp}/{unit.CurrentData.hp} - {damage}({damageType})");
        }
    }
}