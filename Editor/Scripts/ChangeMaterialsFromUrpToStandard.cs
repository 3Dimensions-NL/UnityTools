using UnityEditor;
using UnityEngine;

namespace Assets._3Dimensions.Utilities.Editor
{
    public static class ChangeMaterialsFromUrpToStandard
    {
        [MenuItem("3Dimensions/Change URP materials back to Standard Shader")]
        static void ChangeUrpErrorShadersToStandard()
        {
            ChangeShaders(Shader.Find("Hidden/InternalErrorShader"), Shader.Find("Standard"));
        }

        static Object[] GetSelectedMaterials()
        {
            return Selection.GetFiltered(typeof(Material), SelectionMode.DeepAssets);
        }
    
        static void ChangeShaders(Shader oldShader, Shader newShader)
        {
            int counter = 0;
            int progress = 0;
            if (Selection.objects.Length > 0)
            {
                Object[] materiales = GetSelectedMaterials();
                if (materiales.Length > 0)
                {
                    foreach( Material mat in materiales )
                    {
                        if (mat.shader == oldShader)
                        {
                            mat.shader = newShader;
                            counter++;
                        }
                        progress++;
                        EditorUtility.DisplayProgressBar("Converting materials...",
                            "Found (" + progress + "/" + materiales.Length + ") prefabs.", (float)materiales.Length / progress);     
                    } 
                }
            }
            EditorUtility.ClearProgressBar();

            EditorUtility.DisplayDialog( "Message", "materials changed: " + counter, "OK" );
        }
    }
}
