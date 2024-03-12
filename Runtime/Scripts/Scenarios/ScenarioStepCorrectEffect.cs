using Sirenix.OdinInspector;
using UnityEngine;
namespace _3Dimensions.Tools.Runtime.Scripts.Scenarios
{
    public class ScenarioStepCorrectEffect : MonoBehaviour
    {
        [SerializeField] private Light alarmLight;
        [SerializeField, Range(0, 20)] private float lightIntensity;
        [SerializeField] private float pulseLength = 0.5f;

        [SerializeField] private bool activeOnStart;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private bool loop;

        [SerializeField] private bool stopAfterTime;
        [SerializeField] private float stopTime = 4;
        private float _elapsedTime;

        private void Awake()
        {
            if (audioSource)
            {
                audioSource.playOnAwake = false;
                audioSource.loop = loop;
            }
        }
        
        void Start()
        {
            if (activeOnStart)
            {
                StartEffect();
            }
            else
            {
                StopEffect();
            }
        }
        
        private void Update()
        {
            if (stopAfterTime)
            {
                _elapsedTime += Time.deltaTime;
                if (alarmLight) alarmLight.intensity = Mathf.PingPong(_elapsedTime, pulseLength) * lightIntensity;
                if (_elapsedTime > stopTime)
                {
                    StopEffect();
                }
            }
        }
        
        [Button]
        public void StartEffect()
        {
            _elapsedTime = 0;
            if (alarmLight) alarmLight.enabled = true;
            if (audioSource) audioSource.Play();
        }

        [Button]
        public void StopEffect()
        {
            if (alarmLight)
            {
                alarmLight.intensity = 0;
                alarmLight.enabled = false;
            }
            if (audioSource) audioSource.Stop();
        }
    }
}