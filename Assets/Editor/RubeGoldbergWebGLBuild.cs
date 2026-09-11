using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
public static class RubeGoldbergWebGLBuild
{
    public static void Run()
    {
        RubeGoldbergRenderViews.Run();
        var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions {
            scenes = new[] { RubeGoldbergSceneBuilder.ScenePath },
            locationPathName = "Builds/RubeGoldbergWebGL",
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        });
        Directory.CreateDirectory("Logs");
        File.WriteAllText("Logs/RubeWebGLSummary.txt", report.summary.result + "\nErrors: " + report.summary.totalErrors + "\nBytes: " + report.summary.totalSize);
        if (report.summary.result != BuildResult.Succeeded) throw new System.Exception("WebGL build failed: " + report.summary.result);
        Debug.Log("[Machine WebGL] Build succeeded");
    }
}

