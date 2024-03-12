#if UNITY_EDITOR
using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
namespace _3Dimensions.Tools.Runtime.Scripts
{
    public class Screenshot : MonoBehaviour
    {
        public int superSize = 1;
        [InlineButton(nameof(SetPath))] public string path = "";

        [BoxGroup("File name")] public string fileName = "Screenshot_";
        [BoxGroup("File name")] public string fileSuffix;
        [BoxGroup("File name")] public bool useDate;

        public bool openFileAfterCreation;
        
        [HideInInspector] public string lastScreenshot = "";
        [HideInInspector] public bool renderRunning;

        public Action ScreenshotCreated;
        public Action ScreenshotFailed;

        public void SetPath()
        {
            path = EditorUtility.SaveFolderPanel("Path to Save Images", path, Application.dataPath);
            EditorUtility.SetDirty(this);
            Debug.Log("Path Set");
        }

        [Button]
        public void TakeScreenshot()
        {
            if (renderRunning)
            {
                Debug.LogWarning("A screenshot is being rendered, try again later");
                return;
            }

            if (string.IsNullOrEmpty(path))
            {
                SetPath();
            }

            if (!string.IsNullOrEmpty(path))
            {
                StartCoroutine(RenderScreenshot());
            }
            else
            {
                Debug.LogError("Screenshot Path is not set!");
            }
        }

        [Button]
        public void OpenLastScreenshot()
        {
            if (lastScreenshot != "")
            {
                Application.OpenURL("file://" + lastScreenshot);
                Debug.Log("Opening File " + lastScreenshot);
            }
        }

        [Button]
        public void OpenScreenshotFolder()
        {
            Application.OpenURL("file://" + path);
        }

        IEnumerator RenderScreenshot()
        {
            renderRunning = true;
            string filename = useDate 
                ? path + "/" + fileName + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day +
                "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + DateTime.Now.Millisecond + ".png" 
                : path + "/" + fileName + fileSuffix + ".png";

            yield return new WaitForEndOfFrame();

            try
            {
                ScreenCapture.CaptureScreenshot(filename, superSize);
                lastScreenshot = filename;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                ScreenshotFailed?.Invoke();
            }
            
            yield return new WaitForEndOfFrame();
            renderRunning = false;

            if (openFileAfterCreation) OpenLastScreenshot();
            
            ScreenshotCreated?.Invoke();
            
            EditorUtility.SetDirty(this);
        }
    }
}
#endif
