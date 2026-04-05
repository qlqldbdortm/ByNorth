using System.Collections;
using ByNorth.Unit.Behaviour.Structure;
using System.Collections.Generic;
using ByNorth.SlotSystem.Data;
using ByNorth.Unit;
using ByNorth.Unit.Behaviour.Movable;
using ByNorth.Unit.DieEvent;
using UnityEngine;

namespace ByNorth.Core.VillageSystem
{
    public class VillageManager: MonoBehaviour
    {
        public static VillageManager CurrentVillage = null;


        public static List<BreakOnDie> BrokenObjects { get; } = new();
        
        public List<NonPlayerHandler> spawnedUnits; 
        
        public Vector3 playerPos;
        public Transform[] evacuatePoints;
        public StructureHandler[] allStructures;
        public SpawnUnitOnDie[] spawnUnitOnDies;
            
        void Reset()
        {
            playerPos = transform.Find("PlayerPoint").position;
            List<Transform> points = new();
            foreach (Transform point in transform.Find("EvacuateGroup"))
            {
                points.Add(point);
            }
            evacuatePoints = points.ToArray();
            allStructures = GetComponentsInChildren<StructureHandler>();
            spawnUnitOnDies = GetComponentsInChildren<SpawnUnitOnDie>();
        }


        public void Init(VillageResourceData resourceData)
        {
            CurrentVillage = this;
            BrokenObjects.Clear();
            StoragableManager.Instance.storagables.Clear();

            // 그냥 아이템을 꺼낼 수 있는 Box형태의 데이터를 모두 갖고오기
            foreach (var structure in allStructures)
            {
                if (structure is BoxHandler boxHandler)
                {
                    StoragableManager.Instance.storagables.Add(boxHandler);
                }
            }

            // 집 순서 섞기
            List<SpawnUnitOnDie> spawnPoints = new(spawnUnitOnDies);
            foreach (SpawnUnitOnDie spawnPoint in spawnPoints)
            {
                spawnPoint.CreateUnits.Clear();
            }
            for (int i = 0; i < spawnPoints.Count; i++)
            {
                for (int j = 0; j < spawnPoints.Count; j++)
                {
                    int r = Random.Range(0, spawnPoints.Count);

                    (spawnPoints[i], spawnPoints[r]) = (spawnPoints[r], spawnPoints[i]);
                }
            }
            
            // 빈 집 정하기
            int emptySize = Random.Range(0, Mathf.FloorToInt(spawnUnitOnDies.Length * 0.05f));
            spawnPoints.RemoveRange(0, emptySize);
            
            // 넣어야 할 사람 데이터 생성
            Stack<UnitData> unitStack = new(200);
            foreach (var createData in resourceData.createUnitData)
            {
                for (int i = 0; i < createData.count; i++)
                {
                    unitStack.Push(createData.data);
                }
            }

            // 사람 집에 넣기
            int idx = 0;
            while (unitStack.Count > 0)
            {
                spawnPoints[idx].CreateUnits.Add(unitStack.Pop());

                if (++idx >= spawnPoints.Count)
                {
                    idx = 0;
                }
            }
            
            // 창고 갖고와서 섞기
            List<IStoragable> storagables = new(GetComponentsInChildren<IStoragable>());
            for (int i = 0; i < storagables.Count; i++)
            {
                for (int j = 0; j < storagables.Count; j++)
                {
                    int r = Random.Range(0, storagables.Count);

                    (storagables[i], storagables[r]) = (storagables[r], storagables[i]);
                }
            }
            
            // 넣어야 할 아이템 데이터 생성
            Stack<SlotData> itemStack = new(200);
            foreach (var createData in resourceData.createItemData)
            {
                for (int i = 0; i < createData.count; i++)
                {
                    itemStack.Push(createData.data);
                }
            }

            // 아이템 창고에 넣기
            idx = 0;
            while (itemStack.Count > 0 && storagables.Count > 0)
            {
                storagables[idx].AddItem(itemStack.Pop());

                if (storagables[idx].HasFull)
                {
                    storagables.RemoveAt(idx);
                    idx--;
                }
                
                if (++idx >= storagables.Count)
                {
                    idx = 0;
                }
            }

            StartCoroutine(PlayerPositioningCo());
        }

        private IEnumerator PlayerPositioningCo()
        {
            yield return new WaitUntil(() => PlayerHandler.Player);

            PlayerHandler.Player.transform.position = playerPos;
        }
        
        /// <summary>
        /// 마을에 NPC가 생성되면 같이 실행되어야 할 메서드 
        /// </summary>
        /// <param name="npc">소환된 NPC</param>
        public void VillageUnitSpawn(NonPlayerHandler npc)
        {
            if (!spawnedUnits.Contains(npc))
            {
                spawnedUnits.Add(npc);
            }
        }
        /// <summary>
        /// 마을에서 NPC가 죽으면 같이 실행되어야 할 메서드
        /// </summary>
        /// <param name="npc">죽은 NPC</param>
        public void VillageUnitDespawn(NonPlayerHandler npc)
        {
            if (spawnedUnits.Contains(npc))
            {
                spawnedUnits.Remove(npc);
            }
        }
        /// <summary>
        /// 스테이지가 넘어가게 되면 기존에 있던 NPC 데이터를 지우고 Despawn 하는 메서드
        /// </summary>
        public void VillageUnitAllDespawn()
        {
            var units = new List<NonPlayerHandler>(spawnedUnits);
            foreach (var npc in units)
            {
                var despawnUnit = npc?.GetComponent<Unit.Unit>();
                npc?.OnRelease(despawnUnit);
            }
            spawnedUnits.Clear();
        }
        
        /// <summary>
        /// 마을에 소환된 NPC 리스트를 모두 가져오는 메서드
        /// </summary>
        /// <returns></returns>
        public List<NonPlayerHandler> GetAllVillageUnits() => spawnedUnits;
    }
}