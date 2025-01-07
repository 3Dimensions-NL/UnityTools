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

        // Fields for Timed Animation
        [SerializeField] private float onEnableDelay = 2f;
        [SerializeField] private float nextCharacterDelay = 0.1f;

        // Fields for Animated Completion
        [SerializeField] [Range(0, 1)] private float animatedCompletion;

        // Property displayed only in the inspector
        private int CharacterCount => (int)(textToDisplay.Length * animatedCompletion);

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