using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Balcony telescope. Stand next to it and press E: the camera looks through the eyepiece at Earth with a narrow
// field of view (zoom) and a round "lens" mask. Move the mouse a little to look around. E or Esc steps back.
public class MuseumTelescope : MonoBehaviour
{
    public MuseumVisitorController visitor;
    public MuseumExhibitInteractor interactor;
    public Transform eye;            // camera position at the eyepiece
    public Transform target;         // Earth
    public float zoomFov=1.8f, range=2.2f, lookAround=1.5f;
    public GameObject prompt;        // "[E] LOOK THROUGH THE TELESCOPE"
    public GameObject view;          // lens mask + caption, shown while looking
    public RawImage lens;

    bool viewing; int enteredFrame; Vector2 offset;

    void Awake()
    {
        if(visitor==null)visitor=FindFirstObjectByType<MuseumVisitorController>();
        if(interactor==null&&visitor!=null)interactor=visitor.GetComponent<MuseumExhibitInteractor>();
        if(lens!=null)lens.texture=MakeLens();
        if(view!=null)view.SetActive(false);if(prompt!=null)prompt.SetActive(false);
    }

    void Update()
    {
        var keys=Keyboard.current;
        if(!viewing){
            bool near=visitor!=null && !visitor.inputLocked && Vector3.Distance(visitor.transform.position,transform.position)<range;
            if(prompt!=null)prompt.SetActive(near);
            if(near && keys!=null && keys.eKey.wasPressedThisFrame)Enter();
            return;
        }
        // Gentle look-around with the mouse, limited to a small cone around Earth.
        var mouse=Mouse.current;
        if(mouse!=null){offset+=mouse.delta.ReadValue()*0.01f;offset=Vector2.ClampMagnitude(offset,lookAround);}
        eye.rotation=Quaternion.LookRotation(target.position-eye.position)*Quaternion.Euler(-offset.y,offset.x,0);
        if(lens!=null){float a=(float)Screen.width/Screen.height;lens.uvRect=new Rect(-(a-1)/2f,0,a,1);} // keep the lens round
        if(Time.frameCount>enteredFrame && keys!=null && (keys.eKey.wasPressedThisFrame||keys.escapeKey.wasPressedThisFrame))Exit();
    }

    void Enter()
    {
        viewing=true;enteredFrame=Time.frameCount;offset=Vector2.zero;
        visitor.inputLocked=true;visitor.SetLocked(true);visitor.overrideFov=zoomFov;visitor.cameraOverride=eye;
        if(interactor!=null)interactor.enabled=false;
        if(prompt!=null)prompt.SetActive(false);if(view!=null)view.SetActive(true);
    }
    void Exit()
    {
        viewing=false;visitor.cameraOverride=null;visitor.inputLocked=false;
        if(interactor!=null)interactor.enabled=true;if(view!=null)view.SetActive(false);
    }

    // Black texture with a soft round hole in the middle.
    static Texture2D MakeLens()
    {
        const int S=256;var t=new Texture2D(S,S,TextureFormat.RGBA32,false){wrapMode=TextureWrapMode.Clamp};
        for(int y=0;y<S;y++)for(int x=0;x<S;x++){
            float d=Vector2.Distance(new Vector2(x,y),new Vector2(S/2f,S/2f))/(S/2f);
            t.SetPixel(x,y,new Color(0,0,0,Mathf.SmoothStep(0,1,(d-0.86f)/0.1f)));
        }
        t.Apply();return t;
    }
}
