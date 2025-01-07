using _3Dimensions.Tools.Runtime.Scripts.Utilities;
using UnityEditor;
using UnityEngine;

namespace _3Dimensions.Tools.Editor.Scripts.Utilities
{
    [CustomEditor(typeof(Screenshot))]
    public class ScreenshotEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // Referentie naar Screenshot component
            Screenshot screenshot = (Screenshot)target;

            // Start de custom UI rendering
            EditorGUILayout.LabelField("General Settings", EditorStyles.boldLabel);

            // SuperSize veld
            screenshot.superSize = EditorGUILayout.IntField("Super Size", screenshot.superSize);

            // Path veld met inline Set Path knop
            EditorGUILayout.BeginHorizontal(); // Begin van een horizontale lijn
            screenshot.path = EditorGUILayout.TextField("Path", screenshot.path);
            if (GUILayout.Button("Set Path", GUILayout.MaxWidth(80)))
            {
                screenshot.SetPath();
            }

            EditorGUILayout.EndHorizontal(); // Einde horizontale lijn

            // Filename settings
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("File Name Settings", EditorStyles.boldLabel);
            screenshot.fileName = EditorGUILayout.TextField("File Name", screenshot.fileName);
            screenshot.fileSuffix = EditorGUILayout.TextField("File Suffix", screenshot.fileSuffix);
            screenshot.useDate = EditorGUILayout.Toggle("Use Date", screenshot.useDate);

            EditorGUILayout.Space();
            screenshot.openFileAfterCreation =
                EditorGUILayout.Toggle("Open File After Creation", screenshot.openFileAfterCreation);

            // Separator voor extra functies
            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("Screenshot Controls", EditorStyles.boldLabel);

            // Screenshot controles (knoppen)
            if (GUILayout.Button("Take Screenshot"))
            {
                screenshot.TakeScreenshot();
            }

            if (GUILayout.Button("Open Last Screenshot"))
            {
                screenshot.OpenLastScreenshot();
            }

            if (GUILayout.Button("Open Screenshot Folder"))
            {
                screenshot.OpenScreenshotFolder();
            }

            // Zet veranderingen als "Dirty" indien wijzigingen zijn gemaakt in de Inspector
            if (GUI.changed)
            {
                EditorUtility.SetDirty(screenshot);
            }
        }
    }
}