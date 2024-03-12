using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
namespace _3Dimensions.Tools.Runtime.Scripts.Text
{
    [ExecuteAlways]
    public class InputFieldTextAnimator : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;

        [SerializeField] private bool useAnimateCompletion;
        public string textToDisplay = "Some text";
        
        [BoxGroup("Timed animation"), HideIf("useAnimateCompletion")] public float onEnableDelay = 2f;
        [BoxGroup("Timed animation"), HideIf("useAnimateCompletion")] public float nextCharacterDelay = 0.1f;

        [BoxGroup("Animated completion"), ShowIf("useAnimateCompletion"), Range(0, 1)] public float animatedCompletion;
        
        [BoxGroup("Animated completion"), ShowIf("useAnimateCompletion"), ShowInInspector] private int CharacterCount => (int)(textToDisplay.Length * animatedCompletion);

        private float _elapsedTime;

        private void OnEnable()
        {
            _elapsedTime = 0;
        }

        private void Update()
        {
            if (useAnimateCompletion)
            {
                int shownCharacterCount = (int)(textToDisplay.Length * animatedCompletion);
                
                inputField.text = "";

                for (int i = 0; i < shownCharacterCount; i++)
                {
                    inputField.text += textToDisplay[i];
                }
                
            }
            else
            {
                _elapsedTime += Time.deltaTime;

                inputField.text = "";

                for (int i = 0; i < textToDisplay.Length; i++)
                {
                    float characterTime = i * nextCharacterDelay + onEnableDelay;
                    if (_elapsedTime > characterTime)
                    {
                        inputField.text += textToDisplay[i];
                    }
                }
            }
        }
    }
}
