using ByNorth.Core.GameFlow;
using ByNorth.SlotSystem;
using CW.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Windows;

namespace ByNorth.Core
{
    // TODO:
    //  모든 저장은 SaveManager를 통해서 진행하며, 기존의 NewtonJsonConvert형태인
    //  BlackList형태가 아닌, WhiteList형태로 진행할 것임.
    //  Save를 받는 피 클래스는
    //  [JsonObject(MemberSerialization.OptIn)] Attribute를 달게 되고,
    //  Save되는 피 필드는
    //  [JsonProperty] Attribute를 달아야 함.
    public static class SaveManager
    {
        public static void Save()
        {
            // TODO:
            //  1. 현재는 테스트를 위해 대충 Path를 정했지만,
            //  추후에는 PlayerPrefs나, 제대로 된 위치에 Save해야 함.
            //  2. 해당 위치에선 Method를 호출해서 SaveFile로 만드는 작업만 해야 하고,
            //  JsonConvert는 별도의 Method를 만들어서 작업하는 걸 추천함.
            //  ### 하단의 SaveSlotData Method 참조 ###
            PlayerPrefs.SetString("PlayerInfo", SavePlayerInfo());
            PlayerPrefs.SetString("SlotData", SaveSlotData());
        }

        public static void LoadPlayerInfo()
        {
            string saveData = PlayerPrefs.GetString("PlayerInfo");
            JsonConvert.PopulateObject(saveData, PlayerInfo.Instance);
        }

        public static void LoadSlotData()
        {
            string saveData = PlayerPrefs.GetString("SlotData");

            int idx = 0;
            JObject obj = JObject.Parse(PlayerPrefs.GetString("SlotData"));
            foreach (var n in obj["inventorySlotGroup"]["slotsList"])
            {
                int uid = int.Parse(n["ItemId"].ToString());
                SlotManager.Instance.inventorySlotGroup.slotsList[idx++].ItemId = uid;
            }

            idx = 0;
            obj = JObject.Parse(PlayerPrefs.GetString("SlotData"));
            foreach (var n in obj["skillSlotGroup"]["slotsList"])
            {
                int uid = int.Parse(n["ItemId"].ToString());
                SlotManager.Instance.skillSlotGroup.slotsList[idx++].ItemId = uid;
            }
            
            idx = 0;
            obj = JObject.Parse(PlayerPrefs.GetString("SlotData"));
            foreach (var n in obj["equipmentGroup"]["slotsList"])
            {
                int uid = int.Parse(n["ItemId"].ToString());
                SlotManager.Instance.equipmentGroup.slotsList[idx++].ItemId = uid;
            }
            
            idx = 0;
            obj = JObject.Parse(PlayerPrefs.GetString("SlotData"));
            foreach (var n in obj["quickSlotGroups"])
            {
                int slotIdx = 0;
                foreach (var nn in n["slotsList"])
                {
                    int uid = int.Parse(nn["ItemId"].ToString());
                    SlotManager.Instance.quickSlotGroups[idx].slotsList[slotIdx++].ItemId = uid;
                }
                idx++;
            }
        }
        
        private static string SavePlayerInfo() => JsonConvert.SerializeObject(PlayerInfo.Instance);
        private static string SaveSlotData() => JsonConvert.SerializeObject(SlotManager.Instance);
    }
}