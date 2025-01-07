using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _3Dimensions.Tools.Runtime.Scripts.LevelDesign
{
    [ExecuteAlways]
    public class LineObjectPlacement : MonoBehaviour
    {
        [Serializable]
        public struct LineSegment
        {
            public GameObject prefab;
            public Vector3 positionCorrection;
            public Vector3 rotationCorrection;
            public float length;
            public bool isCorner;
            public bool isEndpoint;
        }

        private class LineSection
        {
            public List<LineSegment> Segments = new List<LineSegment>();
        }

        [SerializeField] private bool instantiateInOrder;
        [SerializeField] private LineSegment[] segments;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float raycastLength = 10;

        private Transform[] LinePoints
        {
            get
            {
                if (transform.childCount == 0) return Array.Empty<Transform>();
                Transform[] points = new Transform[transform.childCount];
                for (int i = 0; i < transform.childCount; i++)
                {
                    points[i] = transform.GetChild(i);
                    points[i].name = "Point (" + (i + 1) + ")";
                }

                return points;
            }
        }

        private Vector3[] _linePointPositions = Array.Empty<Vector3>();

        private void LateUpdate()
        {
            if (!Application.isPlaying)
            {
                if (LinePoints == null)
                {
                    return;
                }

                if (LinePoints.Length != _linePointPositions.Length)
                {
                    PlacePrefabs();
                    return;
                }

                bool redrawRequired = false;
                for (int i = 0; i < LinePoints.Length; i++)
                {
                    if (LinePoints[i].position != _linePointPositions[i])
                    {
                        redrawRequired = true;
                    }
                }

                if (redrawRequired) PlacePrefabs();
            }
        }

        public void AddNewPoint()
        {
            // Create point
            Transform newPoint = new GameObject().transform;

            // Set position of new point at last location
            if (LinePoints == null || LinePoints.Length == 0)
            {
                newPoint.position = transform.position;
            }
            else
            {
                newPoint.position = LinePoints[^1].position;
            }

            newPoint.SetParent(transform);
            LineObjectPlacementPoint point = newPoint.gameObject.AddComponent<LineObjectPlacementPoint>();
            point.lineObject = this;

            // Select point in editor
#if UNITY_EDITOR
            UnityEditor.Selection.activeGameObject = newPoint.gameObject;
#endif
        }

        private void PlacePrefabs()
        {
            RemovePrefabs();
            CorrectLinePointsHeight();

            if (LinePoints == null) return;
            if (LinePoints.Length <= 1) return;

            _linePointPositions = new Vector3[LinePoints.Length];

            int cornerIndex = 0;
            int segmentIndex = 0;

            for (int i = 0; i < LinePoints.Length; i++)
            {
                // Save position for later check on changes
                _linePointPositions[i] = LinePoints[i].position;

                // First place corner segments
                LineSegment[] cornerSegments = Array.FindAll(segments, x => x.isCorner);
                LineSegment[] endSegments = Array.FindAll(segments, x => x.isEndpoint);

                Vector3 startPoint = i < LinePoints.Length - 1
                    ? new Vector3(LinePoints[i].position.x, 0, LinePoints[i].position.z)
                    : new Vector3(LinePoints[i - 1].position.x, 0, LinePoints[i - 1].position.z);

                Vector3 endPoint = i < LinePoints.Length - 1
                    ? new Vector3(LinePoints[i + 1].position.x, 0, LinePoints[i + 1].position.z)
                    : new Vector3(LinePoints[i].position.x, 0, LinePoints[i].position.z);

                Quaternion rotation = Quaternion.LookRotation(endPoint - startPoint, Vector3.up);

                // Place corners & endpoint
                if (instantiateInOrder)
                {
                    if (cornerSegments.Length > 0)
                    {
                        if (i < LinePoints.Length - 1)
                        {
                            Instantiate(cornerSegments[cornerIndex].prefab,
                                LinePoints[i].position,
                                rotation,
                                LinePoints[i]);
                            cornerIndex++;
                            if (cornerIndex >= cornerSegments.Length) cornerIndex = 0;
                        }
                    }
                }
                else
                {
                    if (cornerSegments.Length > 0)
                    {
                        if (i < LinePoints.Length - 1)
                        {
                            Instantiate(cornerSegments[Random.Range(0, cornerSegments.Length - 1)].prefab,
                                LinePoints[i].position,
                                rotation,
                                LinePoints[i]);
                        }
                    }
                }

                if (endSegments.Length > 0)
                {
                    int index = Random.Range(0, endSegments.Length - 1);
                    Instantiate(endSegments[index].prefab,
                        LinePoints[i].position + (rotation * endSegments[index].positionCorrection),
                        rotation,
                        LinePoints[i]);
                }

                // Now place segments in between
                LineSegment[] inBetweenSegments =
                    Array.FindAll(segments, x => x is { isCorner: false, isEndpoint: false });

                if (i < LinePoints.Length - 1)
                {
                    Vector3 direction = (LinePoints[i + 1].position - LinePoints[i].position).normalized;
                    float targetDistance = Vector3.Distance(LinePoints[i].position, LinePoints[i + 1].position);
                    float placedLength = 0;

                    while (placedLength <= targetDistance)
                    {
                        LineSegment segment = instantiateInOrder
                            ? inBetweenSegments[segmentIndex]
                            : inBetweenSegments[Random.Range(0, inBetweenSegments.Length)];

                        if (segment.length == 0)
                        {
                            Debug.LogError(
                                "Segment " + segment + " length was 0! Length must be set to a larger value!");
                            return;
                        }

                        Vector3 segmentStart = LinePoints[i].position + (direction * placedLength);
                        Vector3 segmentEnd = segmentStart + (direction * segment.length);
                        Vector3 segmentPosition = new Vector3(segmentStart.x,
                            CorrectedSegmentHeight(segmentStart, segmentEnd).y, segmentStart.z);
                        Instantiate(segment.prefab,
                            segmentPosition + (Quaternion.LookRotation(direction) * segment.positionCorrection),
                            rotation * Quaternion.Euler(segment.rotationCorrection), LinePoints[i]);

                        placedLength += segment.length;

                        if (instantiateInOrder)
                        {
                            segmentIndex = segmentIndex < inBetweenSegments.Length - 1 ? segmentIndex + 1 : 0;
                        }
                    }
                }
            }
        }

        private void RemovePrefabs()
        {
            if (LinePoints == null) return;
            if (LinePoints.Length == 0) return;

            for (int i = 0; i < LinePoints.Length; i++)
            {
                if (LinePoints[i].childCount != 0)
                {
                    for (int j = LinePoints[i].childCount - 1; j >= 0; j--)
                    {
                        Transform child = LinePoints[i].GetChild(j);
                        DestroyImmediate(child.gameObject);
                    }
                }
            }
        }

        private void CorrectLinePointsHeight()
        {
            for (int i = 0; i < LinePoints.Length; i++)
            {
                Vector3 worldPosRayOrigin = LinePoints[i].position + Vector3.up * (raycastLength * 0.5f);
                Vector3 worldPos;
                if (Physics.Raycast(worldPosRayOrigin, -transform.up, out RaycastHit hit, raycastLength, layerMask))
                {
                    worldPos = hit.point;
                }
                else
                {
                    worldPos = LinePoints[i].position;
                }

                LinePoints[i].position = worldPos;
            }
        }

        private Vector3 CorrectedSegmentHeight(Vector3 startPoint, Vector3 endPoint)
        {
            Vector3 halfway = (endPoint - startPoint) * 0.5f;
            Vector3 basePos = startPoint + halfway;
            Vector3 worldPosRayOrigin = basePos + Vector3.up * (raycastLength * 0.5f);
            if (Physics.Raycast(worldPosRayOrigin, -transform.up, out RaycastHit hit, raycastLength, layerMask))
            {
                return hit.point;
            }

            return basePos;
        }

        private void OnDrawGizmos()
        {
            if (LinePoints == null) return;
            if (LinePoints.Length == 0) return;
            Gizmos.color = Color.cyan;

            for (int i = 0; i < LinePoints.Length; i++)
            {
                if (i < LinePoints.Length - 1)
                {
                    Gizmos.DrawLine(LinePoints[i].position, LinePoints[i + 1].position);
                }
            }
        }
    }
}