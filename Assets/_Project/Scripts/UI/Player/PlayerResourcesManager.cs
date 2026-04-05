using ByNorth.Unit.Behaviour.Movable;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace ByNorth.UI.Player
{
    public class PlayerResourcesManager: MonoBehaviour
    {
        [SerializeField] public TextSlider hpSlider;
        [SerializeField] public TextSlider staminaSlider;
        [SerializeField] public TextMeshProUGUI expText;


        public Unit.Unit Player => PlayerHandler.Player?.Unit;


        private int lastHp = -1;
        private float lastStamina = -1;

        private int maxHp = -1;
        private float maxStamina = -1;
        

        void Start()
        {
            _ = RefreshHpSliderAsync();
            _ = RefreshStaminaSliderAsync();
        }

        void Update()
        {
            expText.text = $"Exp: {Player.CurrentData.exp}";
        }


        private bool HasPlayer() => Player;
        
        private bool IsNotEqualHp() => Player && (Player.CurrentHp != lastHp || Player.CurrentData.hp != maxHp);
        private bool IsNotEqualStamina() => Player && (!Mathf.Approximately(Player.CurrentStamina, lastStamina) || !Mathf.Approximately(Player.CurrentData.stamina, maxStamina));


        private async UniTask RefreshHpSliderAsync()
        {
            if (hpSlider is null) return;
            
            await UniTask.WaitUntil(HasPlayer);
            hpSlider.maxValue = Player.CurrentData.hp;
            hpSlider.SetValue(Player.CurrentHp);
            
            while (true)
            {
                await UniTask.WaitUntil(IsNotEqualHp);
                
                lastHp = Player.CurrentHp;
                maxHp = Player.CurrentData.hp;
                hpSlider.maxValue = Player.CurrentData.hp;
                hpSlider.SetValueWithAnimation(Player.CurrentHp);
            }
        }
        private async UniTask RefreshStaminaSliderAsync()
        {
            if (staminaSlider is null) return;
            
            await UniTask.WaitUntil(HasPlayer);
            staminaSlider.maxValue = Player.CurrentData.stamina;
            staminaSlider.SetValue(Player.CurrentStamina);
            
            while (true)
            {
                await UniTask.WaitUntil(IsNotEqualStamina);
                
                lastStamina = Player.CurrentStamina;
                maxStamina = Player.CurrentData.stamina;
                staminaSlider.maxValue = Player.CurrentData.stamina;
                staminaSlider.SetValueWithAnimation(Player.CurrentStamina);
            }
        }
    }
}