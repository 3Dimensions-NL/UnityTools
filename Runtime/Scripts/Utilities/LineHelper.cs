using System;
using UnityEngine;
namespace _3Dimensions.Tools.Runtime.Scripts.Utilities
{
#if UNITY_EDITOR
    [UnityEditor.CanEditMultipleObjects]
#endif
    [ExecuteAlways]
    [Serializable]
    [RequireComponent(typeof(LineRenderer))]
    public class LineHelper : MonoBehaviour {

        public bool worldSpace = true;
        public bool updateInRuntime = false;
        public bool useChildren;
        public bool loop;

        private LineRenderer _lr;
        [SerializeField] Transform[] linePoints;

        void Start () {
            _lr = GetComponent<LineRenderer>();
            _lr.generateLightingData = true;
        }
	
        void LateUpdate () {
            if ((updateInRuntime && Application.isPlaying) || !Application.isPlaying)
            {
                if (useChildren)
                {
                    if (linePoints.Length != transform.childCount)
                    {
                        linePoints = new Transform[transform.childCount];
                        for (int i = 0; i < linePoints.Length; i++)
                        {
                            linePoints[i] = transform.GetChild(i);
                        }
                    }
                }
                
                if (linePoints.Length > 1)
                {
                    UpdateLine();
                }
                else
                {
                    Debug.LogWarning("Line has to few points");
                }
            }
        }

        private void UpdateLine()
        {
            _lr.useWorldSpace = worldSpace;

            Vector3[] positions = new Vector3[linePoints.Length];
            for (int i = 0; i < linePoints.Length; i++)
            {
                if (worldSpace)
                {
                    positions[i] = linePoints[i].position;
                }
                else
                {
                    positions[i] = linePoints[i].localPosition;
                }
            }
            _lr.positionCount = positions.Length;
            _lr.SetPositions(positions);
            _lr.loop = loop;
        }
    }
}
