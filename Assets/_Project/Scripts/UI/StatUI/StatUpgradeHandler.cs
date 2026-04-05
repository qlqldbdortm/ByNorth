using ByNorth.Unit.Behaviour.Movable;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ByNorth.UI.StatUI {
    public enum StatType {
        None = 0,
        Hp,
        Stamina,
        Strength,
    }

    public abstract class StatUpgradeHandler : MonoBehaviour {
        public Unit.Unit Player => PlayerHandler.Player.Unit;
      
        void Awake()
        {
            UpdateStatText(); 
        }

        /// <summary>
        /// 스탯을 올림
        /// </summary>
        public abstract void StatUp();

        /// <summary>
        /// 올라간 스탯의 정보를 Unit에 보냄
        /// </summary>
        public abstract void ApplyStat();

        /// <summary>
        /// 현재 캐릭터가 가지고 있는 경험치에서 소모 경험치량을 빼는 매소드
        /// </summary>
        /// <param name="consumptionExp">소모 경험치</param>
        public abstract void ConsumptionExp(float consumptionExp);

        /// <summary>
        /// 경험치 요구량 계산 (2 ^ level) * 100
        /// </summary>
        /// <param name="level">스탯을 몇 번 올렸나 알려주는 파라미터</param>
        /// <returns></returns>
        public abstract float ConsumptionExpReturn(int level);

        /// <summary>
        /// StatUI 텍스트 업데이트
        /// </summary>
        public abstract void UpdateStatText();

    }
}
