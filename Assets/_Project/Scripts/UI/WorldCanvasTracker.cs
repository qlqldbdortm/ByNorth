using ByNorth.Unit.Behaviour.Structure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ByNorth.UI
{
    /// <summary>
    /// 상호작용한 오브젝트위에 아이콘을 띄우기 위한 스크립트
    /// </summary>
    public class WorldCanvasTracker : MonoBehaviour
    {
        public Transform Target { get; set; } = null;


        private Camera mainCamera = null;


        void Start()
        {
            mainCamera = Camera.main;
        }
        
        void Update()
        {
            transform.position = Target.position;
            transform.rotation = mainCamera.transform.rotation;
        }
    }
}
