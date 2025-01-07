using _3Dimensions.Tools.Runtime.Scripts.LevelDesign;
using UnityEditor;
using UnityEngine;

namespace _3Dimensions.Tools.Editor.Scripts.LevelDesign
{
    [CustomEditor(typeof(LineObjectPlacementPoint))]
    public class LineObjectPlacementPointEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // Draw the default Inspector
            DrawDefaultInspector();

            // Get a reference to the target script
            LineObjectPlacementPoint placementPoint = (LineObjectPlacementPoint)target;

            // Add custom buttons
            if (GUILayout.Button("Select Line Object"))
            {
                placementPoint.SelectLineObject();
            }

            if (GUILayout.Button("Add New Point"))
            {
                placementPoint.AddNewPoint();
            }

            if (GUILayout.Button("Remove This Point"))
            {
                placementPoint.RemoveThisPoint();
            }
        }
    }
}