using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByNorth.Unit.Behaviour.Movable
{
    public enum AnimationType
    {
        None = 0,

        /// <summary>
        /// 오른쪽에서 휘두르기
        /// </summary>
        RightSlash = 10,
        /// <summary>
        /// 오른쪽에서 세게 휘두르기
        /// </summary>
        RightHeavySlash = 11,
        /// <summary>
        /// 위에서 아래로 휘두르기
        /// </summary>
        OverheadSlash = 20,
        /// <summary>
        /// 위에서 아래로 세게 휘두르기
        /// </summary>
        OverheadHeavySlash = 21,

        /// <summary>
        /// 보스의 도끼 던지는 모션
        /// </summary>
        LeftOneHandSlash = 30,
        RightOneHandSlash = 31,
        
        /// <summary>
        /// 보스의 슈퍼점프 모션
        /// </summary>
        Jump = 40,
        JumpAttack = 41,
    }
}
