using System.Collections.Generic;
using UnityEngine;

namespace _3Dimensions.Tools.Runtime.Scripts.Scenarios
{
    public class ScenarioStepTrigger : MonoBehaviour
    {
        [SerializeField] private ScenarioStep stepToTrigger;

        [SerializeField,
         Tooltip(
             "Add steps to activate this trigger only when one is active, or leave empty to always activate.")]
        private List<ScenarioStep> stepsWhereItCanBeTriggered = new();

        [ContextMenu("Trigger Step")]
        public void TriggerStep()
        {
            if (stepsWhereItCanBeTriggered.Count == 0)
            {
                stepToTrigger.MakeStepCurrent(); // If no steps are specified, always trigger (default)
                return;
            }

            if (stepsWhereItCanBeTriggered.Contains(ScenarioController.Instance.CurrentStep))
            {
                if (ScenarioController.Instance.CurrentStep != stepToTrigger) stepToTrigger.MakeStepCurrent();
            }
        }
    }
}