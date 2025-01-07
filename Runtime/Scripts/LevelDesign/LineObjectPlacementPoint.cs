using UnityEngine;

namespace _3Dimensions.Tools.Runtime.Scripts.LevelDesign
{
    public class LineObjectPlacementPoint : MonoBehaviour
    {
        public LineObjectPlacement lineObject;

#if UNITY_EDITOR
        public void SelectLineObject()
        {
            if (lineObject == null) return;
            UnityEditor.Selection.activeGameObject = lineObject.gameObject;
        }

        public void AddNewPoint()
        {
            if (lineObject == null) return;
            lineObject.AddNewPoint();
        }

        public void RemoveThisPoint()
        {
            if (lineObject != null)
            {
                UnityEditor.Selection.activeGameObject = lineObject.gameObject;
            }

            DestroyImmediate(gameObject);
        }
#endif

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(transform.position, 0.5f);
        }
    }
}