namespace ByNorth.Unit {
    public enum EntityType {
        None,
        /// <summary>
        /// 유저가 조종하는 캐릭터
        /// </summary>
        Player,
        /// <summary>
        /// 유저가 조종하지 않는 보스를 포함한 다른 캐릭터들
        /// </summary>
        Npc,
        /// <summary>
        /// 맵에 설치되어 있는 오브젝트들
        /// </summary>
        Object,
    }
}
