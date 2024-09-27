using UnityEngine;
namespace _3Dimensions.Tools.Runtime.Scripts.Utilities
{
    public class RotateObjectByAxis : MonoBehaviour
    {
        [SerializeField] private Vector3 axis = Vector3.up;
        [SerializeField] private float speed = 20f;

        public bool active = true;
        public Space space = Space.Self;

        private void Update()
        {
            if (active) transform.Rotate(axis * (speed * Time.deltaTime), space);
        }
    }
}
