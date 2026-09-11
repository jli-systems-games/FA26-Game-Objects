using UnityEngine;
namespace RubeGoldberg {
public class MachineFailsafe : MonoBehaviour {
 public MachineGameManager manager;bool[] used=new bool[8];float lastProgress;int stage;float metric;
 float Progress(){switch(manager.CurrentStage){case 1:return manager.playerBall.position.x;case 2:float v=0;foreach(var d in manager.dominoes)v+=Mathf.Abs(Mathf.DeltaAngle(0,d.rotation.eulerAngles.z))/90;return v;case 3:case 4:return manager.leverBall.position.x;case 5:return manager.slider.position.x;case 6:return manager.pendulum.transform.TransformPoint(new Vector3(0,-1.5f,0)).x;case 7:return manager.finalBall.position.x;default:return 0;}}
 void FixedUpdate(){int s=manager.CurrentStage;if(s==0||manager.MachineComplete)return;if(stage!=s){stage=s;lastProgress=Time.time;metric=Progress();}float p=Progress();if(p-metric>.15f){metric=p;lastProgress=Time.time;}if(used[s]||Time.time-lastProgress<7)return;used[s]=true;string action="";switch(s){case 1:manager.playerBall.AddForce(Vector3.right*1.5f,ForceMode.Impulse);action="PlayerBall +X impulse";break;case 2:foreach(var d in manager.dominoes)if(Vector3.Dot(d.transform.up,Vector3.up)>.8f){d.AddTorque(Vector3.back*1.4f,ForceMode.Impulse);action=d.name+" forward torque";break;}break;case 3:manager.leverBall.AddForce(new Vector3(2.2f,1.2f,0),ForceMode.Impulse);action="LeverBall launch assist";break;case 4:manager.leverBall.AddForce(Vector3.right*2,ForceMode.Impulse);action="LeverBall catcher assist";break;case 5:manager.slider.AddForce(Vector3.right*2.5f,ForceMode.Impulse);action="slider assist";break;case 6:manager.pendulum.AddTorque(Vector3.forward*1.5f,ForceMode.Impulse);action="pendulum torque";break;case 7:manager.finalBall.AddForce(Vector3.right*2,ForceMode.Impulse);action="FinalBall +X impulse";break;}Debug.Log("[Machine Failsafe] Stage "+s+" "+action+" applied");}
}
}


