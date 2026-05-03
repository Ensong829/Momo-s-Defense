using System.IO;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MomosDefense.Editor
{
    public static class PrototypeAndroidBuilder
    {
        private const string MainMenuScenePath = "Assets/_MomosDefense/Scenes/MainMenu.unity";
        private const string LevelSelectScenePath = "Assets/_MomosDefense/Scenes/LevelSelect.unity";
        private const string ScenePath = "Assets/_MomosDefense/Scenes/Prototype_MomoDefense.unity";
        private const string BuildPath = "Builds/Android/MomosDefensePrototype.apk";

        [MenuItem("Momo's Defense/Build Android Prototype")]
        public static void BuildAndroidPrototype()
        {
            string directory = Path.GetDirectoryName(BuildPath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            BuildPlayerOptions options = new BuildPlayerOptions
            {
                scenes = ResolveBuildScenes(),
                locationPathName = BuildPath,
                target = BuildTarget.Android,
                options = BuildOptions.Development
            };

            BuildReportSummary(BuildPipeline.BuildPlayer(options));
        }

        private static void BuildReportSummary(UnityEditor.Build.Reporting.BuildReport report)
        {
            if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                throw new InvalidOperationException($"Android prototype build failed: {report.summary.result}");
            }

            Debug.Log($"Android prototype build succeeded: {report.summary.outputPath}");
        }

        private static string[] ResolveBuildScenes()
        {
            List<string> scenes = new List<string>();

            TryAddScene(scenes, MainMenuScenePath);
            TryAddScene(scenes, LevelSelectScenePath);
            TryAddScene(scenes, ScenePath);

            return scenes.ToArray();
        }

        private static void TryAddScene(List<string> scenes, string scenePath)
        {
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath) != null)
            {
                scenes.Add(scenePath);
            }
        }
    }
}
