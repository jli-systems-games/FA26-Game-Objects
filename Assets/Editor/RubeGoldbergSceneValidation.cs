using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using RubeGoldberg;
public static class RubeGoldbergSceneValidation {
 public static void Check(Transform root) {
  Action<bool,string> require=(ok,message)=>{if(!ok)throw new InvalidOperationException("[Machine Validation] "+message);};
  var transforms=root.GetComponentsInChildren<Transform>(true);Func<string,Transform> find=n=>transforms.FirstOrDefault(t=>t.name==n);
  require(root.position==Vector3.zero&&root.rotation==Quaternion.identity&&root.localScale==Vector3.one,"Root identity");
  var stages=transforms.Where(t=>t.name.StartsWith("Stage0")&&!t.name.Contains("Platform")).ToArray();require(stages.Length==7,"Seven stages");foreach(var t in stages)require(t.position==Vector3.zero&&t.rotation==Quaternion.identity&&t.localScale==Vector3.one,"Stage parent identity");
  foreach(string n in new[]{"PlayerBall","LeverBall","FinalBall"})require(find(n)&&find(n).GetComponent<Rigidbody>()&&find(n).GetComponent<SphereCollider>(),n+" physics");
  var dominoes=transforms.Where(t=>t.name.StartsWith("Domino")).ToArray();require(dominoes.Length==7,"Seven dominoes");foreach(var d in dominoes)require(d.GetComponent<Rigidbody>()&&d.GetComponent<BoxCollider>(),"Domino physics");
  Physics.SyncTransforms();for(int i=0;i<6;i++)require(!dominoes[i].GetComponent<Collider>().bounds.Intersects(dominoes[i+1].GetComponent<Collider>().bounds),"Domino overlap");
  foreach(string n in new[]{"LeverArm","PendulumArm"}){var h=find(n).GetComponent<HingeJoint>();require(h&&h.connectedBody&&h.axis==Vector3.forward,n+" hinge");require(Vector3.Distance(h.transform.TransformPoint(h.anchor),h.connectedBody.transform.TransformPoint(h.connectedAnchor))<.01f,n+" anchor alignment");}
  require(find("PendulumArm").GetComponent<HingeJoint>().anchor.y==1.5f,"Pendulum top anchor");require(!find("PendulumWeight").GetComponent<Rigidbody>(),"Compound pendulum");
  require(find("SliderBlock").GetComponent<Rigidbody>().constraints==PhysicsValidator.SliderConstraints,"Slider X freedom");require(!find("FinalBall").GetComponent<Rigidbody>().isKinematic,"Dynamic final ball");
  var cameras=root.GetComponentsInChildren<Camera>();require(cameras.Length==4&&cameras.Count(c=>c.enabled)==1&&cameras[0].enabled,"Four cameras, first active");require(root.GetComponentsInChildren<AudioListener>().Count(l=>l.enabled)==1,"Single listener");
  var ct=root.GetComponentsInChildren<CameraTrigger>();require(ct.Length==3,"Three camera triggers");foreach(var t in ct)require(t.requiredActivator&&t.cameraToDisable&&t.cameraToEnable&&t.listenerToDisable&&t.listenerToEnable&&t.GetComponent<Collider>().isTrigger,"Camera trigger references");
  foreach(string n in new[]{"PendulumReleaseTrigger","FinalBallGateTrigger","FinalTrigger"})require(find(n)&&find(n).GetComponent<Collider>().isTrigger,n);
  foreach(var t in transforms)require(GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject)==0,"Missing script "+t.name);
  var m=root.GetComponentInChildren<MachineGameManager>();require(m&&m.completeText&&m.particles&&m.finaleLight&&m.restartAutomatically,"Finale and restart");
  Debug.Log("[Machine Validation] PASS: hierarchy, rigidbodies, domino spacing, hinges, slider constraints, cameras, filtered triggers, finale, references and missing scripts");
 }
}

