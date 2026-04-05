using System.Collections.Generic;
using UnityEngine;

namespace ByNorth.Unit.Modifier
{
    public abstract class ModifierBase: ScriptableObject
    {
        [Header("상태이상 기본 정보")]
        [Tooltip("상태이상 명칭")] public string codeName;
        [Tooltip("상태이상 아이콘")] public Sprite icon;
         
        [Tooltip("상태이상 지속시간")] public float time = 5;
        
        
        /// <summary>
        /// 상태이상 적용
        /// </summary>
        /// <param name="unit"></param>
        public abstract void Modify(Unit unit);
        /// <summary>
        /// 상태이상 해제
        /// </summary>
        /// <param name="unit"></param>
        public abstract void Undo(Unit unit);
    }
}