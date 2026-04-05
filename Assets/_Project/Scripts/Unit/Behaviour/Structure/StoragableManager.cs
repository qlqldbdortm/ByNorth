using System;
using ByNorth.Core;
using ByNorth.SlotSystem;
using System.Collections.Generic;
using System.Threading;
using ByNorth.SlotSystem.Data;
using ByNorth.UI;
using ByNorth.Unit.Behaviour.Movable;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ByNorth.Unit.Behaviour.Structure
{
    public class StoragableManager : Singleton<StoragableManager>
    {
        public WorldCanvasTracker interactiveIcon;
        public float interactionRange = 5f;
        
        
        private Transform Player => PlayerHandler.Player?.transform ?? null;
        private SlotGroup StorageSlotGroup => SlotManager.Instance.storageSlotGroup;
        public bool HasSelected => selectedStoragable is not null;
        
        
        public List<IStoragable> storagables = new();
        private IStoragable selectedStoragable = null;

        private IStoragable opendStoragable = null;

        private CancellationTokenSource token = null;


        void Start()
        {
            token = new();
            _ = CycleAsync();
        }
        void OnDestroy()
        {
            token.Cancel();
        }

        private async UniTask CycleAsync()
        {
            while (true)
            {
                await UniTask.WaitForSeconds(0.1f, cancellationToken: token.Token);
            
                if(StorageSlotGroup.IsVisible) continue;
            
                IStoragable nearest = FindNearestInteractable();
                if (nearest != selectedStoragable)
                {
                    // 이전 interactable 범위 벗어남 처리
                    if (selectedStoragable != null)
                    {
                        interactiveIcon.gameObject.SetActive(false);
                        CloseStorage();
                    }

                    selectedStoragable = nearest;
                    if (selectedStoragable == null) continue;
                
                    interactiveIcon.Target = selectedStoragable.Transform;
                    interactiveIcon.gameObject.SetActive(true);
                }
            }
        }

        public void OpenStorage()
        {
            opendStoragable = selectedStoragable;
            
            Queue<SlotData> queue = new(opendStoragable.OpenStorage());
            foreach (var slot in StorageSlotGroup.slotsList)
            {
                slot.slotData = queue.Count > 0 ? queue.Dequeue() : null;
                slot.Refresh();
            }
            StorageSlotGroup.Show(true);
        }

        public void CloseStorage()
        {
            if (opendStoragable is null) return;
            
            List<SlotData> data = new();
            foreach (var slot in StorageSlotGroup.slotsList)
            {
                if(slot.slotData == null) continue;
                data.Add(slot.slotData);
            }
            
            opendStoragable.CloseStorage(data);
            StorageSlotGroup.Show(false);
            opendStoragable = null;
        }

        private IStoragable FindNearestInteractable()
        {
            IStoragable nearest = null;
            float minDist = float.MaxValue;

            foreach (var storagable in storagables)
            {
                float dist = Vector3.Distance(Player.position, storagable.Transform.position);
                if (dist <= interactionRange && dist < minDist)
                {
                    nearest = storagable;
                    minDist = dist;
                }
            }
            return nearest;
        }
    }
}
