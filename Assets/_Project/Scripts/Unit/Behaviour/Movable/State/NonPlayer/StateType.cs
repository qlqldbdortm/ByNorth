namespace ByNorth.Unit.Behaviour.Movable.State.NonPlayer
{
    public enum StateType
    {
        /// <summary>
        /// 일상 상태
        /// </summary>
        Idle,
        
        /// <summary>
        /// 단거리 도주<br/>
        /// 플레이어가 보이지 않는 정도까지 도주
        /// </summary>
        Runaway,
        /// <summary>
        /// 피난 장소까지 도주
        /// </summary>
        Evacuate,
        /// <summary>
        /// 공황 상태<br/>
        /// 플레이어가 공격을 해도 아무것도 하지 않음
        /// </summary>
        Scare,
        
        /// <summary>
        /// 순찰 경로를 따라 순찰
        /// </summary>
        Patrol,
        /// <summary>
        /// 플레이어의 흔적을 추적
        /// </summary>
        Track,
        /// <summary>
        /// 감지 거리 내에 존재하는 플레이어를 추적
        /// </summary>
        Chase,
        /// <summary>
        /// 플레이어와의 거리가 사거리 내인 상태에서 전투
        /// </summary>
        Fight,
        
        /// <summary>
        /// 스턴 상태
        /// </summary>
        Stun,
    }
}