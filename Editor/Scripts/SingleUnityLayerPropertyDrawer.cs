using _3Dimensions.Tools.Runtime.Scripts.Utilities;
using UnityEditor;
using UnityEngine;
namespace _3Dimensions.Tools.Editor.Scripts
{
    [CustomPropertyDrawer(typeof(SingleUnityLayer))]
    public class SingleUnityLayerPropertyDrawer : PropertyDrawer 
    {
        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            EditorGUI.BeginProperty(_position, GUIContent.none, _property);
            SerializedProperty layerIndex = _property.FindPropertyRelative("m_LayerIndex");
            _position = EditorGUI.PrefixLabel(_position, GUIUtility.GetControlID(FocusType.Passive), _label);
            if (layerIndex != null)
            {
                layerIndex.intValue = EditorGUI.LayerField(_position, layerIndex.intValue);
            }
            EditorGUI.EndProperty( );
        }
    }
}