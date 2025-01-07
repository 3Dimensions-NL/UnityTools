using _3Dimensions.Tools.Runtime.Scripts.Utilities;
using UnityEditor;
using UnityEngine;

namespace _3Dimensions.Tools.Editor.Scripts.Utilities
{
    [CustomEditor(typeof(GameObjectSteps))]
    public class GameObjectStepsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // Reference the target script
            var stepsScript = (GameObjectSteps)target;

            // Draw the default inspector for other fields
            DrawDefaultInspector();

            // Add a horizontal layout group for controls
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Controls", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();

            // Add buttons for First, Previous, Next, and Last
            if (GUILayout.Button("|<"))
            {
                stepsScript.First();
                MarkSceneDirty();
            }

            if (GUILayout.Button("<"))
            {
                stepsScript.Previous();
                MarkSceneDirty();
            }

            if (GUILayout.Button(">"))
            {
                stepsScript.Next();
                MarkSceneDirty();
            }

            if (GUILayout.Button(">|"))
            {
                stepsScript.Last();
                MarkSceneDirty();
            }

            EditorGUILayout.EndHorizontal();

            // Add a button for CollectSteps
            EditorGUILayout.Space();
            if (GUILayout.Button("Collect Steps"))
            {
                stepsScript.CollectSteps();
                MarkSceneDirty();
            }
        }

        // Marks the scene as dirty so changes are saved
        private void MarkSceneDirty()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                EditorUtility.SetDirty(target);
            }
#endif
        }
    }
}