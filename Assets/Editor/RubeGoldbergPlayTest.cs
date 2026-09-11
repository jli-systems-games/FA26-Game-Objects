using UnityEditor;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using RubeGoldberg;
[InitializeOnLoad]
public static class RubeGoldbergPlayTest {
 static double started;static bool sawComplete;static int lastStage;static double lastReport;
 static RubeGoldbergPlayTest(){EditorApplication.update+=Tick;}
 public static void NaturalRobustness(){SessionState.SetBool("RGNoAssist",true);Robustness();}
 public static void Robustness(){SessionState.SetFloat("RGStep",1f/60f);SessionState.SetInt("RGRuns",2);Run();}
 public static void RebuildAndRun(){AssetDatabase.DeleteAsset(RubeGoldbergSceneBuilder.ScenePath);RubeGoldbergSceneBuilder.Build();Run();}
 public static void Run(){EditorSceneManager.OpenScene(RubeGoldbergSceneBuilder.ScenePath);SessionState.SetBool("RGTest",true);EditorApplication.EnterPlaymode();}
 static void Tick(){if(!SessionState.GetBool("RGTest",false)||!EditorApplication.isPlaying)return;if(started==0)started=EditorApplication.timeSinceStartup;var m=Object.FindAnyObjectByType<MachineGameManager>();if(!m)return;if(SessionState.GetBool("RGNoAssist",false)){var failsafe=Object.FindAnyObjectByType<MachineFailsafe>();if(failsafe)failsafe.enabled=false;}Time.timeScale=5;Time.fixedDeltaTime=SessionState.GetFloat("RGStep",.02f);
 if(m.CurrentStage!=lastStage){lastStage=m.CurrentStage;Debug.Log("[PlayTest] Observed stage "+lastStage);}
 if(EditorApplication.timeSinceStartup-lastReport>3){lastReport=EditorApplication.timeSinceStartup;Debug.Log("[PlayTest] t="+Time.time+" stage="+m.CurrentStage+" player="+m.playerBall.position+" lever="+m.leverBall.position+" slider="+m.slider.position+" final="+m.finalBall.position+" domino angles="+string.Join(",",m.dominoes.Select(d=>d.rotation.eulerAngles.z.ToString("F0"))));}
 if(m.MachineComplete&&!sawComplete){int switches=0;foreach(var t in Object.FindObjectsByType<CameraTrigger>())if(t.hasTriggered)switches++;if(Object.FindObjectsByType<PhysicsContactEvidence>().Length!=3||!m.particles.isPlaying||Object.FindObjectsByType<AudioListener>().Count(l=>l.enabled)!=1||Object.FindObjectsByType<PhysicsContactEvidence>().Any(e=>!e.HasCollided)||switches!=3||!m.completeText.gameObject.activeSelf||!m.finaleLight.enabled){Debug.LogError("[PlayTest] FAIL: finale/camera validation");SessionState.SetBool("RGTest",false);EditorApplication.Exit(3);return;}Debug.Log("[PlayTest] Three camera switches, completion text and finale light verified");sawComplete=true;}
 if(sawComplete&&!m.MachineComplete&&m.CurrentStage==1){Debug.Log("[PlayTest] PASS: finale and automatic scene restart observed; fixed step="+Time.fixedDeltaTime);if(SessionState.GetInt("RGRuns",1)>1){SessionState.SetInt("RGRuns",1);SessionState.SetFloat("RGStep",.025f);sawComplete=false;return;}SessionState.SetBool("RGTest",false);EditorApplication.Exit(0);}
 if(Time.time>100||EditorApplication.timeSinceStartup-started>100){Debug.LogError("[PlayTest] FAIL: timeout stage "+m.CurrentStage);SessionState.SetBool("RGTest",false);EditorApplication.Exit(2);}
 }
}






