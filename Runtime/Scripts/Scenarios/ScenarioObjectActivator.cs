using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace _3Dimensions.Tools.Runtime.Scripts.Scenarios
{
    [ExecuteAlways]
    public class ScenarioObjectActivator : MonoBehaviour
    {
        public List<ScenarioStep> stepsWhereActivated;
        [SerializeField] private GameObject[] gameObjectsToActivate;
        
        public UnityEvent activationActions;
        private ScenarioObjectActivator[] _childActivators;
        
        private void Start()
        {
            ScenarioController.Instance.OnStartedEvent += ControllerOnStartedEvent;
            ScenarioController.Instance.OnStoppedEvent += ControllerOnStoppedEvent;
            
            foreach (GameObject go in gameObjectsToActivate)
            {
                go.SetActive(false);
            }

            ControllerOnStepChangeEvent(ScenarioController.Instance.CurrentStep);
        }

        private void OnDestroy()
        {
            if (ScenarioController.Instance == null) return;
            ScenarioController.Instance.OnStartedEvent -= ControllerOnStartedEvent;
            ScenarioController.Instance.OnStoppedEvent -= ControllerOnStoppedEvent;
        }
        
        private void ControllerOnStartedEvent()
        {
            foreach (GameObject go in gameObjectsToActivate)
            {
                go.SetActive(false);
            }
        }
        
        private void ControllerOnStoppedEvent()
        {
            foreach (GameObject go in gameObjectsToActivate)
            {
                go.SetActive(false);
            }
        }

        public void ControllerOnStepChangeEvent(ScenarioStep step)
        {
            bool contains = stepsWhereActivated.Contains(step);
            foreach (GameObject go in gameObjectsToActivate)
            {
                go.SetActive(contains);
            }
            
            _childActivators = GetComponentsInChildren<ScenarioObjectActivator>(true);

            foreach (ScenarioObjectActivator activator in _childActivators)
            {
                if (activator != this)
                {
                    activator.ControllerOnStepChangeEvent(step);
                }
            }

            if (contains) activationActions.Invoke();
        }

        public void ForceOn()
        {
            foreach (GameObject go in gameObjectsToActivate)
            {
                go.SetActive(true);
            }
            
            foreach (ScenarioObjectActivator activator in _childActivators)
            {
                if (activator != this)
                {
                    activator.ForceOn();
                }
            }
        }
    }
}
