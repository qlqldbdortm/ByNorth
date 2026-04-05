using System;
using ByNorth.UI;
using UnityEngine;

namespace ByNorth.Unit.DamageEvent
{
    public class TextOnDamaged: IDamaged
    {
        // ReSharper disable Unity.PerformanceAnalysis
        public void OnDamaged(Unit unit, int damage, DamageType damageType)
        {
            if (!DamageTextManager.Instance) return;

            DamageText.Spawn(unit, damage, damageType);
        }
    }
}