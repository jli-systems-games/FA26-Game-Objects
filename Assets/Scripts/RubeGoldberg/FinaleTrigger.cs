using UnityEngine;
namespace RubeGoldberg {
public class FinaleTrigger : MonoBehaviour {
 public GameObject requiredActivator;public MachineGameManager manager;bool fired;
 void OnTriggerEnter(Collider other){if(fired||!CameraTrigger.Matches(other,requiredActivator))return;fired=true;manager.Complete();}
}
}
