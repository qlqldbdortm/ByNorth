using System;
using TMPro;
using UnityEngine;
using ByNorth.Unit;
using UnityEngine.Serialization;

namespace ByNorth.UI.StatUI {
    public class StatContainerHandler : StatUpgradeHandler {
        [Header("현재 스탯과 올라가는 스탯을 보여주는 TextMeshProUGUI")]
        public TextMeshProUGUI plusText;
        [Header("소모 경험치를 보여주는 TextMeshProUGUI")]
        public TextMeshProUGUI consumptionText;
        [Header("올라가는 스탯 종류")]
        public StatType statType;
        [Header("올라가는 스탯 양")]
        public int statAmount;

        private int StatLevel
        {
            get
            {
                int addedValue = 0;
                switch (statType)
                {
                    case StatType.Hp:
                        addedValue = Player.CurrentData.hp - Player.BaseData.hp;
                        break;
                    case StatType.Stamina:
                        addedValue = (int)(Player.CurrentData.stamina - Player.BaseData.stamina);
                        break;
                    case StatType.Strength:
                    default:
                        addedValue = (int)(Player.CurrentData.strength - Player.BaseData.strength);
                        break;
                }
                
                return addedValue / statAmount;
            }
        }

        public override void ApplyStat()
        {
            switch (statType)
            {
                case StatType.Hp:
                    Player.CurrentData.hp += statAmount;
                    Player.TakeHeal(statAmount);
                    break;

                case StatType.Stamina:
                    Player.CurrentData.stamina += statAmount;
                    Player.RecoverStamina(statAmount);
                    break;

                case StatType.Strength:
                    Player.CurrentData.strength += statAmount;
                    Player.RecoverStamina(statAmount);
                    break;
            }
        }

        public override void ConsumptionExp(float consumptionExp)
        {
            Player.CurrentData.exp -= (int)consumptionExp;
        }

        public override float ConsumptionExpReturn(int level)
        {
            return Mathf.Pow(2, level) * 100;
        }

        public override void StatUp()
        {
            float requiredExp = ConsumptionExpReturn(StatLevel);

            if (Player.CurrentData.exp >= requiredExp)
            {
                ConsumptionExp(requiredExp);
                ApplyStat();
            }
            else
            {
                Debug.Log("경험치 부족");
            }

            UpdateStatText();
        }

        public override void UpdateStatText()
        {
            switch (statType)
            {
                case StatType.Hp:
                    plusText.text = $"{Player.BaseData.hp} + <color=green>{statAmount * StatLevel}</color>";
                    break;

                case StatType.Stamina:
                    plusText.text = $"{Player.BaseData.stamina} + <color=green>{statAmount * StatLevel}</color>";
                    break;

                case StatType.Strength:
                    plusText.text = $"{Player.BaseData.strength} + <color=green>{statAmount * StatLevel}</color>";
                    break;
            }
            consumptionText.text = $"소모 경험치 : {ConsumptionExpReturn(StatLevel)}";
        }
    }
}
