using System;
using System.Collections;
using UnityEngine;
// Required for Coroutine

namespace _3Dimensions.Tools.Runtime.Scripts.Utilities
{
    [ExecuteAlways]
    public class FollowTransform : MonoBehaviour
    {
        public Transform transformToFollow;
        public UpdateLoop updateLoop = UpdateLoop.LateUpdate;
        
        [Tooltip("Use world or local offset")]
        public bool worldOffset;

        [Tooltip("The offset relative to the transform to follow (for both world or relative offset)")]
        public Vector3 positionOffset;

        [Tooltip("Force a specific scale or follow the transform's scale")]
        public bool forceScale;
        public Vector3 scale = Vector3.one;

        [Tooltip("Force a rotation, or follow the rotation of the other transform")]
        public bool forceRotation;
        public Vector3 forcedEulerRotation;

        [Tooltip("Enable smooth movement, rotation, and scale interpolation")]
        public bool smoothen;
        public float smoothMoveSpeed = 10f;
        public float smoothRotateSpeed = 5f;
        public float smoothScaleSpeed = 2f;

        [Tooltip("Keep the object upright when following")]
        public bool keepUpright;

        [Tooltip("Use threshold to decide when to reset transform to the target")]
        public bool useThreshold;
        public float angleThreshold = 30f;
        public float distanceThreshold = 0.3f;

        private float _angle;
        private bool _resetting;
        private Vector3 _lastPosition;

        public void ForcePositionAndRotation(Transform newTransformToFollow)
        {
            transformToFollow = newTransformToFollow;
            transform.SetPositionAndRotation(CalculatedPosition, CalculatedRotation);
            transform.localScale = forceScale ? scale : transformToFollow.localScale;
        }

        private void Start()
        {
            if (transformToFollow)
            {
                _lastPosition = transformToFollow.position;
            }
        }

        void Update()
        {
            if (!transformToFollow) return;
            if (updateLoop != UpdateLoop.Update) return;

            Calculate(Time.deltaTime);
        }

        private void LateUpdate()
        {
            if (!transformToFollow) return;
            if (updateLoop != UpdateLoop.LateUpdate) return;

            Calculate(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (!transformToFollow) return;
            if (updateLoop != UpdateLoop.FixedUpdate) return;
            
            Calculate(Time.fixedDeltaTime);
        }

        private void Calculate(float delta)
        {
            transform.localScale = Vector3.Lerp(
                transform.localScale,
                forceScale ? scale : transformToFollow.localScale,
                delta * smoothScaleSpeed
            );

            if (!smoothen)
            {
                ForcePositionAndRotation(transformToFollow);
                return;
            }

            float distance = Vector3.Distance(transformToFollow.position, _lastPosition);

            Vector3 currentForwardFlattened = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
            Vector3 targetForwardFlattened = new Vector3(transformToFollow.forward.x, 0, transformToFollow.forward.z).normalized;
            _angle = Vector3.Angle(currentForwardFlattened, targetForwardFlattened);

            if (useThreshold)
            {
                if (distance > distanceThreshold || _angle >= angleThreshold)
                {
                    if (!_resetting)
                    {
                        _resetting = true;

                        StartCoroutine(ResetTransformRoutine(delta));
                    }

                    return;
                }

                return;
            }
            
            Vector3 lerpPosition = Vector3.Lerp(transform.position, CalculatedPosition, delta * smoothMoveSpeed);
            Quaternion lerpRotation = Quaternion.Lerp(transform.rotation, CalculatedRotation, delta * smoothRotateSpeed);

            transform.SetPositionAndRotation(lerpPosition, lerpRotation);
        }

        private Vector3 CalculatedPosition
        {
            get
            {
                if (worldOffset)
                {
                    return transformToFollow.position + positionOffset;
                }

                return transformToFollow.rotation * positionOffset + transformToFollow.position;
            }
        }

        private Quaternion CalculatedRotation
        {
            get
            {
                if (forceRotation)
                {
                    return Quaternion.Euler(forcedEulerRotation);
                }

                return keepUpright
                    ? Quaternion.FromToRotation(transformToFollow.up, Vector3.up) * transformToFollow.rotation
                    : transformToFollow.rotation;
            }
        }

        private IEnumerator ResetTransformRoutine(float delta)
        {
            float duration = 0.5f;
            float elapsedTime = 0f;

            Vector3 startPosition = transform.position;
            Quaternion startRotation = transform.rotation;

            while (elapsedTime < duration)
            {
                elapsedTime += delta;
                float blend = elapsedTime / duration;

                transform.position = Vector3.Lerp(startPosition, CalculatedPosition, blend);
                transform.rotation = Quaternion.Lerp(startRotation, CalculatedRotation, blend);

                yield return null;
            }

            transform.SetPositionAndRotation(CalculatedPosition, CalculatedRotation);
            _lastPosition = transformToFollow.position;
            _resetting = false;
        }

        public enum UpdateLoop
        {
            Update,
            LateUpdate,
            FixedUpdate
        }
    }
}