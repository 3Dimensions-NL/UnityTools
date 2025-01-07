using _3Dimensions.Tools.Runtime.Scripts.Scenarios;
using UnityEditor;
using UnityEngine;

namespace _3Dimensions.Tools.Editor.Scripts.Scenarios
{
    [CustomEditor(typeof(ScenarioStep))]
    public class ScenarioStepEditor : UnityEditor.Editor
    {
        private ScenarioStep _step;
        private ScenarioController _controller;

        private void OnEnable()
        {
            _step = (ScenarioStep)target;
            _controller = _step.GetComponentInParent<ScenarioController>();
        }

        public override void OnInspectorGUI()
        {
            // Default inspector fields
            DrawDefaultInspector();

            EditorGUILayout.Space(); // Add spacing
            EditorGUILayout.LabelField("Step Controls", EditorStyles.boldLabel);

            // Display the step controls buttons in a horizontal layout
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("|< First"))
            {
                CallPrivateMethod(_step, "First");
            }

            if (GUILayout.Button("< Previous"))
            {
                CallPrivateMethod(_step, "PreviousStep");
            }

            if (GUILayout.Button("Next >"))
            {
                CallPrivateMethod(_step, "NextStep");
            }

            if (GUILayout.Button("Last >|"))
            {
                CallPrivateMethod(_step, "LastStep");
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(); // Add spacing

            // Add a button to set this step as the current step
            if (GUILayout.Button("Set As Current Step"))
            {
                CallPrivateMethod(_step, "SetStepCurrent");
            }
        }

        /// <summary>
        /// Calls a private method on the target object using reflection.
        /// </summary>
        /// <param name="target">The object on which the method is called.</param>
        /// <param name="methodName">The name of the method to call.</param>
        private void CallPrivateMethod(Object target, string methodName)
        {
            var method = target.GetType().GetMethod(methodName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (method != null)
            {
                Undo.RecordObject(target, $"Invoke {methodName} on {target.name}");
                method.Invoke(target, null);
                EditorUtility.SetDirty(target);
            }
            else
            {
                Debug.LogError($"Method '{methodName}' not found on {target.GetType().Name}");
            }
        }
    }
}