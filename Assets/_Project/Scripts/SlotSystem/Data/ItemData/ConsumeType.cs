using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ByNorth.SlotSystem.Data.ItemData
{
    /// <summary>
    /// 소비 아이템 사용시 적용되는 효과
    /// </summary>
    public enum ConsumeType {
        None,
        /// <summary>
        /// HP 회복
        /// </summary>
        Hp,
        /// <summary>
        /// 스태미나 회복
        /// </summary>
        Stamina,
        /// <summary>
        /// 버프 적용
        /// </summary>
        Modifier,
    }
}
