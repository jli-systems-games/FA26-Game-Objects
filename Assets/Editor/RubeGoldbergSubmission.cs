using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Build.Reporting;
using UnityEngine;
public static class RubeGoldbergSubmission
{
 public static void Build()
 {
  var scene=EditorSceneManager.OpenScene(RubeGoldbergSceneBuilder.ScenePath);
  var root=scene.GetRootGameObjects().First(g=>g.name=="RubeGoldbergMachine");
  RubeGoldbergSceneValidation.Check(root.transform);
  EditorBuildSettings.scenes=new[]{new EditorBuildSettingsScene(scene.path,true)};
  PlayerSettings.WebGL.compressionFormat=WebGLCompressionFormat.Disabled;
  PlayerSettings.WebGL.threadsSupport=false;
  PlayerSettings.WebGL.initialMemorySize=128;
  PlayerSettings.productName="Week 3 Rube Goldberg Machine";
  AssetDatabase.SaveAssets();
  var args=Environment.GetCommandLineArgs();var i=Array.IndexOf(args,"-submissionOutput");
  var output=i>=0?args[i+1]:"Builds/Week3Web";
  var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=new[]{scene.path},locationPathName=output,target=BuildTarget.WebGL,options=BuildOptions.None});
  Directory.CreateDirectory("Logs");
  File.WriteAllText("Logs/SubmissionBuild.txt",report.summary.result+"\nErrors: "+report.summary.totalErrors+"\nBytes: "+report.summary.totalSize);
  if(report.summary.result!=BuildResult.Succeeded)throw new Exception("Submission build failed");
  Debug.Log("[Submission] BUILD PASS");
 }
}
