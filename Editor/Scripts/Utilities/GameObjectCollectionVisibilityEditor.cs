using _3Dimensions.Tools.Runtime.Scripts.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace _3Dimensions.Tools.Editor.Scripts.Utilities
{
    [CustomEditor(typeof(GameObjectCollectionVisibility))]
    public class GameObjectCollectionVisibilityEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // Reference the target script
            var visibilityScript = (GameObjectCollectionVisibility)target;
            
            // Draw the default inspector for other fields
            DrawDefaultInspector();
            
            // Add button
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Controls", EditorStyles.boldLabel);
            
            if (GUILayout.Button("Collect GameObjects"))
            {
                visibilityScript.CollectChildren();
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