using UnityEngine;

namespace ByNorth.Unit.Modifier.StatsModifier
{
    [CreateAssetMenu(fileName = "Fixed", menuName = "Modifier/StatsModifier/Fixed")]
    public class StatsFixedModifier: StatsModifier
    {

        public override void Modify(Unit unit)
        {
            unit.CurrentData.hp += (int)hp;
            unit.CurrentData.stamina += stamina;
            unit.CurrentData.strength += (int)strength;
            unit.CurrentData.moveSpeed += moveSpeed;

            CheckMaximum(unit);
        }
        public override void Undo(Unit unit)
        {
            unit.CurrentData.hp -= (int)hp;
            unit.CurrentData.stamina -= stamina;
            unit.CurrentData.strength -= (int)strength;
            unit.CurrentData.moveSpeed -= moveSpeed;

            CheckMaximum(unit);
        }

        private void CheckMaximum(Unit unit)
        {
            // 최대치를 넘어가는 경우 줄어야 함
            if (unit.CurrentHp > unit.CurrentData.hp)
            {
                unit.TakeDamage(unit.CurrentHp - unit.CurrentData.hp);
            }
            if (unit.CurrentStamina > unit.CurrentData.stamina)
            {
                unit.ConsumeStamina(unit.CurrentStamina - unit.CurrentData.stamina);
            }
        }
    }
}