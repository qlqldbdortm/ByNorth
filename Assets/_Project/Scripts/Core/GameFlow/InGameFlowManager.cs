using ByNorth.Core.VillageSystem;
using ByNorth.Unit.Behaviour.Movable;
using ByNorth.Unit.Behaviour.Structure;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

namespace ByNorth.Core.GameFlow
{
    public class InGameFlowManager: Singleton<InGameFlowManager>
    {
        public VisualEffect snowEffect;

        private GameObject villageObject = null;
        
        
        protected override void Awake()
        {
            base.Awake();
            
            snowEffect.SetFloat("Karma", PlayerInfo.Instance.NormalKarma);
            CreateVillage();
        }


        public void CreateVillage()
        {
            SaveManager.Save();
            if (villageObject != null)
            {
                VillageManager.CurrentVillage.VillageUnitAllDespawn();
                DestroyImmediate(villageObject);
                villageObject = null;
            }

            if (PlayerInfo.Instance.VillageIds.Count <= 0)
            {
                PlayerInfo.Instance.HasClear = true;
                SceneManager.LoadScene("End");
                return;
            }
            
            int villageId = PlayerInfo.Instance.VillageIds[0];
            int resourceId = PlayerInfo.Instance.ResourceIds[0];

            if (villageId < 0) // 보스전
            {
                StoragableManager.Instance.storagables.Clear();
                
                int bossId = villageId + DataManager.Instance.bosses.Length;
                villageObject = Instantiate(DataManager.Instance.bossField, Vector3.zero, Quaternion.identity);
                Instantiate(DataManager.Instance.bosses[bossId], Vector3.zero, Quaternion.identity);

                PlayerHandler.Player.Controller.enabled = false;
                PlayerHandler.Player.transform.position = Vector3.back * 10;
                PlayerHandler.Player.Controller.enabled = true;
            }
            else
            {
                VillageManager village = Instantiate(DataManager.Instance.villages[villageId], Vector3.zero, Quaternion.identity);
                village.Init(DataManager.Instance.resourceData[resourceId]);
                villageObject = village.gameObject;
            }
            PlayerInfo.Instance.VillageIds.RemoveAt(0);
            PlayerInfo.Instance.ResourceIds.RemoveAt(0);
        }
    }
}