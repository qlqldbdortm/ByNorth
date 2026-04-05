using UnityEngine;

namespace ByNorth.Unit.Modifier.StatsModifier
{
    [CreateAssetMenu(fileName = "Rate", menuName = "Modifier/StatsModifier/Rate")]
    public class StatsRateModifier: StatsModifier
    {
        public override void Modify(Unit unit)
        {
            int hp = Mathf.RoundToInt(unit.BaseData.hp * this.hp);
            float stamina = (int)(unit.BaseData.stamina * this.stamina);
            int strength = Mathf.RoundToInt(unit.BaseData.strength * this.strength);
            float moveSpeed = (int)(unit.BaseData.moveSpeed * this.moveSpeed);
            
            unit.CurrentData.hp += hp;
            unit.CurrentData.stamina += stamina;
            unit.CurrentData.strength += strength;
            unit.CurrentData.moveSpeed += moveSpeed;
            
            CheckMaximum(unit);
        }

        public override void Undo(Unit unit)
        {
            int hp = Mathf.RoundToInt(unit.BaseData.hp * this.hp);
            float stamina = (int)(unit.BaseData.stamina * this.stamina);
            int strength = Mathf.RoundToInt(unit.BaseData.strength * this.strength);
            float moveSpeed = (int)(unit.BaseData.moveSpeed * this.moveSpeed);
            
            unit.CurrentData.hp -= hp;
            unit.CurrentData.stamina -= stamina;
            unit.CurrentData.strength -= strength;
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