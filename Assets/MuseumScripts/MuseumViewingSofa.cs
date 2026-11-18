using UnityEngine;
using UnityEngine.InputSystem;

public class MuseumViewingSofa : MonoBehaviour
{
    public Transform headRest, bodyRest, screen;
    public float useRange=2.6f;
    public bool IsReclining { get; private set; }
    MuseumVisitorController visitor;
    CharacterController body;
    Vector3 returnPosition;
    Quaternion returnRotation;
    bool controllerEnabled, visitorEnabled;
    float cameraFov, yaw, pitch;
    Animator[] animators;bool[] animatorStates;
    MuseumMenu menu;
    MuseumExhibitInteractor interactor;bool interactorEnabled;
    void Start(){visitor=FindFirstObjectByType<MuseumVisitorController>();menu=FindFirstObjectByType<MuseumMenu>();if(visitor){body=visitor.GetComponent<CharacterController>();interactor=visitor.GetComponent<MuseumExhibitInteractor>();}}
    bool CanUse => visitor && !visitor.inputLocked && visitor.cameraOverride==null && Vector3.Distance(visitor.transform.position,transform.position)<useRange;
    void Update()
    {
        if(!visitor || (menu && menu.MenuOpen))return;
        var keyboard=Keyboard.current;
        if(IsReclining){
            if(keyboard!=null && (keyboard.rKey.wasPressedThisFrame || keyboard.qKey.wasPressedThisFrame)){StandUp();return;}
            var mouse=Mouse.current;if(mouse!=null && Cursor.lockState==CursorLockMode.Locked){var delta=mouse.delta.ReadValue();yaw=Mathf.Clamp(yaw+delta.x*.07f,-25,25);pitch=Mathf.Clamp(pitch-delta.y*.07f,-18,18);}
        }else if(CanUse && keyboard!=null && keyboard.rKey.wasPressedThisFrame)Recline();
    }
    void LateUpdate()
    {
        if(!IsReclining || !visitor.worldCamera)return;
        var camera=visitor.worldCamera;camera.transform.position=headRest.position;
        camera.transform.rotation=Quaternion.LookRotation(screen.position-headRest.position,Vector3.up)*Quaternion.Euler(pitch,yaw,0);
        camera.fieldOfView=60;
    }
    public void Recline()
    {
        if(!CanUse || !headRest || !bodyRest || !screen)return;
        returnPosition=visitor.transform.position;returnRotation=visitor.transform.rotation;
        visitorEnabled=visitor.enabled;controllerEnabled=body.enabled;cameraFov=visitor.worldCamera.fieldOfView;
        if(interactor){interactorEnabled=interactor.enabled;interactor.enabled=false;}
        animators=visitor.GetComponentsInChildren<Animator>();animatorStates=new bool[animators.Length];
        for(int i=0;i<animators.Length;i++){animatorStates[i]=animators[i].enabled;animators[i].enabled=false;}
        visitor.inputLocked=true;visitor.cameraOverride=headRest;visitor.enabled=false;body.enabled=false;
        visitor.transform.SetPositionAndRotation(bodyRest.position,bodyRest.rotation);visitor.SetLocked(true);
        yaw=pitch=0;IsReclining=true;
    }
    public void StandUp()
    {
        if(!IsReclining || !visitor)return;
        IsReclining=false;visitor.transform.SetPositionAndRotation(returnPosition,returnRotation);
        visitor.cameraOverride=null;visitor.inputLocked=false;body.enabled=controllerEnabled;visitor.enabled=visitorEnabled;
        if(interactor)interactor.enabled=interactorEnabled;
        for(int i=0;i<animators.Length;i++)if(animators[i])animators[i].enabled=animatorStates[i];
        if(visitor.worldCamera)visitor.worldCamera.fieldOfView=cameraFov;visitor.SetLocked(true);
    }
    void OnGUI()
    {
        if((menu && menu.MenuOpen) || (!IsReclining && !CanUse))return;
        var style=new GUIStyle(GUI.skin.box){fontSize=22,alignment=TextAnchor.MiddleCenter};
        GUI.Box(new Rect(Screen.width*.5f-250,Screen.height-190,500,42),IsReclining?"[R / Q]  GET UP    ·    MOUSE  LOOK":"[R]  LIE DOWN ON SOFA",style);
    }
    void OnDisable(){StandUp();}
}
