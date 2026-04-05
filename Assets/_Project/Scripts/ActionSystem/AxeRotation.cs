using UnityEngine;

namespace ByNorth {
    public class AxeRotation : MonoBehaviour {
        public float rotationSpeed = 1;
        void Update()
        {
            transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0);
        }
    }
}
