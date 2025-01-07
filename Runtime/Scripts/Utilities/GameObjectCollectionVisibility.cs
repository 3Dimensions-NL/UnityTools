using UnityEngine;
namespace _3Dimensions.Tools.Runtime.Scripts.Utilities
{
    [ExecuteAlways]
    public class GameObjectCollectionVisibility : MonoBehaviour
    {
        public GameObject[] gameObjects;
        public bool visible;

        private void Update()
        {
            foreach (var go in gameObjects)
            {
                go.SetActive(visible);
            }
        }
        
        public void CollectChildren()
        {
            gameObjects = new GameObject[transform.childCount];

            for (int i = 0; i < gameObjects.Length; i++)
            {
                gameObjects[i] = transform.GetChild(i).gameObject;
            }

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
}
