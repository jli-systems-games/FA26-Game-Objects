using UnityEngine;
using System.Collections;
namespace RubeGoldberg {
public class FinalBallGateRelease : MonoBehaviour {
 public GameObject requiredActivator;public Collider gate;bool fired;
 public bool HasLaunched {get;private set;}
 void OnTriggerEnter(Collider other){if(fired||!CameraTrigger.Matches(other,requiredActivator))return;fired=true;if(gate)StartCoroutine(FlyAway());Debug.Log("[Machine] FinalBall gate released");}
 IEnumerator FlyAway(){
  HasLaunched=true;
  foreach(var collider in gate.GetComponentsInChildren<Collider>())collider.enabled=false;
  var target=gate.transform;var start=target.position;var rotation=target.rotation;
  // A cosmetic flight clears the ball immediately without introducing new collisions.
  for(float t=0;t<2.5f;t+=Time.deltaTime){
   target.position=start+new Vector3(3.5f*t,9f*t+2f*t*t,2f*t);
   target.rotation=rotation*Quaternion.Euler(240*t,360*t,540*t);
   yield return null;
  }
  foreach(var visual in gate.GetComponentsInChildren<Renderer>())visual.enabled=false;
  Debug.Log("[Machine Effect] Final gate flew offscreen");
 }
}
}

