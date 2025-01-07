using UnityEditor;
using UnityEngine;

namespace _3Dimensions.Tools.Editor.Scripts.Scenarios
{
    [CustomEditor(typeof(_3Dimensions.Tools.Runtime.Scripts.Scenarios.ScenarioStepCorrectEffect))]
    public class ScenarioStepCorrectEffectEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // Draw the default Inspector
            DrawDefaultInspector();

            // Get a reference to the target object (ScenarioStepCorrectEffect)
            _3Dimensions.Tools.Runtime.Scripts.Scenarios.ScenarioStepCorrectEffect scenarioStep = 
                (_3Dimensions.Tools.Runtime.Scripts.Scenarios.ScenarioStepCorrectEffect)target;

            // Add spacing
            GUILayout.Space(10);

            // Create buttons in the Inspector
            if (GUILayout.Button("Start Effect"))
            {
                scenarioStep.StartEffect();
            }

            if (GUILayout.Button("Stop Effect"))
            {
                scenarioStep.StopEffect();
            }
        }
    }
}