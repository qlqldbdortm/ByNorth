using ByNorth.Core;
using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace ByNorth.Effect
{
    public class EffectManager : Singleton<EffectManager>
    {
        public Transform objectGroup;

        public EffectHandler GetEffect(EffectHandler prefab) => LeanPool.Spawn(prefab, objectGroup);

        public EffectHandler GetVFXEffect(EffectHandler prefab)
        {
            var obj = LeanPool.Spawn(prefab, objectGroup);
            var vfx = obj.GetComponentInChildren<VisualEffect>();
            vfx?.Play();
            return obj;
        }
    }
}
