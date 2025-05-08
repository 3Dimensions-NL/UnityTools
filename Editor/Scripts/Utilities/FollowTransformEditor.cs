using UnityEditor;
namespace _3Dimensions.Tools.Editor.Scripts.Utilities
{
    [CustomEditor(typeof( _3Dimensions.Tools.Runtime.Scripts.Utilities.FollowTransform))]
    public class FollowTransformEditor : UnityEditor.Editor
    {
        private SerializedProperty _transformToFollowProp;
        private SerializedProperty _worldOffsetProp;
        private SerializedProperty _positionOffsetProp;
        private SerializedProperty _forceScaleProp;
        private SerializedProperty _scaleProp;
        private SerializedProperty _forceRotationProp;
        private SerializedProperty _forcedEulerRotationProp;
        private SerializedProperty _smoothenProp;
        private SerializedProperty _smoothMoveSpeedProp;
        private SerializedProperty _smoothRotateSpeedProp;
        private SerializedProperty _smoothScaleSpeedProp;
        private SerializedProperty _keepUprightProp;
        private SerializedProperty _useThresholdProp;
        private SerializedProperty _angleThresholdProp;
        private SerializedProperty _distanceThresholdProp;

        private void OnEnable()
        {
            _transformToFollowProp = serializedObject.FindProperty("transformToFollow");
            _worldOffsetProp = serializedObject.FindProperty("worldOffset");
            _positionOffsetProp = serializedObject.FindProperty("positionOffset");
            _forceScaleProp = serializedObject.FindProperty("forceScale");
            _scaleProp = serializedObject.FindProperty("scale");
            _forceRotationProp = serializedObject.FindProperty("forceRotation");
            _forcedEulerRotationProp = serializedObject.FindProperty("forcedEulerRotation");
            _smoothenProp = serializedObject.FindProperty("smoothen");
            _smoothMoveSpeedProp = serializedObject.FindProperty("smoothMoveSpeed");
            _smoothRotateSpeedProp = serializedObject.FindProperty("smoothRotateSpeed");
            _smoothScaleSpeedProp = serializedObject.FindProperty("smoothScaleSpeed");
            _keepUprightProp = serializedObject.FindProperty("keepUpright");
            _useThresholdProp = serializedObject.FindProperty("useThreshold");
            _angleThresholdProp = serializedObject.FindProperty("angleThreshold");
            _distanceThresholdProp = serializedObject.FindProperty("distanceThreshold");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_transformToFollowProp);
            EditorGUILayout.PropertyField(_worldOffsetProp);
            EditorGUILayout.PropertyField(_positionOffsetProp);
            EditorGUILayout.PropertyField(_forceScaleProp);
            if (_forceScaleProp.boolValue)
            {
                EditorGUILayout.PropertyField(_scaleProp);
            }

            EditorGUILayout.PropertyField(_forceRotationProp);
            if (_forceRotationProp.boolValue)
            {
                EditorGUILayout.PropertyField(_forcedEulerRotationProp);
            }

            EditorGUILayout.PropertyField(_smoothenProp);
            if (_smoothenProp.boolValue)
            {
                EditorGUILayout.PropertyField(_smoothMoveSpeedProp);
                EditorGUILayout.PropertyField(_smoothRotateSpeedProp);
                EditorGUILayout.PropertyField(_smoothScaleSpeedProp);
            }

            EditorGUILayout.PropertyField(_keepUprightProp);
            EditorGUILayout.PropertyField(_useThresholdProp);
            if (_useThresholdProp.boolValue)
            {
                EditorGUILayout.PropertyField(_angleThresholdProp);
                EditorGUILayout.PropertyField(_distanceThresholdProp);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}