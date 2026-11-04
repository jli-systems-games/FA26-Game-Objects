using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using TMPro;

[RequireComponent(typeof(CharacterController))]
public class MuseumVisitorController : MonoBehaviour
{
    public Camera worldCamera;
    public TMP_Text controlsText;
    public float walkSpeed=3.5f, runSpeed=5.5f, sensitivity=0.1f;
    public bool firstPerson=true;
    CharacterController body;
    Animator animator;
    Renderer[] visuals;
    Vector3 spawn;
    float yaw,pitch,verticalSpeed,animationSpeed;
    bool locked;
    public bool IsGrounded => body != null && body.isGrounded;
#if UNITY_EDITOR
    [System.NonSerialized] public Vector2 testMovement;
#endif
    void Awake()
    {
        body=GetComponent<CharacterController>();animator=GetComponent<Animator>();
        visuals=GetComponentsInChildren<Renderer>();spawn=transform.position;
        if(worldCamera==null)worldCamera=Camera.main;
        yaw=transform.eulerAngles.y;
        if(animator!=null){animator.applyRootMotion=false;animator.cullingMode=AnimatorCullingMode.AlwaysAnimate;}
        ApplyView();
    }
    void Start(){UpdateCamera();UpdateHint();}
    void SetLocked(bool value)
    { locked=value;Cursor.lockState=value?CursorLockMode.Locked:CursorLockMode.None;Cursor.visible=!value;UpdateHint(); }
    void OnDisable(){Cursor.lockState=CursorLockMode.None;Cursor.visible=true;}
    void OnApplicationFocus(bool focus){if(!focus)SetLocked(false);}
    public void ToggleView(){firstPerson=!firstPerson;ApplyView();UpdateCamera();UpdateHint();}
    void ApplyView()
    {
        if(visuals==null)return;
        foreach(var r in visuals)r.shadowCastingMode=firstPerson?ShadowCastingMode.ShadowsOnly:ShadowCastingMode.On;
        if(worldCamera!=null)worldCamera.fieldOfView=firstPerson?66:60;
    }
    void UpdateHint()
    {
        if(controlsText==null)return;
        controlsText.text=(locked?"":"CLICK TO EXPLORE  |  ")+
            (firstPerson?"FIRST PERSON":"THIRD PERSON")+
            "\nWASD / ARROWS  MOVE     MOUSE  LOOK     SHIFT  RUN     V  VIEW     ESC  CURSOR";
    }
    void Update()
    {
        var keys=Keyboard.current;var mouse=Mouse.current;
        if(keys!=null && keys.escapeKey.wasPressedThisFrame)SetLocked(false);
        else if(mouse!=null && mouse.leftButton.wasPressedThisFrame && !locked)SetLocked(true);
        if(keys!=null && keys.vKey.wasPressedThisFrame)ToggleView();
        if(locked && mouse!=null && Cursor.lockState==CursorLockMode.Locked){
            Vector2 delta=Vector2.ClampMagnitude(mouse.delta.ReadValue(),300);
            yaw+=delta.x*sensitivity;pitch=Mathf.Clamp(pitch-delta.y*sensitivity,-65,70);
        }
        Vector2 input=Vector2.zero;bool sprint=false;
        if(locked && keys!=null){
            input.x=(keys.dKey.isPressed||keys.rightArrowKey.isPressed?1:0)-(keys.aKey.isPressed||keys.leftArrowKey.isPressed?1:0);
            input.y=(keys.wKey.isPressed||keys.upArrowKey.isPressed?1:0)-(keys.sKey.isPressed||keys.downArrowKey.isPressed?1:0);
            sprint=keys.leftShiftKey.isPressed||keys.rightShiftKey.isPressed;
        }
#if UNITY_EDITOR
        if(testMovement.sqrMagnitude>0)input=testMovement;
#endif
        input=Vector2.ClampMagnitude(input,1);
        Vector3 move=Quaternion.Euler(0,yaw,0)*new Vector3(input.x,0,input.y);
        float speed=sprint?runSpeed:walkSpeed;
        if(firstPerson)transform.rotation=Quaternion.Euler(0,yaw,0);
        else if(move.sqrMagnitude>0.01f)transform.rotation=Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(move),1-Mathf.Exp(-14*Time.deltaTime));
        if(body.isGrounded && verticalSpeed<0)verticalSpeed=-2;
        verticalSpeed=Mathf.Max(verticalSpeed-20*Time.deltaTime,-35);
        body.Move((move*speed+Vector3.up*verticalSpeed)*Time.deltaTime);
        if(transform.position.y<-5){body.enabled=false;transform.position=spawn;body.enabled=true;verticalSpeed=-2;}
        if(animator!=null){
            animationSpeed=Mathf.MoveTowards(animationSpeed,input.magnitude*speed,12*Time.deltaTime);
            animator.SetFloat("Speed",animationSpeed);animator.SetFloat("MotionSpeed",1);
            animator.SetBool("Grounded",body.isGrounded);animator.SetBool("Jump",false);animator.SetBool("FreeFall",!body.isGrounded && verticalSpeed<-4);
        }
    }
    void LateUpdate(){UpdateCamera();}
    void UpdateCamera()
    {
        if(worldCamera==null)return;
        Quaternion rotation=Quaternion.Euler(pitch,yaw,0);
        Vector3 pivot=transform.position+Vector3.up*(firstPerson?1.65f:1.45f);
        Vector3 position=pivot;
        if(!firstPerson){
            Vector3 offset=rotation*new Vector3(0.4f,0.25f,-3.2f);
            float distance=offset.magnitude;
            foreach(var hit in Physics.SphereCastAll(pivot,0.18f,offset.normalized,distance,~0,QueryTriggerInteraction.Ignore)){
                if(hit.collider.transform==transform || hit.collider.transform.IsChildOf(transform))continue;
                distance=Mathf.Min(distance,Mathf.Max(0.15f,hit.distance-0.08f));
            }
            position=pivot+offset.normalized*distance;
        }
        worldCamera.transform.SetPositionAndRotation(position,rotation);
    }
    // Animation clips contain these events; keep receivers on the new controller.
    void OnFootstep(AnimationEvent e){}
    void OnLand(AnimationEvent e){}
}
