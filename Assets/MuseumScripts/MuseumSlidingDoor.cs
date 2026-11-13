using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

// The spaceship hatch at the start of the game.
// When the visitor stands close and presses SPACE, the door panel slides sideways and stays open.
public class MuseumSlidingDoor : MonoBehaviour
{
    public Transform door;            // the panel that moves (has a collider, so it blocks until open)
    public Vector3 openOffset=new Vector3(1.7f,0,0); // local slide distance
    public float range=2.5f;
    public TMP_Text prompt;           // "[E] OPEN HATCH", hidden when far away or open
    public GameObject promptRoot;     // optional: the panel around the prompt (shown/hidden instead of just the text)
    public Transform visitor;
    public GameObject welcomeUI;      // screen-space welcome text (sharp, not pixelated), shown inside the ship until the hatch opens
    public float welcomeRange=6f;

    bool open;
    float t;
    Vector3 closedPos;
    AudioSource sound;
    AudioClip hiss;
    MuseumVisitorController controller;

    void Awake()
    {
        if(visitor==null){var v=FindFirstObjectByType<MuseumVisitorController>();if(v!=null)visitor=v.transform;}
        if(visitor!=null)controller=visitor.GetComponent<MuseumVisitorController>();
        closedPos=door.localPosition;
        sound=gameObject.AddComponent<AudioSource>();sound.playOnAwake=false;sound.spatialBlend=0.7f;
        if(prompt!=null)prompt.text="<b>[SPACE]</b>  OPEN HATCH";
        hiss=MuseumExhibitInteractor.Tone("Hatch",new[]{220f,330f,440f},0.12f,0.25f);
    }

    void Update()
    {
        // Ignore the hatch while a menu is open (the visitor's input is locked then).
        bool near=visitor!=null && Vector3.Distance(visitor.position,transform.position)<range && (controller==null||!controller.inputLocked);
        var p=promptRoot!=null?promptRoot:(prompt!=null?prompt.gameObject:null);
        if(p!=null)p.SetActive(near && !open);
        if(welcomeUI!=null){
            bool inside=visitor!=null && !open && Vector3.Distance(visitor.position,transform.position)<welcomeRange && (controller==null||!controller.inputLocked);
            welcomeUI.SetActive(inside);
        }
        var keys=Keyboard.current;
        if(!open && near && keys!=null && keys.spaceKey.wasPressedThisFrame){open=true;sound.PlayOneShot(hiss,0.6f);}
        // Slide smoothly from closed to open over about half a second.
        t=Mathf.MoveTowards(t,open?1:0,Time.unscaledDeltaTime*2f);
        door.localPosition=closedPos+openOffset*Mathf.SmoothStep(0,1,t);
    }
}
