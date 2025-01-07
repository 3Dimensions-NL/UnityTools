using System.Collections.Generic;
using UnityEngine;
namespace _3Dimensions.Tools.Runtime.Scripts.Scenarios
{
    [ExecuteAlways]
    public class ScenarioComponentActivator : MonoBehaviour
    {
        public List<ScenarioStep> stepsWhereActivated = new();
        public List<Behaviour> componentsToActivate = new();

        private void OnEnable()
        {
            //Check current step
            ControllerOnStepChangeEvent(ScenarioController.Instance.CurrentStep);
        }

        public void ControllerOnStepChangeEvent(ScenarioStep newStep)
        {
            bool isEnabled = stepsWhereActivated.Contains(newStep);
            
            foreach (Behaviour component in componentsToActivate)
            {
                component.enabled = isEnabled;
            }
        }
    }
}