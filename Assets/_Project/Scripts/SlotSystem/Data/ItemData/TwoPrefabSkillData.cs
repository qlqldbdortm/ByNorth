using ByNorth.ActionSystem;
using ByNorth.SlotSystem.Data.ItemData;
using ByNorth.SlotSystem.Data.SkillData;
using ByNorth.Unit.Behaviour.Movable;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ByNorth.SlotSystem.Data.ItemData {
    [CreateAssetMenu(fileName = "New TwoPrefabSkillData", menuName = "Scriptable Object/Skill/TwoPrefabSkillData")]
    public class TwoPrefabSkillData : SkillData.SkillData
    {
        public AttackData secondaryAttackData;
        
        [Header("두 공격 사이 딜레이")]
        public float delayBetweenAttack = 0.1f;

        public override void Use()
        {
            if (!IsUsable) return;
            PlayerHandler.Player.UseAttackData(primaryAttackData);
            _ = Wave();
        }
        
        public async UniTask Wave()
        {
            await UniTask.WaitForSeconds(delayBetweenAttack);
            ActionExecutor.Spawn(secondaryAttackData.actionData,secondaryAttackData.damage, PlayerHandler.Player.Unit);
        }
    }
}
