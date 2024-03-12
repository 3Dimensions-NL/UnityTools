using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
namespace _3Dimensions.Tools.Runtime.Scripts.Scenarios
{
    public class ScenarioStep : MonoBehaviour
    {
        public UnityEvent onStepStarted;
        public UnityEvent onStepStopped;
        
        public void MakeStepCurrent()
        {
            if (ScenarioController.Instance.CurrentStep == this) return;
            ScenarioController.Instance.SetScenarioStep(this);
        }

#if UNITY_EDITOR
        [Button] 
        private void SetStepCurrent()
        {
            ScenarioController controller = GetComponentInParent<ScenarioController>();
            if (Application.isPlaying) controller.SetScenarioStepFromClient(GetStepIndex());
            else controller.SetScenarioStepInEditor(this);
        }

        [HorizontalGroup("Steps")]
        [Button("|<")]
        private void First()
        {
            ScenarioController controller = GetComponentInParent<ScenarioController>();
            SetStepCurrent();
            controller.FirstStep();
            SelectActiveStep();
        }
        
        [HorizontalGroup("Steps")]
        [Button("<")]
        private void PreviousStep()
        {
            ScenarioController controller = GetComponentInParent<ScenarioController>();
            SetStepCurrent();
            controller.PreviousStep();
            SelectActiveStep();
        }

        [HorizontalGroup("Steps")]
        [Button(">")]
        private void NextStep()
        {
            ScenarioController controller = GetComponentInParent<ScenarioController>();
            SetStepCurrent();
            controller.NextStep();
            SelectActiveStep();
        }
        
        [HorizontalGroup("Steps")]
        [Button(">|")]
        private void LastStep()
        {
            ScenarioController controller = GetComponentInParent<ScenarioController>();
            SetStepCurrent();
            controller.LastStep();
            SelectActiveStep();
        }

        
        private int GetStepIndex()
        {
            ScenarioController controller = GetComponentInParent<ScenarioController>();
            for (int i = 0; i < controller.scenarioSteps.Length; i++)
            {
                if (controller.scenarioSteps[i] == this)
                {
                    return i;
                } 
            }
            return 0;
        }

        private void SelectActiveStep()
        {
            ScenarioController controller = GetComponentInParent<ScenarioController>();
            UnityEditor.Selection.activeGameObject = controller.CurrentStep.gameObject;
        }
#endif
    }
}
