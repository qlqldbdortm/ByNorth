using ByNorth.Core;
using EPOOutline;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ByNorth.Unit.Behaviour.Structure
{
    public abstract class StructureHandler : MonoBehaviour
    {
        public Transform iconPos; // 오브젝트에 나타날 아이콘 위치
        
        private Outlinable outlinable;

        // 상호작용을 했었는지, 상호작용 중인지 확인하는 상태 
        protected bool hasInteracted = false;
        protected bool hasInteracting = false;

        protected virtual void Start()
        {
            outlinable = GetComponent<Outlinable>();
        }
        void Update()
        {
            if (hasInteracted && hasInteracting) // 상호작용 중일 때
            {
                outlinable.OutlineParameters.Color = Color.green;
            }
            else if (hasInteracted) // 상호작용을 했었을 때
            {
                outlinable.OutlineParameters.Color = Color.yellow;
            }
        }
    }
}
