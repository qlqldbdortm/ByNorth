using System.Collections;
using System.Collections.Generic;
using ByNorth.Core.GameFlow;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ByNorth
{
    public class KarmaSight : MonoBehaviour
    {
        [SerializeField] private Volume volume;
        [SerializeField] private Vignette vignette;
        
        [Header("카르마 1당 오르는 수치")]
        public float valuePerStack = 0.05f;
 
        void Awake()
        {
            if (volume.profile.TryGet<Vignette>(out vignette))
            {
                vignette.active = true;
            }
        }
        
        public void UpdateVignette(int karma)
        {
            vignette.intensity.value = Mathf.Clamp01(karma * valuePerStack);
        }
    }
}
