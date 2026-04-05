using System.Linq;
using ByNorth.SlotSystem.Data.ItemData;
using ByNorth.Unit;
using UnityEditor;
using UnityEngine;

namespace ByNorth.Core.VillageSystem
{
    [CreateAssetMenu(menuName = "Village/Resource Data", fileName = "new Village Resource Data")]
    public class VillageResourceData: ScriptableObject
    {
        public CreateData<UnitData>[] createUnitData;
        public CreateData<ItemData>[] createItemData;

        #if UNITY_EDITOR
        void Reset()
        {
            string[] guids = AssetDatabase.FindAssets("t:ItemData");
            createItemData = guids.Select(guid => new CreateData<ItemData>(AssetDatabase.LoadAssetAtPath<ItemData>(AssetDatabase.GUIDToAssetPath(guid)), 0)).ToArray();
            var items = createItemData.ToList();
            items.Sort((x, y) => x.data.uid.CompareTo(y.data.uid));
            createItemData = items.ToArray();
        }
        #endif
    }
}