using System;
using System.Text;
using ByNorth.InputHandler;
using ByNorth.SlotSystem.Data;
using ByNorth.SlotSystem.Data.ItemData;
using ByNorth.SlotSystem.Data.SkillData;
using ByNorth.SlotSystem.Slot;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ByNorth.Extensions
{
    public static class Extensions {
        /// <summary>
        /// y좌표를 무시하고 XZ 평면에서의 거리를 계산
        /// </summary>
        public static float DistanceXZ(this Vector3 from, Vector3 to)
        {
            Vector2 fromXZ = new Vector2(from.x, from.z);
            Vector2 toXZ = new Vector2(to.x, to.z);
            return Vector2.Distance(fromXZ, toXZ);
        }

        public static string ItemToolTip(this SlotData data)
        {
            StringBuilder sb = new();
            if (data is not SkillData skill)
            {
                sb.AppendLine($"아이템 이름 : <b>{data.itemName}</b>");
                if (data is ItemData itemData)
                {
                    sb.AppendLine($"아이템 종류 : {itemData.itemType}");
                    sb.AppendLine($"아이템 설명 : {itemData.description}");
                }

                if (data is WeaponData weapon)
                {
                    sb.AppendLine($"기본 공격 대미지 :  {weapon.normalAttackData.damage}");
                    sb.AppendLine($"소비 스태미나 : {weapon.normalAttackData.stamina}");
                    sb.AppendLine($"강한 공격 대미지 :  {weapon.hardAttackData.damage}");
                    sb.AppendLine($"소비 스태미나 : {weapon.hardAttackData.stamina}");
                }
                else if (data is ArmorData armor)
                {
                    sb.AppendLine($"피해 감소율 : {armor.damageReduction}");
                }
                else if (data is ConsumableData consumable)
                {
                    if (consumable.amountOfRecovery == 0)
                    {
                        sb.AppendLine("소비아이템 종류 : 특수 효과 아이템");
                    }
                    else
                    {
                        sb.AppendLine($"소비 아이템 종류 : {consumable.consumeType.ToString()}");
                        sb.AppendLine($"아이템 회복량 : {consumable.amountOfRecovery}");
                    }
                }
            }
            else
            {
                sb.AppendLine($"스킬 이름 : <b>{skill.skillName}</b>");
                sb.AppendLine($"스킬 설명 : {skill.skillDescription}");
                sb.AppendLine($"스킬 쿨타임 : {skill.cooltime}");
                sb.AppendLine($"스킬 코스트 : {skill.cost} Stamina");
                if (skill is TwoPrefabSkillData twoSkill)
                {
                    sb.AppendLine($"스킬 1타 대미지 : {twoSkill.primaryAttackData.damage}");
                    sb.AppendLine($"스킬 2타 대미지 : {twoSkill.secondaryAttackData.damage}");
                }
                else
                {
                    sb.AppendLine($"스킬 1타 대미지 : {skill.primaryAttackData.damage}");
                }
            }

            return sb.ToString();
        }
        
        public static void AddAction(this InputActionMap actionMap, string actionName,
            Action<InputAction.CallbackContext> started, Action<InputAction.CallbackContext> performed, Action<InputAction.CallbackContext> canceled)
        {
            InputAction inputAction = actionMap.FindAction(actionName);
            if (started is not null)
            {
                inputAction.started += started;
                InputManager.OnChangedScene += () => inputAction.started -= started;
            }
            if (performed is not null)
            {
                inputAction.performed += performed;
                InputManager.OnChangedScene += () => inputAction.performed -= performed;
            }
            if (canceled is not null)
            {
                inputAction.canceled += canceled;
                InputManager.OnChangedScene += () => inputAction.canceled -= canceled;
            }
        }

        public static void AddAction(this InputActionMap actionMap, string actionName,
            Action<InputAction.CallbackContext> action)
        {
            InputAction inputAction = actionMap.FindAction(actionName);
            inputAction.started += action;
            inputAction.performed += action;
            inputAction.canceled += action;
            InputManager.OnChangedScene += () =>
            {
                inputAction.started -= action;
                inputAction.performed -= action;
                inputAction.canceled -= action;
            };
        }
    }
}
