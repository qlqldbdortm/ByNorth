using System;
using ByNorth.SlotSystem.Data;
using ByNorth.SlotSystem.Data.ItemData;
using ByNorth.SlotSystem.Data.SkillData;
using DG.Tweening;
using System.Runtime.CompilerServices;
using System.Threading;
using ByNorth.Core;
using ByNorth.Extensions;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;


namespace ByNorth.SlotSystem.Slot {
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class Slot : Selectable, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler,
        IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public static Slot FocusedSlot { get; private set; } = null;
        public static Slot SelectedSlot { get; private set; } = null;
        
        
        [Header("아이템 & 스킬")]
        public SlotData slotData;

        [Header("UI")]
        [Tooltip("선택 표시용 이미지")] public GameObject selectedImage;
        [Tooltip("포커스 표시용 이미지")] public GameObject focusedImage;
        [Tooltip("아이템 이미지")] public Image icon;
        
        [Tooltip("쿨다운 마스크")]public Image cooldownMask;
        [Tooltip("쿨다운 텍스트")] public TextMeshProUGUI cooldownText;


        // TODO: 만약, 해당 아이템의 특이한 수치가 더 존재한다면 그것도 따로 받을 방법을 강구해야 할 필요가 있어보임.
        /// <summary>
        /// Save에서 Uid값을 저장할 수 있게 만든 Reflection용 Property<br/>
        /// 따로 사용하지 말아야 함.
        /// </summary>
        [JsonProperty]
        public int ItemId
        {
            get => slotData?.uid ?? -1;
            set
            {
                int uid = value;
                if (uid < 0) // -1은 아이템이 없는 상태임.
                {
                    slotData = null;
                }
                else
                {
                    slotData = DataManager.GetItemData(uid);
                    print(slotData.itemName);
                }
                Refresh();
            }
        }
        
        public virtual bool CanUse => CooldownTime <= Time.time;
        public float CooldownTime { get; private set; } = 0;


        private Canvas canvas;
        private Transform parentAfterDrag;
        private CancellationTokenSource token = new();
        
        
        void Awake()
        {
            FindCanvas();
            Unfocus();
        }

        void OnDestroy()
        {
            token.Cancel();
            Unfocus();
            Unselect();
        }


        public void SetData(SlotData data)
        {
            slotData = data;
            
            Refresh();
        }

        private void FindCanvas()
        {
            canvas = GetComponentInParent<Canvas>();
        }
        /// <summary>
        /// 인벤토리의 슬롯을 초기화
        /// </summary>
        public virtual void ClearSlot()
        {
            slotData = null;
            icon.sprite = null;
            icon.enabled = false;
        }
        public virtual void Refresh()
        {
            icon.enabled = slotData;
            icon.sprite = slotData?.icon;
            icon.rectTransform.localPosition = Vector3.zero;
        }


        public new void Select()
        {
            if (SelectedSlot == this)
            {
                Use();
                SelectedSlot.Unselect();
                return;
            }
            
            SelectedSlot?.Unselect();
            
            SelectedSlot = this;
            selectedImage.SetActive(true);
        }
        public void Unselect()
        {
            SelectedSlot = null;
            selectedImage.SetActive(false);
        }

        /// <summary>
        /// 슬롯이 선택되었을 때 선택된 슬롯의 Background의 색상 변경
        /// </summary>
        public void OnFocus()
        {
            FocusedSlot?.Unfocus();
            FocusedSlot = this;
            focusedImage.SetActive(true);
            if ((this is InventorySlot || this is StorageSlot || this is SkillSlot) && slotData is not null)
            {
                SlotManager.Instance.tooltipUI.ShowTooltip(slotData);
            }
        }

        /// <summary>
        /// 슬롯 선택이 해제되었을 때 해당 슬롯의 Background를 초기화
        /// </summary>
        public void Unfocus()
        {
            focusedImage.SetActive(false);
            if ((this is InventorySlot || this is StorageSlot || this is SkillSlot) && slotData is not null)
            {
                SlotManager.Instance.tooltipUI.HideTooltip();
            }
        }

        public void Cooldown(float cooldownTime)
        {
            CooldownTime = cooldownTime;
            
            _ = CooldownAsync();
        }


        private async UniTask CooldownAsync()
        {
            cooldownMask.gameObject.SetActive(true);
            
            float startTime = Time.time;
            float remTime = CooldownTime - startTime;
            while (CooldownTime > Time.time)
            {
                await UniTask.Yield(cancellationToken: token.Token);
                
                float t = Time.time - startTime;
                float amount = 1 - (t / remTime);
                
                cooldownMask.fillAmount = amount;
                cooldownText?.SetText($"{CooldownTime - Time.time:F1}");
            }

            cooldownMask.gameObject.SetActive(false);
        }
        
        public abstract void Use();
        public abstract void OnSwap(Slot fromSlot);

        #region DragEvent
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (slotData is null) return;

            icon.raycastTarget = false;
            parentAfterDrag = icon.transform.parent;
            icon.transform.SetParent(canvas.transform); // 캔버스 최상단
            
            Select();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (slotData is null) return;

            icon.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            icon.raycastTarget = true;
            icon.transform.SetParent(parentAfterDrag);
            icon.transform.localPosition = Vector3.zero;

            if (slotData is not null)
            {
                FocusedSlot?.OnSwap(this);
            }
            SelectedSlot?.Unselect();
        }
        #endregion

        #region PointerEvent
        protected float lastClickTime = 0;
        protected const float DOUBLE_CLICK_THRESHOLD = 0.3f;
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            float timeSinceLastClick = Time.time - lastClickTime;
            if (timeSinceLastClick <= DOUBLE_CLICK_THRESHOLD)
            {
                Use();
            }
            lastClickTime = Time.time;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnFocus();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Unfocus();
            if (FocusedSlot == this)
            {
                FocusedSlot = null;
            }
        }
        #endregion
    }
}