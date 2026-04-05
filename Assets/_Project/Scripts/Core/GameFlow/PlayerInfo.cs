using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.VFX;

namespace ByNorth.Core.GameFlow
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PlayerInfo: Singleton<PlayerInfo>
    {
        private const int STAGE_SIZE = 6;
        
        
        [JsonProperty]
        public List<int> VillageIds { get; set; } = null;
        [JsonProperty]
        public List<int> ResourceIds { get; set; } = null;
        
        [JsonProperty]
        public int NormalKarma { get; set; } = 0;
        [JsonProperty]
        public int ChildKarma { get; set; } = 0;
        public bool HasSaveData { get; set; } = false;
        public bool HasClear { get; set; } = false;
        private VisualEffect SnowEffect => InGameFlowManager.Instance.snowEffect;
        private KarmaSight KarmaSight => GetComponent<KarmaSight>();

        public void RandomNextStage()
        {
            HasClear = false;
            NormalKarma = 0;
            ChildKarma = 0;
            
            VillageIds = new(STAGE_SIZE);
            ResourceIds = new(STAGE_SIZE);
            for (int i = 0; i < STAGE_SIZE; i++)
            {
                int id = 0;
                if (i % 2 == 1) // 보스전
                {
                    id = -Random.Range(1, DataManager.Instance.bosses.Length + 1); 
                }
                else // 민간지역 약탈
                {
                    id = Random.Range(0, DataManager.Instance.villages.Length);
                }
                VillageIds.Add(id);
                ResourceIds.Add(Random.Range(0, DataManager.Instance.resourceData.Length));
                print($"{VillageIds[i]}, {ResourceIds[i]}");
            }
        }

        public void AddNormalKarma(int amount)
        {
            NormalKarma += amount;
            
            SnowEffect.SetFloat("Karma", NormalKarma);
        }
        public void AddChildKarma(int amount)
        {
            ChildKarma += amount;
            
            KarmaSight.UpdateVignette(ChildKarma);
        }
    }
}