using UnityEngine;
namespace RubeGoldberg {
public class CameraTrigger : MonoBehaviour {
 public GameObject requiredActivator; public Camera cameraToDisable,cameraToEnable; public AudioListener listenerToDisable,listenerToEnable; public bool hasTriggered;
 public static bool Matches(Collider other,GameObject expected){return expected && (other.gameObject==expected || (other.attachedRigidbody && other.attachedRigidbody.gameObject==expected));}
 void OnTriggerEnter(Collider other){if(hasTriggered || !Matches(other,requiredActivator))return;hasTriggered=true;if(cameraToDisable)cameraToDisable.enabled=false;if(listenerToDisable)listenerToDisable.enabled=false;if(cameraToEnable)cameraToEnable.enabled=true;if(listenerToEnable)listenerToEnable.enabled=true;Debug.Log("[Machine] "+cameraToDisable.name+" -> "+cameraToEnable.name);}
}
}
