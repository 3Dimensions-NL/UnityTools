using UnityEngine;
namespace _3Dimensions.Tools.Runtime.Scripts
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
    }
}
