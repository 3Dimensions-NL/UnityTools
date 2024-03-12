using System.Collections.Generic;
using UnityEngine;
namespace _3Dimensions.Tools.Runtime.Scripts.Scenarios
{
    public class ScenarioStepMultiTrigger : MonoBehaviour
    {
        [SerializeField] private ScenarioStep stepToTrigger;
        [SerializeField] private List<ScenarioStep> stepsWhereItCanBeTriggered = new List<ScenarioStep>();

        [SerializeField] private List<GameObject> triggeringGameObjects = new List<GameObject>();
        private List<GameObject> _triggeredGameObjects = new List<GameObject>();
        
        public void TriggerStep(GameObject triggeringGameObject)
        {
            if (stepsWhereItCanBeTriggered.Contains(ScenarioController.Instance.CurrentStep))
            {
                if (!_triggeredGameObjects.Contains(triggeringGameObject)) _triggeredGameObjects.Add(triggeringGameObject);

                if (triggeringGameObjects.Count == _triggeredGameObjects.Count)
                {
                    stepToTrigger.MakeStepCurrent();
                }
            }
        }
    }
}