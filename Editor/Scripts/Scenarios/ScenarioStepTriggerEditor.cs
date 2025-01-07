using _3Dimensions.Tools.Runtime.Scripts.Scenarios;
using UnityEditor;
using UnityEngine;

namespace _3Dimensions.Tools.Editor.Scripts.Scenarios
{
    [CustomEditor(typeof(ScenarioStepTrigger))]
    public class ScenarioStepTriggerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ScenarioStepTrigger stepTrigger = (ScenarioStepTrigger)target;

            if (GUILayout.Button("Trigger Step"))
            {
                stepTrigger.TriggerStep();
            }
        }
    }
}