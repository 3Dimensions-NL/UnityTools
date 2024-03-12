using System.Collections.Generic;
using UnityEngine;
namespace _3Dimensions.Tools.Runtime.Scripts.Scenarios
{
    public class ScenarioComponentActivator : MonoBehaviour
    {
        public List<ScenarioStep> stepsWhereActivated = new List<ScenarioStep>();
        public List<Behaviour> componentsToActivate = new List<Behaviour>();

        private void OnEnable()
        {
            ScenarioController.Instance.OnStepChangedEvent += InstanceOnOnStepChangedEvent;
            
            //Check current step
            InstanceOnOnStepChangedEvent(ScenarioController.Instance.CurrentStep);
        }
        
        private void OnDisable()
        {
            if (ScenarioController.Instance) ScenarioController.Instance.OnStepChangedEvent -= InstanceOnOnStepChangedEvent;
        }
        
        private void InstanceOnOnStepChangedEvent(ScenarioStep newStep)
        {
            bool isEnabled = stepsWhereActivated.Contains(newStep);
            
            foreach (Behaviour component in componentsToActivate)
            {
                component.enabled = isEnabled;
            }
        }
    }
}