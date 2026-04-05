using System;
using ByNorth.Core;
using ByNorth.InputHandler;
using ByNorth.SlotSystem.Data;
using ByNorth.SlotSystem.Data.ItemData;
using ByNorth.SlotSystem.Data.SkillData;
using ByNorth.SlotSystem.Slot;
using ByNorth.Unit.Behaviour.Movable;
using Newtonsoft.Json;
using System.Collections.Generic;
using ByNorth.Core.GameFlow;
using ByNorth.InputHandler.InGame;
using ByNorth.UI.InGame;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;



namespace ByNorth.SlotSystem {
    [JsonObject(MemberSerialization.OptIn)]
    public class SlotManager : Singleton<SlotManager> {
        private const float DEFAULT_COOLDOWN_TIME = 0.5f;
        [Header("툴팁 상자")]
        public TooltipUI tooltipUI;
        [Header("퀵슬롯 기본위치, 이동위치")]
        public Transform defaultPos;    // 기본 위치 (하단)
        public Transform inventoryPos; // 인벤토리/스킬창 열릴 때 위치

        [Header("장비슬롯 기본위치, 이동위치")]
        public Transform defaultEquipPos;
        public Transform moveEquipPos;

        [Header("인벤토리 & 퀵슬롯 & 스킬슬롯 & 장비슬롯 & 상호작용용 인벤토리(상자) 슬롯 참조")]
        [Tooltip("인벤토리 슬롯")]
        [JsonProperty]
        public SlotGroup inventorySlotGroup;
        [Tooltip("스킬창 슬롯")]
        [JsonProperty]
        public SlotGroup skillSlotGroup;
        [Tooltip("장비창 슬롯")]
        [JsonProperty]
        public SlotGroup equipmentGroup;
        [Tooltip("퀵슬롯")]
        [JsonProperty]
        public SlotGroup[] quickSlotGroups;
        [Tooltip("상호작용 슬롯 (상자 등의 오브젝트에서 상호작용 시 해당 오브젝트가 가지고 있는 아이템을 보여주는 슬롯 그룹)")]
        public SlotGroup storageSlotGroup;


        private int selectedSlotIdx = -1;
        private int activeGroup = -1; // 퀵슬롯 체인지용 (기본적으로 퀵슬롯 1번을 메인으로) private

        public int QuickSlotCount => quickSlotGroups[0].slotsList.Count;

        void Start()
        {
            //ClearAllSlot();
            ToggleQuickSlotGroup();
            if (PlayerInfo.Instance.HasSaveData)
            {
                SaveManager.LoadSlotData();
            }
        }

        public int ReturnSupplyItemCount()
        {
            int count = 0;
            foreach (var slot in inventorySlotGroup.slotsList)
            {
                if (slot.slotData is SupplyData)
                {
                    count++;
                }
            }
            return count;
        }

        public void DeleteSupplyItem()
        {
            foreach (var slot in inventorySlotGroup.slotsList)
            {
                if (slot.slotData is SupplyData)
                {
                    slot.ClearSlot();
                }
            }
        }
        private void ClearAllSlot()
        {
            ClearAllSlots(equipmentGroup);
            ClearAllSlots(quickSlotGroups);
            ClearAllSlots(skillSlotGroup);
            ClearAllSlots(inventorySlotGroup);
            ClearAllSlots(storageSlotGroup);
        }

        /// <summary>
        /// 퀵슬롯을 1~4번의 단축키를 이용하여 포커스하는 메서드
        /// </summary>
        /// <param name="newValue"></param>
        public void UseQuickSlot(int newValue)
        {
            List<Slot.Slot> quickSlots = quickSlotGroups[activeGroup].slotsList;
            quickSlots[newValue]?.Use();
        }

        /// <summary>
        /// 인벤토리에 아이템을 넣는 메서드
        /// 아이템이 없을 경우 아이템을 새로 만들어 넣음.
        /// </summary>
        /// <param name="item">획득한 아이템</param>
        /// <returns>아이템이 있으면 ture, 없으면 false</returns>
        public bool AddItem(SlotData item)
        {
            for (int i = 0; i < inventorySlotGroup.slotsList.Count; i++)
            {
                Slot.Slot slot = inventorySlotGroup.slotsList[i];
                if (slot.slotData == null)
                {
                    slot.SetData(item);
                    RefreshAllQuickSlot();
                    return true;
                }
            }
            return false;
        }

        public int AddSkill(WeaponData weapon)
        {
            for (int i = 0; i < skillSlotGroup.slotsList.Count; i++)
            {
                Slot.Slot slot = skillSlotGroup.slotsList[i];
                if (slot.slotData is null)
                {
                    slot.SetData(weapon.skillData);
                    return i;
                }
            }
            return -1;
        }
        public void ToggleQuickSlotGroup()
        {
            activeGroup++;
            if (activeGroup >= quickSlotGroups.Length)
            {
                activeGroup = 0;
            }

            for (int i = 0; i < quickSlotGroups.Length; i++)
            {
                quickSlotGroups[i].Show(i == activeGroup);
                if (i == activeGroup)
                {
                    foreach (var slot in quickSlotGroups[i].slotsList)
                    {
                        slot.Unfocus();
                    }
                }
            }

            // 선택된 슬롯 초기화
            selectedSlotIdx = -1;
        }

        public void OpenInventory()
        {
            inventorySlotGroup.Show(true);
            ChangeSlotMode(true);
        }
        public void CloseInventory()
        {
            inventorySlotGroup.Show(false);
            ChangeSlotMode(false);
        }

        public void OpenStats()
        {
            skillSlotGroup.Show(true);
            ChangeSlotMode(true);
        }
        public void CloseStats()
        {
            skillSlotGroup.Show(false);
            ChangeSlotMode(false);
        }

        private void ChangeSlotMode(bool isPopupMode)
        {
            if (isPopupMode)
            {
                equipmentGroup.slotRoot.transform.SetParent(moveEquipPos, false);
                
                for (int i = 0; i < quickSlotGroups.Length; i++)
                {
                    quickSlotGroups[i].slotRoot.transform.SetParent(inventoryPos, false);
                    quickSlotGroups[i].slotRoot.SetActive(true);
                }
            }
            else
            {
                equipmentGroup.slotRoot.transform.SetParent(defaultEquipPos, false);
                equipmentGroup.slotRoot.transform.localScale = Vector3.one;
                equipmentGroup.slotRoot.transform.localPosition = Vector3.zero;
                
                for (int i = 0; i < quickSlotGroups.Length; i++)
                {
                    quickSlotGroups[i].slotRoot.transform.SetParent(defaultPos, false);
                    quickSlotGroups[i].slotRoot.SetActive(i == activeGroup);
                    quickSlotGroups[i].slotRoot.transform.localScale = Vector3.one;
                    quickSlotGroups[i].slotRoot.transform.localPosition = Vector3.zero;
                }
            }
        }

        public int GetCountItem(SlotData data)
        {
            if (data is not ItemData) return -1;

            int count = 0;
            foreach (var slot in inventorySlotGroup.slotsList)
            {
                if (slot.slotData?.Equals(data) ?? false)
                {
                    count++;
                }
            }
            return count;
        }
        public int GetSkillCost(SlotData data)
        {
            if (data is not SkillData) return -1;
            return (data as SkillData).cost;
        }
        public Slot.Slot GetEquipSlot(int slotIdx)
        {
            return equipmentGroup.slotsList[slotIdx];
        }
        public void RemoveSkillSlot(int idx)
        {
            skillSlotGroup.slotsList[idx].ClearSlot();
        }
        public void DeleteItem(ItemData item)
        {
            foreach (var slot in inventorySlotGroup.slotsList)
            {
                if (slot.slotData?.Equals(item) ?? false)
                {
                    slot.ClearSlot();
                    break;
                }
            }
            RefreshAllQuickSlot();
        }
        public void UseSelectedSlot()
        {
            Slot.Slot.SelectedSlot?.Use();
        }
        public Slot.Slot GetFocusedSlot()
        {
            return Slot.Slot.FocusedSlot;
        }
        /// <summary>
        /// 인벤토리에서 아이템을 더블클릭하여 사용했을 경우 
        /// <br>만약 해당 아이템이 퀵슬롯에 올라가 있다면 
        /// 해당 아이템에 쿨타임 표시를 나타내기 위해서 사용하는 메서드</br>
        /// </summary>
        public void CooldownAll(SlotData data)
        {
            List<Slot.Slot> slots = new();
            slots.AddRange(inventorySlotGroup.slotsList);
            slots.AddRange(skillSlotGroup.slotsList);
            foreach (var group in quickSlotGroups)
            {
                slots.AddRange(group.slotsList);
            }

            float cooldownTime = Time.time + DEFAULT_COOLDOWN_TIME;
            foreach (var slot in slots)
            {
                if (slot.slotData == data && data is SkillData skillData)
                {
                    slot.Cooldown(Time.time + skillData.cooltime);
                }
                else if (slot.CooldownTime < cooldownTime)
                {
                    slot.Cooldown(cooldownTime);
                }
            }
        }

        public void MoveSlotFocus(KeyCode keyCode)
        {
            if (Slot.Slot.FocusedSlot is null)
            {
                switch (InGameUIManager.PopupType)
                {
                    case PopupUIType.Stats:
                        skillSlotGroup.slotsList[0].OnFocus();
                        break;
                    case PopupUIType.Inventory:
                    default:
                        inventorySlotGroup.slotsList[0].OnFocus();
                        break;
                }
                return;
            }
            
            Slot.Slot next = keyCode switch
            {
                KeyCode.LeftArrow => Slot.Slot.FocusedSlot?.FindSelectableOnLeft()?.GetComponent<Slot.Slot>(),
                KeyCode.RightArrow => Slot.Slot.FocusedSlot?.FindSelectableOnRight()?.GetComponent<Slot.Slot>(),
                KeyCode.UpArrow => Slot.Slot.FocusedSlot?.FindSelectableOnUp()?.GetComponent<Slot.Slot>(),
                KeyCode.DownArrow => Slot.Slot.FocusedSlot?.FindSelectableOnDown()?.GetComponent<Slot.Slot>(),
                _ => null
            };
            next?.OnFocus();
        }
        public void ActionToPlayer() => InputManager.PlayerInput.SwitchCurrentActionMap("Player");
        public void RefreshAllQuickSlot()
        {
            foreach (var group in quickSlotGroups)
            {
                foreach (var slot in group.slotsList)
                {
                    slot.Refresh();
                }
            }
        }
        
        private void ClearAllSlots(params SlotGroup[] slotGroups)
        {
            foreach (var slotGroup in slotGroups)
            {
                if (slotGroup is null) return;
                
                foreach (var slot in slotGroup.slotsList)
                {
                    slot.ClearSlot();
                }
            }
        }

        /// <summary>
        /// 오브젝트가 가지고 있는 아이템 배열을 사용하여 interactionGroupSlot에 아이템을 채움
        /// </summary>
        public void AddAllItemInteractionSlot(List<SlotData> datas)
        {
            ClearAllSlots(storageSlotGroup);
            for (int i = 0; i < datas.Count; i++)
            {
                Slot.Slot slot = storageSlotGroup.slotsList[i];
                if (slot.slotData is null)
                {
                    slot.SetData(datas[i]);
                    slot.Refresh();
                }
            }
        }

        public List<SlotData> GetRemainingItems()
        {
            List<SlotData> returnData = new List<SlotData>();
            for(int i = 0; i < storageSlotGroup.slotsList.Count; i++)
            {
                returnData.Add(storageSlotGroup.slotsList[i].slotData);
            }
            return returnData;
        }
    }
}