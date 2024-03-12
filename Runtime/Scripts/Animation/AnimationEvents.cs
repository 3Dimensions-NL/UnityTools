using UnityEngine;
namespace _3Dimensions.Tools.Runtime.Scripts.Animation
{
    public class AnimationEvents : MonoBehaviour
    {
        public Animator Animator;
        public string AnimationParameterName = "IsWalking";

        public void SetFloat(float value)
        {
            Animator.SetFloat(AnimationParameterName, value);
        }

        public void SetInt(int value)
        {
            Animator.SetInteger(AnimationParameterName, value);
        }

        public void SetBool(bool value)
        {
            Animator.SetBool(AnimationParameterName, value);
        }

        public void SetTrigger()
        {
            Animator.SetTrigger(AnimationParameterName);
        }
    }
}
