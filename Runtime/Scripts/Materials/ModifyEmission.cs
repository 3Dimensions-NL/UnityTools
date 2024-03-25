using Sirenix.OdinInspector;
using UnityEngine;
namespace _3Dimensions.Tools.Runtime.Scripts.Materials
{
    [ExecuteAlways]
    public class ModifyEmission : MonoBehaviour
    {
        public Material sourceMaterial;
        [ColorUsage(false, true)] public Color emissionColor;

        private Renderer _renderer;
        private Material _material;
        
        void Update()
        {
            if (_renderer == null)
            {
                _renderer = GetComponent<MeshRenderer>();
            }
            
            if (_material == null)
            {
                ResetMaterial();
            }

            _renderer.material = _material;
            _material.SetColor("_EmissionColor", emissionColor);
        }

        [Button]
        private void ResetMaterial()
        {
            _material = sourceMaterial;
        }
    }
}
