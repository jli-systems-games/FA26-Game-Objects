using UnityEngine;
namespace RubeGoldberg {
// A physical domino contact releases the balanced lever latch. No scripted launch.
public class LeverContact : MonoBehaviour {
 public Rigidbody requiredDomino,ball;public MachineGameManager manager;bool fired;
 void OnCollisionEnter(Collision hit){if(fired||hit.rigidbody!=requiredDomino||manager.CurrentStage<2)return;fired=true;GetComponent<Rigidbody>().constraints=RigidbodyConstraints.None;ball.constraints=RigidbodyConstraints.FreezePositionZ;manager.SetStage(3);}
}
}

