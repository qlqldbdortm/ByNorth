using ByNorth.ActionSystem;
using ByNorth.ActionSystem.Triggered;
using ByNorth.LifeCycle;
using ByNorth.Unit.Modifier;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ByNorth
{
    public class ProjectileMover : MonoBehaviour, ISpawn<ActionExecutor>
    {
        public float moveSpeed = 10f;
        public Vector3 offset;
        private Vector3 direction;

        public void OnSpawn(ActionExecutor data)
        {
            direction = data.Caster.transform.forward;
            direction.y = 0f;
            direction.Normalize();

            transform.position += offset;
            transform.forward = direction;
        }


        void Update()
        {
            transform.Translate(moveSpeed * Time.deltaTime * Vector3.forward);
        }
    }
}
