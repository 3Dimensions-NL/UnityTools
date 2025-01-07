using System;
using System.Collections;
using UnityEngine;

namespace _3Dimensions.Tools.Runtime.Scripts.Utilities
{
    public class Screenshot : MonoBehaviour
    {
        public int superSize = 1;

        // Replace InlineButton with ContextMenu for setting path
        public string path = "";

        // Grouping: Replaced BoxGroup with Header for logical organization
        [Header("File Name Settings")] public string fileName = "Screenshot_";
        public string fileSuffix;
        public bool useDate;

        public bool openFileAfterCreation;

        // Replace HideInInspector with NonSerialized
        [NonSerialized] public string lastScreenshot = "";
        [NonSerialized] public bool renderRunning;

        public Action ScreenshotCreated;
        public Action ScreenshotFailed;

        [ContextMenu("Set Path")]
        public void SetPath()
        {
#if UNITY_EDITOR
            path = UnityEditor.EditorUtility.SaveFolderPanel("Path to Save Images", path, Application.dataPath);
            UnityEditor.EditorUtility.SetDirty(this);
#else
            path = Application.dataPath + "/screenshots";
#endif
            Debug.Log("Path Set to: " + path);
        }

        // Replace Button with ContextMenu
        [ContextMenu("Take Screenshot")]
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

        [ContextMenu("Open Last Screenshot")]
        public void OpenLastScreenshot()
        {
            if (lastScreenshot != "")
            {
                Application.OpenURL("file://" + lastScreenshot);
                Debug.Log("Opening File " + lastScreenshot);
            }
        }

        [ContextMenu("Open Screenshot Folder")]
        public void OpenScreenshotFolder()
        {
            Application.OpenURL("file://" + path);
        }

        private IEnumerator RenderScreenshot()
        {
            renderRunning = true;
            string filename = useDate
                ? path + "/" + fileName + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day +
                  "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second +
                  DateTime.Now.Millisecond + ".png"
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

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
}