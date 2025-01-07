using _3Dimensions.Tools.Runtime.Scripts.Scenarios;
using UnityEditor;
using UnityEngine;

namespace _3Dimensions.Tools.Editor.Scripts.Scenarios
{
    [CustomEditor(typeof(ScenarioController))]
    public class ScenarioControllerEditor : UnityEditor.Editor
    {
        // Reference to the target script
        private ScenarioController _controller;

        private void OnEnable()
        {
            _controller = (ScenarioController)target;
        }

        public override void OnInspectorGUI()
        {
            // Default inspector (display variables normally)
            DrawDefaultInspector();

            EditorGUILayout.Space(); // For spacing
            EditorGUILayout.LabelField("Debugging Info", EditorStyles.boldLabel);
            // Display CurrentStep in Inspector
            if (_controller.CurrentStep != null)
            {
                EditorGUILayout.LabelField("Current Step: ", _controller.CurrentStep.name);
            }
            else
            {
                EditorGUILayout.LabelField("Current Step: ", "None");
            }
            
            if (_controller.LastStep != null)
            {
                EditorGUILayout.LabelField("Last Step: ", _controller.LastStep.name);
            }
            else
            {
                EditorGUILayout.LabelField("Last Step: ", "None");
            }
            
            EditorGUILayout.Space(); // For spacing

            // Add buttons in a horizontal group for navigation
            EditorGUILayout.LabelField("Step Controls", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("|< First"))
            {
                _controller.SetFirstStep();
            }
            if (GUILayout.Button("< Previous"))
            {
                _controller.SetPreviousStep();
            }
            if (GUILayout.Button("Next >"))
            {
                _controller.SetNextStep();
            }
            if (GUILayout.Button("Last >|"))
            {
                _controller.SetLastStep();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(); // For spacing

            // Add buttons for scenario methods
            EditorGUILayout.LabelField("Scenario Management", EditorStyles.boldLabel);

            if (GUILayout.Button("Create Scenario Step"))
            {
                Undo.RecordObject(_controller, "Create Scenario Step");
                _controller.GetType().GetMethod("CreateScenarioStep", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.Invoke(_controller, null);
                EditorUtility.SetDirty(_controller);
            }

            if (GUILayout.Button("Activate All Steps"))
            {
                Undo.RecordObject(_controller, "Activate All Steps");
                _controller.GetType().GetMethod("ActivateAllSteps", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.Invoke(_controller, null);
                EditorUtility.SetDirty(_controller);
            }

            if (GUILayout.Button("Get Scenario Steps"))
            {
                Undo.RecordObject(_controller, "Get Scenario Steps");
                _controller.GetScenarioSteps();
                EditorUtility.SetDirty(_controller);
            }
        }
    }
}