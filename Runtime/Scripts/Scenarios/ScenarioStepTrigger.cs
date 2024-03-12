using System.Collections.Generic;
using UnityEngine;
namespace _3Dimensions.Tools.Runtime.Scripts.Scenarios
{
    public class ScenarioStepTrigger : MonoBehaviour
    {
        [SerializeField] private ScenarioStep stepToTrigger;
        [SerializeField] private List<ScenarioStep> stepsWhereItCanBeTriggered = new List<ScenarioStep>();

        public void TriggerStep()
        {
            if (stepsWhereItCanBeTriggered.Contains(ScenarioController.Instance.CurrentStep))
            {
                if (ScenarioController.Instance.CurrentStep != stepToTrigger) stepToTrigger.MakeStepCurrent();
            }
        }
    }
}