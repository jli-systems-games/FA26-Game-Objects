using UnityEngine;
namespace RubeGoldberg {
public class PendulumRelease : MonoBehaviour {
 public GameObject requiredActivator; public Rigidbody pendulum; public MachineGameManager manager; bool fired;
 void OnTriggerEnter(Collider other){if(fired||!CameraTrigger.Matches(other,requiredActivator))return;fired=true;pendulum.isKinematic=false;pendulum.WakeUp();manager.SetStage(6);}
}
}
