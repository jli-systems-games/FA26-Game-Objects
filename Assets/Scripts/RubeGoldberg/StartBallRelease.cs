using System.Collections;
using UnityEngine;
namespace RubeGoldberg {
public class StartBallRelease : MonoBehaviour {
 public MachineGameManager manager; Rigidbody body;
 void Awake(){body=GetComponent<Rigidbody>();body.constraints=RigidbodyConstraints.FreezeAll;}
 IEnumerator Start(){yield return new WaitForSeconds(1);body.constraints=RigidbodyConstraints.None;body.WakeUp();manager.SetStage(1);Debug.Log("[Machine] PlayerBall released");}
}
}
