using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using RubeGoldberg;
[InitializeOnLoad]
public static class RubeGoldbergSceneBuilder {
 public const string ScenePath="Assets/Scenes/RubeGoldbergMachine.unity";
 static Transform root;static Transform[] stages;static Material[] colors;static PhysicsMaterial ballMat,slideMat;static MachineGameManager manager;
 static RubeGoldbergSceneBuilder(){EditorApplication.delayCall+=AutoBuild;}
 static void EnsureIncluded(){var entries=EditorBuildSettings.scenes.ToList();if(entries.Count>0&&entries[0].path==ScenePath&&entries[0].enabled)return;entries.RemoveAll(s=>s.path==ScenePath);entries.Insert(0,new EditorBuildSettingsScene(ScenePath,true));EditorBuildSettings.scenes=entries.ToArray();}
 static void AutoBuild(){
  if(EditorApplication.isPlayingOrWillChangePlaymode||EditorApplication.isCompiling)return;
  if(AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePath)){EnsureIncluded();return;}
  if(SessionState.GetBool("RubeGoldbergAutoBuildAttempted",false))return;
  if(!Application.isBatchMode&&SceneManager.GetActiveScene().path==string.Empty)return;
  SessionState.SetBool("RubeGoldbergAutoBuildAttempted",true);
  try{Build();}catch(Exception e){Debug.LogError("[Machine Builder] Automatic build stopped safely: "+e.Message);}
 }
 [MenuItem("Tools/Rube Goldberg/Build Machine")]
 public static void Build(){
 if(EditorApplication.isPlayingOrWillChangePlaymode)return;
 if(AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePath)){Debug.Log("[Machine Builder] Existing scene preserved. Delete the generated scene explicitly to rebuild.");return;}
 var previous=SceneManager.GetActiveScene();var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,Application.isBatchMode?NewSceneMode.Single:NewSceneMode.Additive);SceneManager.SetActiveScene(scene);
 System.IO.Directory.CreateDirectory("Assets/Scenes");System.IO.Directory.CreateDirectory("Assets/RubeGoldberg/Materials");
 colors=new Material[9];Color[] palette={new Color(.08f,.4f,.95f),new Color(.9f,.12f,.17f),new Color(1,.76f,.12f),new Color(.15f,.68f,.32f),new Color(1,.35f,.06f),new Color(.55f,.2f,.85f),new Color(.05f,.85f,.9f),new Color(.85f,.91f,.95f),new Color(.14f,.18f,.24f)};
 for(int i=0;i<colors.Length;i++){string p="Assets/RubeGoldberg/Materials/Color"+i+".mat";colors[i]=AssetDatabase.LoadAssetAtPath<Material>(p);if(!colors[i]){colors[i]=new Material(Shader.Find("Universal Render Pipeline/Lit"));AssetDatabase.CreateAsset(colors[i],p);}colors[i].color=palette[i];}
 ballMat=PM("Ball",.3f,.04f);slideMat=PM("Slider",.15f,0);
 root=new GameObject("RubeGoldbergMachine").transform;stages=new Transform[7];string[] ns={"Ramp","Domino","Lever","Drop","Slider","Pendulum","Finale"};for(int i=0;i<7;i++)stages[i]=Parent("Stage0"+(i+1)+"_"+ns[i],root);
 var systems=Parent("Systems",root);manager=new GameObject("MachineGameManager").AddComponent<MachineGameManager>();manager.transform.SetParent(systems,false);
 Box("Ground",root,new Vector3(21,-.25f,0),new Vector3(44,.5f,8),8);
 Ramp("Ramp01",0,new Vector3(3,1.6f,0),new Vector3(6,.35f,2.6f),-14);
 manager.playerBall=Ball("PlayerBall",0,new Vector3(.65f,2.8f,0),.75f,7);manager.playerBall.gameObject.AddComponent<StartBallRelease>().manager=manager;manager.playerBall.constraints=RigidbodyConstraints.FreezeAll;
 Box("Stage02Platform",stages[1],new Vector3(8,.15f,0),new Vector3(5.6f,.3f,3),1);
 Box("RampTransition",stages[0],new Vector3(6,.95f,0),new Vector3(.6f,.2f,2.6f),0);manager.dominoes=new Rigidbody[7];for(int i=0;i<7;i++){var d=Box("Domino0"+(i+1),stages[1],new Vector3(6.55f+.60f*i,1.051f,0),new Vector3(.32f,1.5f,.9f),1);manager.dominoes[i]=Body(d,.8f);}
 var lp=Pivot("LeverPivot",stages[2],new Vector3(12.45f,1.55f,0));var lever=Body(Box("LeverArm",stages[2],lp.position,new Vector3(3.6f,.25f,1.1f),2),1.5f);NaturalLeverFix.Configure(lever.gameObject);var lh=lever.gameObject.AddComponent<HingeJoint>();lh.connectedBody=lp;lh.axis=Vector3.forward;lh.useLimits=true;lh.limits=new JointLimits{min=-22,max=22};lh.useSpring=true;lh.spring=new JointSpring{spring=100,damper=3,targetPosition=22};lever.constraints=RigidbodyConstraints.FreezeAll; var scoop=Box("LeverLaunchSurface",stages[2],new Vector3(13.65f,1.83f,0),new Vector3(1.05f,.16f,1.1f),2);scoop.transform.rotation=Quaternion.Euler(0,0,-30);scoop.transform.SetParent(lever.transform,true);
 Box("LeverSupport",stages[2],new Vector3(12.45f,.35f,0),new Vector3(.35f,.7f,1),8);
 manager.leverBall=Ball("LeverBall",2,new Vector3(13.65f,2.34f,0),.72f,4);manager.leverBall.constraints=RigidbodyConstraints.FreezeAll;
 var lc=lever.gameObject.AddComponent<LeverContact>();lc.requiredDomino=manager.dominoes[6];lc.ball=manager.leverBall;lc.manager=manager;
 Ramp("CatchRamp",3,new Vector3(16.05f,1.45f,0),new Vector3(3.7f,.3f,3.2f),-7);
 Ramp("LowerCatchRamp",3,new Vector3(19.4f,.28f,0),new Vector3(3.4f,.12f,2.8f),-2);
 for(int side=-1;side<=1;side+=2)Box("LowerGuide"+side,stages[3],new Vector3(19.8f,.75f,side*.9f),new Vector3(4,1,.18f),3);Box("SliderTrack",stages[4],new Vector3(23.2f,.12f,0),new Vector3(5.5f,.25f,2.8f),4);
 manager.slider=Body(Box("SliderBlock",stages[4],new Vector3(22.25f,.83f,0),new Vector3(1.25f,1.15f,1.25f),4),1.75f);manager.slider.constraints=PhysicsValidator.SliderConstraints;manager.slider.GetComponent<Collider>().sharedMaterial=slideMat;
 for(int s=-1;s<=1;s+=2)Box("SliderRail"+s,stages[4],new Vector3(23.2f,.75f,s*1.5f),new Vector3(5.5f,1.1f,.2f),4);
 Box("SliderEndStop",stages[4],new Vector3(25.6f,.45f,0),new Vector3(.25f,.4f,1.4f),4);var pp=Pivot("PendulumPivot",stages[5],new Vector3(28,4,0));var rotation=Quaternion.Euler(0,0,-45);manager.pendulum=Body(Box("PendulumArm",stages[5],pp.position+rotation*new Vector3(0,-1.5f,0),new Vector3(.28f,3,.28f),5),1);manager.pendulum.transform.rotation=rotation;manager.pendulum.isKinematic=true;
 var ph=manager.pendulum.gameObject.AddComponent<HingeJoint>();ph.autoConfigureConnectedAnchor=false;ph.connectedBody=pp;ph.axis=Vector3.forward;ph.anchor=new Vector3(0,1.5f,0);ph.connectedAnchor=Vector3.zero;
 var weight=GameObject.CreatePrimitive(PrimitiveType.Sphere);weight.name="PendulumWeight";weight.transform.position=pp.position+rotation*new Vector3(0,-3,0);weight.transform.SetParent(manager.pendulum.transform,true);weight.GetComponent<Renderer>().sharedMaterial=colors[5];
 for(int s=-1;s<=1;s+=2)Box("PendulumSupport"+s,stages[5],new Vector3(28,2,s*1.7f),new Vector3(.2f,4,.2f),8);Box("PendulumCrossbar",stages[5],new Vector3(28,4,0),new Vector3(.25f,.25f,3.7f),8).GetComponent<Collider>().enabled=false;
 Box("FinalBallPocket",stages[5],new Vector3(29.75f,1.01f,0),new Vector3(.35f,.3f,2.6f),5);manager.finalBall=Ball("FinalBall",5,new Vector3(29.75f,1.552f,0),.78f,2);
 manager.finalBall.constraints=RigidbodyConstraints.FreezePositionZ;manager.playerBall.gameObject.AddComponent<PhysicsContactEvidence>().expectedBody=manager.dominoes[0];manager.leverBall.gameObject.AddComponent<PhysicsContactEvidence>().expectedBody=manager.slider;manager.finalBall.gameObject.AddComponent<PhysicsContactEvidence>().expectedBody=manager.pendulum;var gate=Box("FinalBallGate",stages[5],new Vector3(30.3f,1.7f,0),new Vector3(.18f,1.3f,2.2f),5);
 var pr=Trigger("PendulumReleaseTrigger",4,new Vector3(25,.9f,0),new Vector3(1,2,2.6f)).AddComponent<PendulumRelease>();pr.requiredActivator=manager.slider.gameObject;pr.pendulum=manager.pendulum;pr.manager=manager;
 var gr=Trigger("FinalBallGateTrigger",5,new Vector3(29.05f,1.3f,0),new Vector3(.18f,1.2f,1.4f)).AddComponent<FinalBallGateRelease>();gr.requiredActivator=manager.pendulum.gameObject;gr.gate=gate.GetComponent<Collider>();
 Ramp("FinalEntryRamp",6,new Vector3(31.75f,.88f,0),new Vector3(2.9f,.28f,2.6f),-5);
 Ramp("FinalRamp",6,new Vector3(35.25f,.57f,0),new Vector3(4.3f,.28f,2.6f),-5);
 var target=Box("FinalTarget",stages[6],new Vector3(38,.8f,0),new Vector3(1.6f,1.6f,1.6f),6);
 var ft=Trigger("FinalTrigger",6,new Vector3(37.7f,.9f,0),new Vector3(1.8f,2.2f,2.6f)).AddComponent<FinaleTrigger>();ft.requiredActivator=manager.finalBall.gameObject;ft.manager=manager;
 var cp=Parent("CameraSystem",root);Camera[] cameras=new Camera[4];Vector3[] pos={new Vector3(6,7.5f,-11.5f),new Vector3(15,7.2f,-11.5f),new Vector3(25.5f,7.5f,-12),new Vector3(34.5f,6.5f,-10.5f)};Vector3[] look={new Vector3(6,1,0),new Vector3(15.5f,1,0),new Vector3(26,1.6f,0),new Vector3(35,.8f,0)};string[] cn={"Camera01_Start","Camera02_LeverDrop","Camera03_SliderPendulum","Camera04_Finale"};for(int i=0;i<4;i++){var c=new GameObject(cn[i]);c.transform.SetParent(cp);c.transform.position=pos[i];c.transform.LookAt(look[i]);cameras[i]=c.AddComponent<Camera>();cameras[i].fieldOfView=i==3?52:55;cameras[i].enabled=i==0;cameras[i].clearFlags=CameraClearFlags.SolidColor;cameras[i].backgroundColor=new Color(.055f,.075f,.11f);c.AddComponent<AudioListener>().enabled=i==0;}
 CamTrigger(0,1,new Vector3(10.55f,1,0),new Vector3(.2f,2.6f,2.5f),manager.dominoes[6].gameObject,cameras);CamTrigger(1,3,new Vector3(20.2f,.9f,0),new Vector3(1.2f,2.4f,2.6f),manager.leverBall.gameObject,cameras);CamTrigger(2,5,new Vector3(31,.9f,0),new Vector3(1,2.2f,2.6f),manager.finalBall.gameObject,cameras);
 var light=new GameObject("Directional Light").AddComponent<Light>();light.transform.SetParent(root);light.type=LightType.Directional;light.intensity=1.4f;light.transform.rotation=Quaternion.Euler(45,-30,0);RenderSettings.ambientLight=new Color(.5f,.55f,.65f);
 var particles=new GameObject("FinaleParticles").AddComponent<ParticleSystem>();particles.transform.SetParent(stages[6]);particles.transform.position=new Vector3(38,2,0);var main=particles.main;main.playOnAwake=false;main.loop=false;main.duration=3;main.startColor=palette[6];main.startSpeed=4;main.startLifetime=2;main.startSize=.15f;particles.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);particles.GetComponent<ParticleSystemRenderer>().sharedMaterial=colors[6];manager.particles=particles;
 var fl=new GameObject("FinaleLight").AddComponent<Light>();fl.transform.SetParent(stages[6]);fl.transform.position=new Vector3(37,3,-1);fl.type=LightType.Point;fl.range=10;fl.intensity=6;fl.color=palette[6];fl.enabled=false;manager.finaleLight=fl;
 var canvas=new GameObject("FinaleCanvas",typeof(Canvas),typeof(CanvasScaler));canvas.transform.SetParent(systems);canvas.GetComponent<Canvas>().renderMode=RenderMode.ScreenSpaceOverlay;canvas.GetComponent<CanvasScaler>().uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize;canvas.GetComponent<CanvasScaler>().referenceResolution=new Vector2(1280,720);
 manager.status=UIText("StageStatus",canvas.transform,"AUTOMATIC PHYSICS MACHINE · STARTING",25,new Vector2(0,310),new Vector2(1200,70));manager.completeText=UIText("MachineCompleteText",canvas.transform,"MACHINE COMPLETE!",58,Vector2.zero,new Vector2(1200,160));manager.completeText.gameObject.SetActive(false);
 var validator=new GameObject("PhysicsValidator").AddComponent<PhysicsValidator>();validator.transform.SetParent(systems);validator.machineRoot=root;validator.manager=manager;var failsafe=new GameObject("MachineFailsafe").AddComponent<MachineFailsafe>();failsafe.transform.SetParent(systems);failsafe.manager=manager;
 RubeGoldbergSceneValidation.Check(root);EditorSceneManager.SaveScene(scene,ScenePath);var scenes=EditorBuildSettings.scenes.ToList();if(!scenes.Any(s=>s.path==ScenePath))scenes.Add(new EditorBuildSettingsScene(ScenePath,true));else scenes.First(s=>s.path==ScenePath).enabled=true;EditorBuildSettings.scenes=scenes.ToArray();EnsureIncluded();AssetDatabase.SaveAssets();System.IO.File.WriteAllText("Assets/RubeGoldberg/BuildComplete.txt","Scene generated successfully; existing scene is the persistent one-time marker.");Debug.Log("[Machine Builder] Scene saved and included in Build Settings");if(previous.IsValid()&&previous.isLoaded){SceneManager.SetActiveScene(previous);EditorSceneManager.CloseScene(scene,true);}
 }
 static PhysicsMaterial PM(string name,float friction,float bounce){string p="Assets/RubeGoldberg/Materials/"+name+".physicMaterial";var m=AssetDatabase.LoadAssetAtPath<PhysicsMaterial>(p);if(!m){m=new PhysicsMaterial(name);AssetDatabase.CreateAsset(m,p);}m.dynamicFriction=m.staticFriction=friction;m.bounciness=bounce;m.bounceCombine=PhysicsMaterialCombine.Minimum;return m;}
 static Transform Parent(string n,Transform p){var t=new GameObject(n).transform;t.SetParent(p,false);return t;}
 static GameObject Box(string n,Transform p,Vector3 pos,Vector3 scale,int color){var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=n;g.transform.SetParent(p,false);g.transform.position=pos;g.transform.localScale=scale;g.GetComponent<Renderer>().sharedMaterial=colors[color];return g;}
 static Rigidbody Body(GameObject g,float mass){if(g.GetComponent<BoxCollider>()){var size=g.transform.localScale;var mat=g.GetComponent<Renderer>().sharedMaterial;g.transform.localScale=Vector3.one;g.GetComponent<BoxCollider>().size=size;UnityEngine.Object.DestroyImmediate(g.GetComponent<MeshRenderer>());UnityEngine.Object.DestroyImmediate(g.GetComponent<MeshFilter>());var visual=GameObject.CreatePrimitive(PrimitiveType.Cube);visual.name="Visual";visual.transform.SetParent(g.transform,false);visual.transform.localScale=size;visual.GetComponent<Renderer>().sharedMaterial=mat;UnityEngine.Object.DestroyImmediate(visual.GetComponent<Collider>());}var b=g.AddComponent<Rigidbody>();b.mass=mass;b.linearDamping=.05f;b.angularDamping=.05f;b.collisionDetectionMode=CollisionDetectionMode.ContinuousDynamic;b.interpolation=RigidbodyInterpolation.Interpolate;b.solverIterations=16;b.solverVelocityIterations=8;return b;}
 static Rigidbody Ball(string n,int s,Vector3 p,float size,int color){var g=GameObject.CreatePrimitive(PrimitiveType.Sphere);g.name=n;g.transform.SetParent(stages[s],false);g.transform.position=p;g.transform.localScale=Vector3.one*size;g.GetComponent<Renderer>().sharedMaterial=colors[color];g.GetComponent<Collider>().sharedMaterial=ballMat;return Body(g,1);}
 static Rigidbody Pivot(string n,Transform p,Vector3 pos){var t=Parent(n,p);t.position=pos;var b=t.gameObject.AddComponent<Rigidbody>();b.isKinematic=true;b.useGravity=false;return b;}
 static void Ramp(string n,int stage,Vector3 pos,Vector3 scale,float angle){var g=Box(n,stages[stage],pos,scale,stage);g.transform.rotation=Quaternion.Euler(0,0,angle);for(int side=-1;side<=1;side+=2){var rail=Box(n+"Rail"+side,stages[stage],pos+new Vector3(0,.45f,side*(scale.z/2+.1f)),new Vector3(scale.x,.7f,.18f),stage);rail.transform.rotation=g.transform.rotation;}}
 static GameObject Trigger(string n,int s,Vector3 pos,Vector3 scale){var g=new GameObject(n);g.transform.SetParent(stages[s],false);g.transform.position=pos;g.AddComponent<BoxCollider>().size=scale;g.GetComponent<BoxCollider>().isTrigger=true;return g;}
 static void CamTrigger(int i,int s,Vector3 p,Vector3 size,GameObject activator,Camera[] cameras){var t=Trigger("CameraTrigger0"+(i+1),s,p,size).AddComponent<CameraTrigger>();t.requiredActivator=activator;t.cameraToDisable=cameras[i];t.cameraToEnable=cameras[i+1];t.listenerToDisable=cameras[i].GetComponent<AudioListener>();t.listenerToEnable=cameras[i+1].GetComponent<AudioListener>();}
 static Text UIText(string name,Transform parent,string value,int size,Vector2 pos,Vector2 dimensions){var g=new GameObject(name,typeof(RectTransform),typeof(CanvasRenderer),typeof(Text));g.transform.SetParent(parent,false);var t=g.GetComponent<Text>();t.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");t.text=value;t.fontSize=size;t.alignment=TextAnchor.MiddleCenter;t.color=Color.white;t.rectTransform.sizeDelta=dimensions;t.rectTransform.anchoredPosition=pos;return t;}
}














