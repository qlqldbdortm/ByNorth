using ByNorth.ActionSystem;
using ByNorth.Unit.Behaviour.Movable;
using System;
using UnityEngine;

namespace ByNorth.SlotSystem.Data.ItemData {
    [Serializable]
    public class AttackData
    {
        public ActionData actionData;
        public float strengthMultiplier = 1f; //TODO : 나중에 어떻게 할지 생각
        public float damage;
        public int stamina;
        public AnimationType animationType;
        public AudioClip attackAudio;
        
        public void Attack() 
        {
            //TODO : Unit 받기
            //ActionExecutor.Spawn(actionData, damage, unit);
        }
    }
}