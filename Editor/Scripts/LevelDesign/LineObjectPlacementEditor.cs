using _3Dimensions.Tools.Runtime.Scripts.LevelDesign;
using UnityEditor;
using UnityEngine;

namespace _3Dimensions.Tools.Editor.Scripts.LevelDesign
{
    [CustomEditor(typeof(LineObjectPlacement))]
    public class LineObjectPlacementEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // Teken de standaard Inspector-velden
            DrawDefaultInspector();

            // Referentie naar het doelobject
            LineObjectPlacement targetScript = (LineObjectPlacement)target;

            // Visuele scheiding
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Custom Actions", EditorStyles.boldLabel);

            // Knop om "AddNewPoint" methode aan te roepen
            if (GUILayout.Button("Add New Point"))
            {
                // Methode aanroepen
                targetScript.AddNewPoint();

                // Update de Scene voor visuele veranderingen
                EditorApplication.QueuePlayerLoopUpdate();
            }

            // Knop om "PlacePrefabs" methode aan te roepen
            if (GUILayout.Button("Place Prefabs"))
            {
                // Methode aanroepen
                typeof(LineObjectPlacement)
                    .GetMethod("PlacePrefabs",
                        System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                    ?.Invoke(targetScript, null);

                // Update de Scene voor visuele veranderingen
                EditorApplication.QueuePlayerLoopUpdate();
            }

            // Knop om "RemovePrefabs" methode aan te roepen
            if (GUILayout.Button("Remove Prefabs"))
            {
                // Methode aanroepen
                typeof(LineObjectPlacement)
                    .GetMethod("RemovePrefabs",
                        System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                    ?.Invoke(targetScript, null);

                // Update de Scene voor visuele veranderingen
                EditorApplication.QueuePlayerLoopUpdate();
            }
        }
    }
}