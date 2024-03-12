using Sirenix.OdinInspector;
using UnityEngine;
namespace _3Dimensions.Tools.Runtime.Scripts.LevelDesign
{
    public class LineObjectPlacementPoint : MonoBehaviour
    {
        public LineObjectPlacement lineObject;

        [Button]
        private void SelectLineObject()
        {
            #if UNITY_EDITOR
            if (lineObject == null) return;
            UnityEditor.Selection.activeGameObject = lineObject.gameObject;
            #endif
        }

        [Button]
        private void AddNewPoint()
        {
            if (lineObject == null) return;
            lineObject.AddNewPoint();
        }

        [Button]
        private void RemoveThisPoint()
        {
            #if UNITY_EDITOR
            if (lineObject != null)
            {
                UnityEditor.Selection.activeGameObject = lineObject.gameObject;
            }
            #endif
            DestroyImmediate(gameObject);
        } 
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(transform.position, 0.5f);
        }
    }
}
