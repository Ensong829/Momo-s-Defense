using System.IO;
using System;
using UnityEditor;
using UnityEngine;

namespace MomosDefense.Editor
{
    public static class PrototypeAndroidBuilder
    {
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
                scenes = new[] { ScenePath },
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
    }
}
