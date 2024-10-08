﻿using UnityEngine;
namespace _3Dimensions.Tools.Runtime.Scripts.Utilities
{
    /// <summary>
    /// Utility to rotate an object in the direction of the main camera
    /// </summary>
    [ExecuteAlways]
    public class LookAtCamera : MonoBehaviour
    {
        private Camera _viewCamera;

        void LateUpdate()
        {
           
            if (Camera.main == null) return;
            
            if (_viewCamera != Camera.main)
            {
                _viewCamera = Camera.main;
            }

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                if (UnityEditor.SceneView.GetAllSceneCameras().Length != 0)
                {
                    _viewCamera = UnityEditor.SceneView.GetAllSceneCameras()[0];
                }
            }
#endif
            
            if (_viewCamera == null) return;

            Vector3 targetPosition = new Vector3(_viewCamera.transform.position.x, transform.position.y, _viewCamera.transform.position.z);
            transform.LookAt(targetPosition);
            transform.rotation *= Quaternion.Euler(0, 180f, 0);
        }
    }
}