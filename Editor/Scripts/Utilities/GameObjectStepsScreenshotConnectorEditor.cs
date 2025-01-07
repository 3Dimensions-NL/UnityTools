using _3Dimensions.Tools.Runtime.Scripts.Utilities;
using UnityEditor;
using UnityEngine;

namespace _3Dimensions.Tools.Editor.Scripts.Utilities
{
    [CustomEditor(typeof(GameObjectStepsScreenshotConnector))]
    public class GameObjectStepsScreenshotConnectorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // Draw the default inspector to retain existing variables
            DrawDefaultInspector();

            // Create a reference to the target script
            var screenshotConnector = (GameObjectStepsScreenshotConnector)target;

            // Add a button in the Inspector to trigger TakeScreenshotsFromSteps
            if (GUILayout.Button("Take Screenshots From Steps"))
            {
                // Check if the script is in play mode, since it uses Coroutines
                if (Application.isPlaying)
                {
                    // Call TakeScreenshotsFromSteps through reflection
                    var method = screenshotConnector.GetType().GetMethod("TakeScreenshotsFromSteps",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                    if (method != null)
                    {
                        method.Invoke(screenshotConnector, null);
                    }
                }
                else
                {
                    Debug.LogWarning("TakeScreenshotsFromSteps can only be executed in Play mode.");
                }
            }
        }
    }
}