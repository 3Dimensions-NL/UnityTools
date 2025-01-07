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
            if (Application.isPlaying) ScenarioController.Instance.SetScenarioStep(this);
            else ScenarioController.Instance.SetScenarioStepInEditor(this);

            Debug.Log("Step " + name + " is now current.", this);
        }

#if UNITY_EDITOR
        // Replaces [Button] SetStepCurrent
        [ContextMenu("Set Step Current")]
        private void SetStepCurrent()
        {
            ScenarioController controller = GetComponentInParent<ScenarioController>();
            if (Application.isPlaying) controller.SetScenarioStepFromClient(GetStepIndex());
            else controller.SetScenarioStepInEditor(this);
        }

        // Replaces [Button] Navigation Buttons with ContextMenu options
        [ContextMenu("Go To First Step")]
        private void First()
        {
            ScenarioController controller = GetComponentInParent<ScenarioController>();
            SetStepCurrent();
            controller.SetFirstStep();
            SelectActiveStep();
        }

        [ContextMenu("Go To Previous Step")]
        private void PreviousStep()
        {
            ScenarioController controller = GetComponentInParent<ScenarioController>();
            SetStepCurrent();
            controller.SetPreviousStep();
            SelectActiveStep();
        }

        [ContextMenu("Go To Next Step")]
        private void NextStep()
        {
            ScenarioController controller = GetComponentInParent<ScenarioController>();
            SetStepCurrent();
            controller.SetNextStep();
            SelectActiveStep();
        }

        [ContextMenu("Go To Last Step")]
        private void LastStep()
        {
            ScenarioController controller = GetComponentInParent<ScenarioController>();
            SetStepCurrent();
            controller.SetLastStep();
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