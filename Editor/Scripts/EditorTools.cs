using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace _3Dimensions.Tools.Editor.Scripts
{
    public static class EditorTools
    {
        [MenuItem("3Dimensions/ Find Missing Scripts In Project")]
        static void FindMissingScriptsInProjectMenuItem()
        {
            string[] prefabPaths = AssetDatabase.GetAllAssetPaths()
                .Where(path => path.EndsWith(".prefab", System.StringComparison.OrdinalIgnoreCase)).ToArray();

            int current = 0;
            int found = 0;

            foreach (string path in prefabPaths)
            {
                current++;
                EditorUtility.DisplayProgressBar("Searching for prefabs with missing scripts...",
                    "Found (" + current + "/" + prefabPaths.Length + ") prefabs.", (float)prefabPaths.Length / current);

                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                foreach (Component component in prefab.GetComponentsInChildren<Component>())
                {
                    EditorUtility.DisplayProgressBar("Searching for prefabs with missing scripts...",
                        "Checking (" + current + "/" + prefabPaths.Length + ") prefabs.",
                        (float)prefabPaths.Length / current);

                    if (component == null)
                    {
                        Debug.Log("Prefab found with missing script: " + path, prefab);
                        found++;
                    }
                }

                EditorUtility.ClearProgressBar();

                if (found == 0)
                {
                    Debug.Log("No prefabs found with missing script.");
                }
                else
                {
                    Debug.LogWarning("Found " + found + " missing scripts! See Logs for prefab references.");
                }
            }
        }

        [MenuItem("3Dimensions/Find Missing Scripts In Scene")]
        static void FindMissingScriptsInSceneMenuItem()
        {
            foreach (GameObject gameObject in GameObject.FindObjectsOfType<GameObject>(true))
            {
                foreach (Component component in gameObject.GetComponents<Component>())
                {
                    if (component == null)
                    {
                        Debug.Log("GameObject found with missing script: " + gameObject.name + "Path = " + GetGameObjectPath(gameObject), gameObject);
                        lastFoundSceneGameObjectWithNoScripts = gameObject;
                        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                        break;
                    }
                }
            }
        }

        private static GameObject lastFoundSceneGameObjectWithNoScripts;

        [MenuItem("3Dimensions/Destroy last found GameObject with missing scripts")]
        static void DestroyLastFoundGameObjectWithMissingScriptsMenuItem()
        {
            if (lastFoundSceneGameObjectWithNoScripts)
            {
                string name = lastFoundSceneGameObjectWithNoScripts.name;
                GameObject.DestroyImmediate(lastFoundSceneGameObjectWithNoScripts);
                Debug.Log(name + " was destroyed.");
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
            else
            {
                Debug.LogWarning("There was no GameObject found with missing scripts!");
            }
        }
        
        public static string GetGameObjectPath(GameObject obj)
        {
            string path = "/" + obj.name;
            while (obj.transform.parent != null)
            {
                obj = obj.transform.parent.gameObject;
                path = "/" + obj.name + path;
            }
            return path;
        }
        
        [MenuItem("3Dimensions/Show Hidden Objects")]
        private static void ShowAll()
        {
            var allGameObjects = Object.FindObjectsOfType<GameObject>(true);
            foreach (var go in allGameObjects)
            {
                switch (go.hideFlags)
                {
                    case HideFlags.HideAndDontSave:
                        go.hideFlags = HideFlags.DontSave;
                        break;
                    case HideFlags.HideInHierarchy:
                    case HideFlags.HideInInspector:
                        go.hideFlags = HideFlags.None;
                        break;
                }
            }
        }
    }
}