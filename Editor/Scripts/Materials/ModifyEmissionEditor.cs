using _3Dimensions.Tools.Runtime.Scripts.Materials;
using UnityEditor;
using UnityEngine;

namespace _3Dimensions.Tools.Editor.Scripts.Materials
{
    [CustomEditor(typeof(ModifyEmission))]
    public class ModifyEmissionEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // Toon de standaard eigenschappen
            DrawDefaultInspector();

            // Referentie naar het target-script
            var modifyEmission = (ModifyEmission)target;

            // Voeg een knop toe in de Inspector
            if (GUILayout.Button("Reset Material"))
            {
                modifyEmission.ResetMaterial();
            }
        }
    }
}