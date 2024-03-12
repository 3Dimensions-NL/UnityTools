using Sirenix.OdinInspector;
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
        [ShowInInspector] public ScenarioStep CurrentStep {
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

        private int _activeStepIndex;
        public int lastStepIndex;
        
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

            foreach (ScenarioStep step in scenarioSteps)
            {
                step.gameObject.SetActive(false);
            }
        }

        public void Start()
        {
            _activeStepIndex = 0;
            lastStepIndex = 0;
            ActivateCurrentStep();
        }
        
        [Button (SdfIconType.Plus)]
        private void CreateScenarioStep()
        {
            GameObject newStep = new GameObject("New Step");
            newStep.transform.SetParent(transform);
            newStep.AddComponent<ScenarioStep>();
            scenarioSteps = GetComponentsInChildren<ScenarioStep>();
        }

        [Button]
        private void ActivateAllSteps()
        {
            GetScenarioSteps();
            GetScenarioObjectActivators();
            
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
        
        [Button]
        public void GetScenarioSteps()
        {
            scenarioSteps = GetComponentsInChildren<ScenarioStep>(true);
        }

        public void GetScenarioObjectActivators()
        {
            _scenarioObjectActivators = FindObjectsOfType<ScenarioObjectActivator>(true);
        }

        private void EditorFirstStep()
        {
            if (!Application.isPlaying) GetScenarioSteps();
            if (!Application.isPlaying) GetScenarioObjectActivators();

            if (Application.isPlaying) SetScenarioStepFromClient(0);
            else SetScenarioStepInEditor(scenarioSteps[0]);
            OnStartedEvent?.Invoke();
        }
        
        private void EditorPreviousStep()
        {
            if (!Application.isPlaying) GetScenarioSteps();
            if (!Application.isPlaying) GetScenarioObjectActivators();

            if (_activeStepIndex <= 0)
            {
                if (Application.isPlaying) SetScenarioStepFromClient(0);
                else SetScenarioStepInEditor(scenarioSteps[0]);
                OnStartedEvent?.Invoke();
                return;
            }
            
            if (Application.isPlaying) SetScenarioStepFromClient(_activeStepIndex - 1);
            else SetScenarioStepInEditor(scenarioSteps[_activeStepIndex - 1]);
        }
        
        private void EditorNextStep()
        {
            if (!Application.isPlaying) GetScenarioSteps();
            if (!Application.isPlaying) GetScenarioObjectActivators();

            for (int i = 0; i < scenarioSteps.Length; i++)
            {
                if (CurrentStep == scenarioSteps[i])
                {
                    if (i >= scenarioSteps.Length -1)
                    {
                        OnStoppedEvent?.Invoke();
                        return;
                    }

                    if (Application.isPlaying) SetScenarioStepFromClient(i + 1);
                    else SetScenarioStepInEditor(scenarioSteps[i+1]);
                    return;
                }
            }
        }

        private void EditorLastStep()
        {
            if (!Application.isPlaying) GetScenarioSteps();
            if (!Application.isPlaying) GetScenarioObjectActivators();
            
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
                //For in editor testing
                lastStepIndex = _activeStepIndex;
                _activeStepIndex = GetStepIndex(step);
                ActivateCurrentStep();
            }
        }

        public void SetScenarioStep(ScenarioStep step)
        {
            if (Application.isPlaying)
            {
                if (CurrentStep == step) return;
                lastStepIndex = _activeStepIndex;
                _activeStepIndex = GetStepIndex(step);
            }
            else
            {
                //For in editor testing
                lastStepIndex = _activeStepIndex;
                _activeStepIndex = GetStepIndex(step);
            }
        }

        public void SetScenarioStepFromClient(int step)
        {
            Debug.Log("Setting scenario step from " + _activeStepIndex + " to " + step + " on server");
            SetScenarioStep(scenarioSteps[step]);
        }

        [HorizontalGroup("Steps")]
        [Button("|<")]
        public void FirstStep()
        {
            if (!Application.isPlaying)
            {
                EditorFirstStep();
                return;
            }
            
            OnStartedEvent?.Invoke();
            SetScenarioStepFromClient(0);
        }
        
        [HorizontalGroup("Steps")]
        [Button("<")]
        public void PreviousStep()
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
        
        [HorizontalGroup("Steps")]
        [Button(">")]
        public void NextStep()
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
        
        [HorizontalGroup("Steps")]
        [Button(">|")]
        public void LastStep()
        {
            if (!Application.isPlaying)
            {
                EditorLastStep();
                return;
            }
            
            SetScenarioStepFromClient(scenarioSteps.Length - 1);
        }

        /// <summary>
        /// Scenario step callback
        /// </summary>
        /// <param name="prev"></param>
        /// <param name="next"></param>
        /// <param name="asServer"></param>
        private void OnScenarioStepChange(int prev, int next, bool asServer)
        {
            Debug.Log("Received callback for scenario step change to " + next + ", asServer = " + asServer, this);
            //Make sure it only runs ones when client and server are active
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
            if (lastStepIndex != _activeStepIndex) DeactivateLastStep();

            if (!Application.isPlaying) GetScenarioSteps();
            if (!Application.isPlaying) GetScenarioObjectActivators();
            
            CurrentStep.gameObject.SetActive(true);

            //This triggers all ObjectActivators to set their activation state
            foreach (ScenarioObjectActivator objectActivator in _scenarioObjectActivators)
            {
                objectActivator.ControllerOnOnStepChangeEvent(CurrentStep);
            }
            
            OnStepChangedEvent?.Invoke(CurrentStep);
            onStepActivated?.Invoke();
            CurrentStep.onStepStarted?.Invoke();
        }

        private void DeactivateLastStep()
        {
            if (!Application.isPlaying) GetScenarioSteps();
            if (!Application.isPlaying) GetScenarioObjectActivators();
            
            if (lastStepIndex != _activeStepIndex)
            {
                if (scenarioSteps[lastStepIndex])
                {
                    scenarioSteps[lastStepIndex].onStepStopped?.Invoke();
                    scenarioSteps[lastStepIndex].gameObject.SetActive(false);
                }
            }
        }
        
        public UnityEvent onStepActivated;
    }
}
