using UnityEngine;

namespace ByNorth.ActionSystem {
    [CreateAssetMenu(menuName = "Action/Action Data", fileName = "New Action Data")]
    public class ActionData : ScriptableObject {
        [Header("프리팹"), Tooltip("프리팹")]
        public ActionExecutor prefab;
        [Header("효과 배율"), Tooltip("효과 배율")]
        public float strengthMultiplier = 1f;
        [Header("삭제까지 타이머"), Tooltip("삭제까지 타이머")]
        public float lifetime = 5f;
    }
}