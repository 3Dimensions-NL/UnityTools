using UnityEngine;

namespace _3Dimensions.Tools.Runtime.Scripts.Materials
{
    [ExecuteAlways]
    public class ModifyEmission : MonoBehaviour
    {
        public Material sourceMaterial;

        // Het gebruik van [ColorUsage] is standaard in Unity, dus dit kan je laten staan.
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

        // Maak de ResetMaterial-functie publiek zodat het vanuit de editor toegankelijk is.
        public void ResetMaterial()
        {
            _material = sourceMaterial;
        }
    }
}