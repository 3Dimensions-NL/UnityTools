using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace _3Dimensions.Tools.Runtime.Scripts.LevelDesign
{
    public class AreaObjectPlacement : MonoBehaviour
    {
        [SerializeField] private GameObject[] prefabs;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private Vector2 size = new Vector2(3, 3);
        [SerializeField] private float placementOffset = 5;
        [SerializeField] private bool randomRotation;
        [SerializeField] private float raycastLength = 10;

        [SerializeField, Tooltip("Read-only position count (calculated from placement positions).")]
        private int positionCount;

        private Vector3[] PlacementPositions
        {
            get
            {
                int xCount = Mathf.RoundToInt(size.x / placementOffset);
                int yCount = Mathf.RoundToInt(size.y / placementOffset);

                Vector3[] positions = new Vector3[xCount * yCount];

                int index = 0;
                for (int x = 0; x < xCount; x++)
                {
                    for (int y = 0; y < yCount; y++)
                    {
                        float xPos = placementOffset * x - (size.x * 0.5f);
                        float zPos = placementOffset * y - (size.y * 0.5f);
                        Vector3 localPos = new Vector3(xPos, raycastLength * 0.5f, zPos);
                        Vector3 worldPosRayOrigin = transform.TransformPoint(localPos);
                        Vector3 worldPos;
                        if (Physics.Raycast(worldPosRayOrigin, -transform.up, out RaycastHit hit, raycastLength,
                                layerMask))
                        {
                            worldPos = hit.point;
                        }
                        else
                        {
                            worldPos = transform.TransformPoint(new Vector3(localPos.x, 0, localPos.z));
                        }

                        positions[index] = worldPos;
                        index++;
                    }
                }

                positionCount = positions.Length; // Update position count
                return positions;
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// Spawns prefabs at placement positions.
        /// </summary>
        public void SpawnPrefabs()
        {
            RemovePrefabs();

            int count = PlacementPositions.Length;
            for (int i = 0; i < count; i++)
            {
                GameObject go = PrefabUtility.InstantiatePrefab(prefabs[Random.Range(0, prefabs.Length)]) as GameObject;
                go.transform.SetPositionAndRotation(PlacementPositions[i],
                    randomRotation ? Quaternion.Euler(0, Random.Range(0, 359), 0) : Quaternion.identity);
                go.transform.SetParent(transform);
            }
        }

        /// <summary>
        /// Removes all prefabs that were spawned.
        /// </summary>
        public void RemovePrefabs()
        {
            if (transform.childCount > 0)
            {
                for (int i = transform.childCount - 1; i >= 0; i--)
                {
                    Transform child = transform.GetChild(i);
                    DestroyImmediate(child.gameObject);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (Selection.activeGameObject != gameObject) return;
            Gizmos.color = Color.green;
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.matrix = rotationMatrix;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(size.x, raycastLength, size.y));

            if (PlacementPositions.Length < 100)
            {
                for (int i = 0; i < PlacementPositions.Length; i++)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawWireSphere(transform.InverseTransformPoint(PlacementPositions[i]), 1f);
                }
            }
        }
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(AreaObjectPlacement))]
    public class AreaObjectPlacementEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            AreaObjectPlacement script = (AreaObjectPlacement)target;

            // Add buttons for Spawn and Remove Prefabs
            if (GUILayout.Button("Spawn Prefabs"))
            {
                script.SpawnPrefabs();
            }

            if (GUILayout.Button("Remove Prefabs"))
            {
                script.RemovePrefabs();
            }
        }
    }
#endif
}