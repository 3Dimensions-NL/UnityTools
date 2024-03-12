using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
namespace _3Dimensions.Tools.Runtime.Scripts.Text
{
    public class TextMeshProAnimator : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        [SerializeField] private bool useAnimateCompletion;
        public string textToDisplay = "Some text";

        [BoxGroup("Timed animation"), HideIf("useAnimateCompletion")]
        public float onEnableDelay = 2f;

        [BoxGroup("Timed animation"), HideIf("useAnimateCompletion")]
        public float nextCharacterDelay = 0.1f;

        [BoxGroup("Animated completion"), ShowIf("useAnimateCompletion"), Range(0, 1)]
        public float animatedCompletion;

        [BoxGroup("Animated completion"), ShowIf("useAnimateCompletion"), ShowInInspector]
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

                text.text = "";

                for (int i = 0; i < shownCharacterCount; i++)
                {
                    text.text += textToDisplay[i];
                }

            }
            else
            {
                _elapsedTime += Time.deltaTime;

                text.text = "";

                for (int i = 0; i < textToDisplay.Length; i++)
                {
                    float characterTime = i * nextCharacterDelay + onEnableDelay;
                    if (_elapsedTime > characterTime)
                    {
                        text.text += textToDisplay[i];
                    }
                }
            }
        }
    }
}
