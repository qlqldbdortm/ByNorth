using ByNorth.Core;
using ByNorth.Effect;
using ByNorth.Unit.Modifier;
using Lean.Pool;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ByNorth.Unit.Modifier {
    public class ModifierIconManager : Singleton<ModifierIconManager> {
        public GameObject iconPrefab;
        public Transform modifierWindow;

        private Dictionary<Sprite, GameObject> modifierDic = new Dictionary<Sprite, GameObject>();
        public void AddModifierIcon(ModifierBase modifier)
        {
            if (modifierDic.ContainsKey(modifier.icon)) return;
            GameObject newIcon = LeanPool.Spawn(iconPrefab, modifierWindow);
            newIcon.GetComponent<Image>().sprite = modifier.icon;
            modifierDic.Add(modifier.icon, newIcon);
        }

        public void RemoveModifierIcon(ModifierBase modifier)
        {
            if(modifierDic.TryGetValue(modifier.icon, out GameObject removeIcon))
            {
                LeanPool.Despawn(removeIcon);
                modifierDic.Remove(modifier.icon);
            }
        }
    }
}
