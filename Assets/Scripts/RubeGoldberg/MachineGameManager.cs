using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace RubeGoldberg {
public class MachineGameManager : MonoBehaviour {
 public int CurrentStage {get; private set;} public bool MachineComplete {get; private set;}
 public Rigidbody playerBall, leverBall, slider, pendulum, finalBall; public Rigidbody[] dominoes;
 public Text status, completeText; public ParticleSystem particles; public Light finaleLight;
 public bool restartAutomatically = true; public float StageStartedAt {get; private set;}
 static readonly string[] names={"Ready","Ramp","Domino chain","Lever","Drop","Slider","Pendulum","Final run"};
 void Start(){if(slider){var effect=slider.GetComponent<SliderImpactEffect>();if(!effect)effect=slider.gameObject.AddComponent<SliderImpactEffect>();effect.pendulum=pendulum;}}
 public void SetStage(int stage) { if(stage<=CurrentStage || MachineComplete)return; CurrentStage=stage; StageStartedAt=Time.time; Debug.Log("[Machine] Stage "+stage+" - "+names[stage]+" started"); if(status)status.text="STAGE "+stage+" / 7   ·   "+names[stage].ToUpper(); }
 void FixedUpdate(){if(CurrentStage==1 && dominoes[0].angularVelocity.magnitude>.2f)SetStage(2); if(CurrentStage==3 && leverBall.position.x>17.8f && leverBall.position.y<1.7f)SetStage(4); if(CurrentStage==4 && slider.linearVelocity.x>.12f)SetStage(5); if(CurrentStage==6 && finalBall.linearVelocity.x>.2f)SetStage(7);}
 public void Complete(){if(MachineComplete)return; MachineComplete=true; Debug.Log("[Machine] MACHINE COMPLETE"); if(completeText)completeText.gameObject.SetActive(true); if(particles)particles.Play(); if(finaleLight)finaleLight.enabled=true; if(restartAutomatically)StartCoroutine(Restart());}
 IEnumerator Restart(){yield return new WaitForSeconds(5); SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);}
}
}
