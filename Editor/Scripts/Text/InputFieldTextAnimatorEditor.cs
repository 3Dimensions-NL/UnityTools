using _3Dimensions.Tools.Runtime.Scripts.Text;
using UnityEditor;
using UnityEngine;

namespace _3Dimensions.Tools.Editor.Scripts.Text
{
    // Custom Editor for InputFieldTextAnimator
    [CustomEditor(typeof(InputFieldTextAnimator))]
    public class InputFieldTextAnimatorEditor : UnityEditor.Editor
    {
        private SerializedProperty inputFieldProp;
        private SerializedProperty useAnimateCompletionProp;
        private SerializedProperty textToDisplayProp;
        private SerializedProperty onEnableDelayProp;
        private SerializedProperty nextCharacterDelayProp;
        private SerializedProperty animatedCompletionProp;

        private void OnEnable()
        {
            // Cache references to serialized properties
            inputFieldProp = serializedObject.FindProperty("inputField");
            useAnimateCompletionProp = serializedObject.FindProperty("useAnimateCompletion");
            textToDisplayProp = serializedObject.FindProperty("textToDisplay");
            onEnableDelayProp = serializedObject.FindProperty("onEnableDelay");
            nextCharacterDelayProp = serializedObject.FindProperty("nextCharacterDelay");
            animatedCompletionProp = serializedObject.FindProperty("animatedCompletion");
        }

        public override void OnInspectorGUI()
        {
            // Update serialized object state
            serializedObject.Update();

            // Draw the fields using SerializedProperty
            EditorGUILayout.PropertyField(inputFieldProp, new GUIContent("Input Field"));
            EditorGUILayout.PropertyField(useAnimateCompletionProp, new GUIContent("Use Animate Completion"));

            // Text to Display field
            EditorGUILayout.PropertyField(textToDisplayProp, new GUIContent("Text To Display"));

            EditorGUILayout.Space();

            if (!useAnimateCompletionProp.boolValue)
            {
                // Group: Timed Animation Fields
                EditorGUILayout.LabelField("Timed Animation", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(onEnableDelayProp, new GUIContent("On Enable Delay"));
                EditorGUILayout.PropertyField(nextCharacterDelayProp, new GUIContent("Next Character Delay"));
                EditorGUI.indentLevel--;
            }
            else
            {
                // Group: Animated Completion Fields
                EditorGUILayout.LabelField("Animated Completion", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                EditorGUILayout.Slider(animatedCompletionProp, 0, 1, new GUIContent("Animated Completion"));

                // Display Character Count read-only in Inspector
                InputFieldTextAnimator animator = (InputFieldTextAnimator)target;
                int characterCount = (int)(animator.textToDisplay.Length * animatedCompletionProp.floatValue);
                EditorGUILayout.LabelField("Character Count", characterCount.ToString());
                EditorGUI.indentLevel--;
            }

            // Apply changes to serialized object
            serializedObject.ApplyModifiedProperties();
        }
    }
}