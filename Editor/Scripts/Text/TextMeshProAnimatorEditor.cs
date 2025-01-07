using UnityEditor;
using UnityEngine;
using _3Dimensions.Tools.Runtime.Scripts.Text;

namespace _3Dimensions.Tools.Editor.Scripts.Text
{
    [CustomEditor(typeof(TextMeshProAnimator))]
    public class TextMeshProAnimatorEditor : UnityEditor.Editor
    {
        // Serialized properties for the fields
        private SerializedProperty textProp;
        private SerializedProperty useAnimateCompletionProp;
        private SerializedProperty textToDisplayProp;
        private SerializedProperty onEnableDelayProp;
        private SerializedProperty nextCharacterDelayProp;
        private SerializedProperty animatedCompletionProp;

        private void OnEnable()
        {
            // Find serialized properties
            textProp = serializedObject.FindProperty("text");
            useAnimateCompletionProp = serializedObject.FindProperty("useAnimateCompletion");
            textToDisplayProp = serializedObject.FindProperty("textToDisplay");
            onEnableDelayProp = serializedObject.FindProperty("onEnableDelay");
            nextCharacterDelayProp = serializedObject.FindProperty("nextCharacterDelay");
            animatedCompletionProp = serializedObject.FindProperty("animatedCompletion");
        }

        public override void OnInspectorGUI()
        {
            // Update the serialized object
            serializedObject.Update();

            // Draw the fields
            EditorGUILayout.PropertyField(textProp, new GUIContent("Text"));
            EditorGUILayout.PropertyField(useAnimateCompletionProp, new GUIContent("Use Animate Completion"));
            EditorGUILayout.PropertyField(textToDisplayProp, new GUIContent("Text to Display"));

            EditorGUILayout.Space();

            if (!useAnimateCompletionProp.boolValue)
            {
                // Timed Animation section
                EditorGUILayout.LabelField("Timed Animation", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(onEnableDelayProp, new GUIContent("On Enable Delay"));
                EditorGUILayout.PropertyField(nextCharacterDelayProp, new GUIContent("Next Character Delay"));
                EditorGUI.indentLevel--;
            }
            else
            {
                // Animated Completion section
                EditorGUILayout.LabelField("Animated Completion", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                EditorGUILayout.Slider(animatedCompletionProp, 0, 1, new GUIContent("Animated Completion"));

                // Display read-only runtime value
                TextMeshProAnimator animator = (TextMeshProAnimator)target;
                int characterCount = (int)(animator.textToDisplay.Length * animatedCompletionProp.floatValue);
                EditorGUILayout.LabelField("Character Count", characterCount.ToString());
                EditorGUI.indentLevel--;
            }

            // Apply modified properties
            serializedObject.ApplyModifiedProperties();
        }
    }
}