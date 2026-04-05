using System.Collections.Generic;
using System.Linq;
using ByNorth.Core.VillageSystem;
using ByNorth.SlotSystem.Data;
using ByNorth.Unit.Behaviour.Movable;
using UnityEditor;
using UnityEngine;

namespace ByNorth.Core
{
    public class DataManager: Singleton<DataManager>
    {
        [SerializeField] private SlotData[] slotData;

        [SerializeField] public VillageResourceData[] resourceData;

        [SerializeField] public VillageManager[] villages;
        [SerializeField] public GameObject bossField;
        [SerializeField] public BossHandler[] bosses;


        public static Dictionary<int, SlotData> ItemData { get; } = new();
        

        protected override void Awake()
        {
            base.Awake();
            
            // SlotData를 Dictionary indexing용으로 가공
            foreach (var data in slotData)
            {
                ItemData.Add(data.uid, data);
            }
        }
        #if UNITY_EDITOR
        void Reset()
        {
            // SlotData 삽입
            string[] guids = AssetDatabase.FindAssets("t:SlotData");
            slotData = guids.Select(guid => AssetDatabase.LoadAssetAtPath<SlotData>(AssetDatabase.GUIDToAssetPath(guid))).ToArray();
            
            // VillageResourceData 삽입
            guids = AssetDatabase.FindAssets("t:VillageResourceData");
            resourceData = guids.Select(guid => AssetDatabase.LoadAssetAtPath<VillageResourceData>(AssetDatabase.GUIDToAssetPath(guid))).ToArray();
            
            // VillagePrefab, BossPrefab 삽입
            guids = AssetDatabase.FindAssets("t:prefab");
            List<VillageManager> villageList = new();
            List<BossHandler> bossList = new();
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab.TryGetComponent(out VillageManager village))
                {
                    villageList.Add(village);
                }
                else if (prefab.TryGetComponent(out BossHandler boss))
                {
                    bossList.Add(boss);
                }
            }
            villages = villageList.ToArray();
            bosses = bossList.ToArray();
        }
        #endif
        
        
        public static SlotData GetItemData(int uid) => ItemData.GetValueOrDefault(uid);
    }
}