using System.Collections;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
namespace _3Dimensions.Tools.Runtime.Scripts
{
    public class GameObjectStepsScreenshotConnector : MonoBehaviour
    {
        [SerializeField] private GameObjectSteps gameObjectSteps;
        [SerializeField] private Screenshot screenshot;
        [SerializeField] private float delayBeforeScreenshot = 0.5f;
        
        private int _screenshotsTaken;

        [Button]
        private void TakeScreenshotsFromSteps()
        {
            _screenshotsTaken = 0;
            StartCoroutine(TakeScreenshot());
        }
        private void ScreenshotCreated()
        {
            Debug.Log("Screenshot created: " + screenshot.lastScreenshot);

            if (_screenshotsTaken < gameObjectSteps.steps.Length - 1)
            {
                _screenshotsTaken++;
                StartCoroutine(TakeScreenshot());
            }
            else
            {
                StopCoroutine(TakeScreenshot());
                EditorUtility.ClearProgressBar();
            }
            
            screenshot.ScreenshotCreated -= ScreenshotCreated;
            screenshot.ScreenshotFailed -= ScreenshotFailed;
        }
        
        private void ScreenshotFailed()
        {

            screenshot.ScreenshotCreated -= ScreenshotCreated;
            screenshot.ScreenshotFailed -= ScreenshotFailed;
            
            StopCoroutine(TakeScreenshot());
        }

        private IEnumerator TakeScreenshot()
        {
            screenshot.ScreenshotCreated += ScreenshotCreated;
            screenshot.ScreenshotFailed += ScreenshotFailed;
            
            gameObjectSteps.ActivateStep(_screenshotsTaken);

            yield return new WaitForSeconds(delayBeforeScreenshot);
            
            screenshot.useDate = false;
            screenshot.fileName = gameObject.name;
            screenshot.fileSuffix = " (" + (_screenshotsTaken + 1).ToString("D3") + ")";
            screenshot.TakeScreenshot();
            
            EditorUtility.DisplayProgressBar("Taking screenshots", "Screenshot " + (_screenshotsTaken + 1) + "/" + gameObjectSteps.steps.Length, (float)_screenshotsTaken / (float) gameObjectSteps.steps.Length);
        }
    }
}
