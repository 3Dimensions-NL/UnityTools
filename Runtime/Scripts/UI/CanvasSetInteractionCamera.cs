using UnityEngine;
namespace _3Dimensions.Tools.Runtime.Scripts.UI
{
    public class CanvasSetInteractionCamera : MonoBehaviour
    {
        private Canvas _canvas;
        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }
        void Update()
        {
            if (_canvas.worldCamera != Camera.main)
            {
                _canvas.worldCamera = Camera.main;
            }
        }
    }
}
