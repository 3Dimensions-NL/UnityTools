using Sirenix.OdinInspector;
using UnityEngine;
namespace _3Dimensions.Tools.Runtime.Scripts.Animation
{
    public class HumanAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private RuntimeAnimatorController animatorController;
        [SerializeField] private float velocityScaleForward = 1f;
        [SerializeField] private float velocityScaleStrafe = 0.5f;
        [SerializeField] private float animationChangeSpeed = 20;
        
        private Vector3 _previousPosition;
        [ShowInInspector] private float _forwardVelocity;
        private float _lastForwardVelocity;
        [ShowInInspector] private float _strafeVelocity;
        private float _lastStrafeVelocity;
        private static readonly int ForwardVelocity = Animator.StringToHash("ForwardVelocity");
        private static readonly int StrafeVelocity = Animator.StringToHash("StrafeVelocity");

        private void Awake()
        {
            animator.runtimeAnimatorController = animatorController;
        }
        
        private void OnEnable()
        {
            _previousPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 currentPosition = transform.position;
            Vector3 relativePosition = currentPosition - _previousPosition;

            _forwardVelocity = velocityScaleForward * Vector3.Dot(relativePosition, transform.forward) * Time.deltaTime;
            _strafeVelocity = velocityScaleStrafe * Vector3.Dot(relativePosition, transform.right) * Time.deltaTime;
            
            Debug.Log("Forward Velocity = " + _forwardVelocity + " Strafe Velocity = " + _strafeVelocity); 
            
            _forwardVelocity = Mathf.Lerp(_lastForwardVelocity, _forwardVelocity, Time.deltaTime * animationChangeSpeed);
            _strafeVelocity = Mathf.Lerp(_lastStrafeVelocity, _strafeVelocity, Time.deltaTime * animationChangeSpeed);
            
            _forwardVelocity = Mathf.Clamp(_forwardVelocity, -1, 1);
            _strafeVelocity = Mathf.Clamp(_strafeVelocity, -1, 1);

            animator.SetFloat(ForwardVelocity, _forwardVelocity);
            animator.SetFloat(StrafeVelocity, _strafeVelocity);
            
            _previousPosition = currentPosition;
            _lastForwardVelocity = _forwardVelocity;
            _lastStrafeVelocity = _strafeVelocity;
            
        }
    }
}
