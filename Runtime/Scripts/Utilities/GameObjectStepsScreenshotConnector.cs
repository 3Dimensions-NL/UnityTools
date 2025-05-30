using System.Collections;
using UnityEngine;
namespace _3Dimensions.Tools.Runtime.Scripts.Utilities
{
    public class GameObjectStepsScreenshotConnector : MonoBehaviour
    {
        [SerializeField] private GameObjectSteps gameObjectSteps;
        [SerializeField] private Screenshot screenshot;
        [SerializeField] private float delayBeforeScreenshot = 0.5f;
        
        private int _screenshotsTaken;

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
#if UNITY_EDITOR
                UnityEditor.EditorUtility.ClearProgressBar();
#endif
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

#if UNITY_EDITOR
            UnityEditor.EditorUtility.DisplayProgressBar(
                "Taking screenshots", 
                "Screenshot " + (_screenshotsTaken + 1) + "/" + gameObjectSteps.steps.Length, 
                (float)(_screenshotsTaken + 1) / (float) gameObjectSteps.steps.Length);
#endif

            yield return new WaitForSeconds(delayBeforeScreenshot);
            
            screenshot.useDate = false;
            screenshot.fileName = gameObject.name;
            screenshot.fileSuffix = " (" + (_screenshotsTaken + 1).ToString("D3") + ")";
            screenshot.TakeScreenshot();
        }
    }
}
