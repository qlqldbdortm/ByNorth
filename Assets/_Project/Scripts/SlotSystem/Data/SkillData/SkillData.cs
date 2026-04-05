using ByNorth.ActionSystem;
using ByNorth.SlotSystem.Data.ItemData;
using ByNorth.SlotSystem.Slot;
using ByNorth.Unit.Behaviour.Movable;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;



namespace ByNorth.SlotSystem.Data.SkillData {
    [CreateAssetMenu(menuName = "Scriptable Object/Skill/SkillData")]
    public class SkillData : SlotData, IUsable {
        [Header("스킬 타입(근접->Melee, 원거리->Ranged, 패시브->Passive)")]
        public SkillType skillType;
        [Header("스킬 이름")]
        public string skillName;
        [Header("스킬 설명")]
        public string skillDescription;
        [Header("스킬 쿨타임")]
        public float cooltime;
        [Header("스킬 코스트")]
        public int cost;
        [Header("스킬 대미지")]
        public int damage;
        [Header("액션 데이터(프리팹)")]
        public AttackData primaryAttackData; 

        public bool IsUsable { get; set; } = true;

        protected void Reset()
        {
            skillType = SkillType.None;
            skillName = "스킬 이름";
            skillDescription = "스킬 설명";
        }

        public virtual void Use()
        {
            if (!IsUsable) return;
            PlayerHandler.Player.UseAttackData(primaryAttackData);
      
        }
    }
}
