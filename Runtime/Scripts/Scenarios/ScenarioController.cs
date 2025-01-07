using UnityEngine;
using UnityEngine.Events;

namespace _3Dimensions.Tools.Runtime.Scripts.Scenarios
{
    public class ScenarioController : MonoBehaviour
    {
        public static ScenarioController Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<ScenarioController>(true);
                return _instance;
            }
        }

        private static ScenarioController _instance;

        public ScenarioStep[] scenarioSteps;
        private ScenarioObjectActivator[] _scenarioObjectActivators;
        private ScenarioComponentActivator[] _scenarioComponentActivators;

        public ScenarioStep CurrentStep
        {
            get
            {
                if (scenarioSteps.Length == 0)
                {
                    scenarioSteps = GetComponentsInChildren<ScenarioStep>();

                    if (scenarioSteps.Length == 0)
                    {
                        CreateScenarioStep();
                    }
                }

                return scenarioSteps[_activeStepIndex];
            }
        }
        
        public ScenarioStep LastStep
        {
            get
            {
                if (scenarioSteps.Length == 0)
                {
                    scenarioSteps = GetComponentsInChildren<ScenarioStep>();

                    if (scenarioSteps.Length == 0)
                    {
                        CreateScenarioStep();
                    }
                }

                return scenarioSteps[_lastStepIndex];
            }
        }

        private int _activeStepIndex;
        private int _lastStepIndex;

        public delegate void OnStarted();

        public event OnStarted OnStartedEvent;

        public delegate void OnStopped();

        public event OnStopped OnStoppedEvent;

        public delegate void OnStepChanged(ScenarioStep newStep);

        public event OnStepChanged OnStepChangedEvent;

        private void Awake()
        {
            GetScenarioSteps();
            GetScenarioObjectActivators();
            GetScenarioComponentActivators();

            foreach (ScenarioStep step in scenarioSteps)
            {
                step.gameObject.SetActive(false);
            }
        }

        public void Start()
        {
            _activeStepIndex = 0;
            _lastStepIndex = 0;
            ActivateCurrentStep();
        }

        // Standard method to replace [Button]
#if UNITY_EDITOR
        [ContextMenu("Create Scenario Step")]
#endif
        private void CreateScenarioStep()
        {
            // Get the current number of steps to determine the appropriate index
            int stepIndex = scenarioSteps.Length;

            // Create a new GameObject for the step with a default name including its index
            GameObject newStep = new GameObject($"New Step ({stepIndex})");

            // Parent the new GameObject to this controller's transform
            newStep.transform.SetParent(transform);

            // Add the ScenarioStep component to the new GameObject
            newStep.AddComponent<ScenarioStep>();

            // Refresh the scenarioSteps array
            scenarioSteps = GetComponentsInChildren<ScenarioStep>();
        }

#if UNITY_EDITOR
        [ContextMenu("Activate All Steps")]
#endif
        private void ActivateAllSteps()
        {
            GetScenarioSteps();
            GetScenarioObjectActivators();
            GetScenarioComponentActivators();

            foreach (ScenarioStep step in scenarioSteps)
            {
                step.gameObject.SetActive(true);
                step.onStepStarted?.Invoke();
            }

            foreach (ScenarioObjectActivator objectActivator in _scenarioObjectActivators)
            {
                objectActivator.ForceOn();
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Get Scenario Steps")]
#endif
        public void GetScenarioSteps()
        {
            scenarioSteps = GetComponentsInChildren<ScenarioStep>(true);
        }

        public void GetScenarioObjectActivators()
        {
            _scenarioObjectActivators = FindObjectsOfType<ScenarioObjectActivator>(true);
        }
        
        public void GetScenarioComponentActivators()
        {
            _scenarioComponentActivators = FindObjectsOfType<ScenarioComponentActivator>(true);
        }

        private void EditorFirstStep()
        {
            if (!Application.isPlaying) GetScenarioSteps();
            if (!Application.isPlaying) GetScenarioObjectActivators();
            if (!Application.isPlaying) GetScenarioComponentActivators();

            OnStartedEvent?.Invoke();
            
            if (Application.isPlaying) SetScenarioStepFromClient(0);
            else SetScenarioStepInEditor(scenarioSteps[0]);
        }

        private void EditorPreviousStep()
        {
            if (!Application.isPlaying) GetScenarioSteps();
            if (!Application.isPlaying) GetScenarioObjectActivators();
            if (!Application.isPlaying) GetScenarioComponentActivators();


            if (_activeStepIndex <= 0)
            {
                OnStartedEvent?.Invoke();
                if (Application.isPlaying) SetScenarioStepFromClient(0);
                else SetScenarioStepInEditor(scenarioSteps[0]);
                return;
            }

            if (Application.isPlaying) SetScenarioStepFromClient(_activeStepIndex - 1);
            else SetScenarioStepInEditor(scenarioSteps[_activeStepIndex - 1]);
        }

        private void EditorNextStep()
        {
            if (!Application.isPlaying) GetScenarioSteps();
            if (!Application.isPlaying) GetScenarioObjectActivators();
            if (!Application.isPlaying) GetScenarioComponentActivators();

            for (int i = 0; i < scenarioSteps.Length; i++)
            {
                if (CurrentStep == scenarioSteps[i])
                {
                    if (i >= scenarioSteps.Length - 1)
                    {
                        OnStoppedEvent?.Invoke();
                        return;
                    }

                    if (Application.isPlaying) SetScenarioStepFromClient(i + 1);
                    else SetScenarioStepInEditor(scenarioSteps[i + 1]);
                    return;
                }
            }
        }

        private void EditorLastStep()
        {
            if (!Application.isPlaying) GetScenarioSteps();
            if (!Application.isPlaying) GetScenarioObjectActivators();
            if (!Application.isPlaying) GetScenarioComponentActivators();

            if (Application.isPlaying) SetScenarioStepFromClient(scenarioSteps.Length - 1);
            else SetScenarioStepInEditor(scenarioSteps[^1]);
        }

        public void SetScenarioStepInEditor(ScenarioStep step)
        {
            if (Application.isPlaying)
            {
                SetScenarioStepFromClient(GetStepIndex(step));
            }
            else
            {
                // For in editor testing
                _lastStepIndex = _activeStepIndex;
                _activeStepIndex = GetStepIndex(step);
                ActivateCurrentStep();
            }
        }

        public void SetScenarioStep(ScenarioStep step)
        {
            if (Application.isPlaying)
            {
                if (CurrentStep == step) return;
                _lastStepIndex = _activeStepIndex;
                _activeStepIndex = GetStepIndex(step);
            }
            else
            {
                // For in editor testing
                _lastStepIndex = _activeStepIndex;
                _activeStepIndex = GetStepIndex(step);
                ActivateCurrentStep();
            }
        }

        public void SetScenarioStepFromClient(int step)
        {
            Debug.Log("Setting scenario step from " + _activeStepIndex + " to " + step + " on server");
            SetScenarioStep(scenarioSteps[step]);
        }

        public void SetFirstStep()
        {
            if (!Application.isPlaying)
            {
                EditorFirstStep();
                return;
            }

            OnStartedEvent?.Invoke();
            SetScenarioStepFromClient(0);
        }

        public void SetPreviousStep()
        {
            if (!Application.isPlaying)
            {
                EditorPreviousStep();
                return;
            }

            if (_activeStepIndex <= 0)
            {
                OnStartedEvent?.Invoke();
            }
            else
            {
                SetScenarioStepFromClient(_activeStepIndex - 1);
            }
        }

        public void SetNextStep()
        {
            if (!Application.isPlaying)
            {
                EditorNextStep();
                return;
            }

            if (_activeStepIndex >= scenarioSteps.Length - 1)
            {
                OnStoppedEvent?.Invoke();
            }
            else
            {
                SetScenarioStepFromClient(_activeStepIndex + 1);
            }
        }

        public void SetLastStep()
        {
            if (!Application.isPlaying)
            {
                EditorLastStep();
                return;
            }

            SetScenarioStepFromClient(scenarioSteps.Length - 1);
        }

        private void OnScenarioStepChange(int prev, int next, bool asServer)
        {
            Debug.Log("Received callback for scenario step change to " + next + ", asServer = " + asServer, this);
            // Make sure it only runs once when client and server are active
            ActivateCurrentStep();
        }

        private int GetStepIndex(ScenarioStep step)
        {
            for (int i = 0; i < scenarioSteps.Length; i++)
            {
                if (scenarioSteps[i] == step)
                {
                    return i;
                }
            }

            return 0;
        }

        private void ActivateCurrentStep()
        {
            if (_lastStepIndex != _activeStepIndex) DeactivateLastStep();

            if (!Application.isPlaying) GetScenarioSteps();
            if (!Application.isPlaying) GetScenarioObjectActivators();
            if (!Application.isPlaying) GetScenarioComponentActivators();

            CurrentStep.gameObject.SetActive(true);

            // This triggers all ObjectActivators to set their activation state
            foreach (ScenarioObjectActivator objectActivator in _scenarioObjectActivators)
            {
                objectActivator.ControllerOnStepChangeEvent(CurrentStep);
            }
            
            // This triggers all ComponentActivators to set their activation state
            foreach (ScenarioComponentActivator componentActivator in _scenarioComponentActivators)
            {
                componentActivator.ControllerOnStepChangeEvent(CurrentStep);
            }

            OnStepChangedEvent?.Invoke(CurrentStep);
            onStepActivated?.Invoke();
            CurrentStep.onStepStarted?.Invoke();
        }

        private void DeactivateLastStep()
        {
            if (!Application.isPlaying) GetScenarioSteps();
            if (!Application.isPlaying) GetScenarioObjectActivators();
            if (!Application.isPlaying) GetScenarioComponentActivators();

            if (_lastStepIndex != _activeStepIndex)
            {
                if (scenarioSteps[_lastStepIndex])
                {
                    scenarioSteps[_lastStepIndex].onStepStopped?.Invoke();
                    scenarioSteps[_lastStepIndex].gameObject.SetActive(false);
                }
            }
        }

        public UnityEvent onStepActivated;
    }
}