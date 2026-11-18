using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

// Another alien visitor. Approach and press T to talk without interrupting exhibits automatically.
// in a speech bubble, typed out with little "alien voice" blips. Walk away and come back: it says its next line.
// The bubble is screen UI, so the text stays sharp (not pixelated).
public class MuseumAlienNPC : MonoBehaviour
{
    [TextArea(2,4)] public string[] lines;
    public float talkRange=3.6f;
    public float voicePitch=1f;        // each alien sounds different
    public float bubbleHeight=2.0f;    // metres above the alien's feet
    public bool floats;                // models without a skeleton bob in the air instead of animating
    public Animator animator;          // optional: "Talk" trigger plays the reaction clip

    static RectTransform bubble; static TMP_Text bubbleText; static MuseumAlienNPC speaker;
    MuseumVisitorController visitor; AudioSource voice; AudioClip blip;
    int line=-1; float typed; Vector3 start; Quaternion homeRot;
    MuseumAlienNPC[] otherVisitors;
    bool focused;
    public bool CanTalk => focused;
    public bool IsTalking => speaker==this;

    void Start()
    {
        visitor=FindFirstObjectByType<MuseumVisitorController>();
        otherVisitors=FindObjectsByType<MuseumAlienNPC>(FindObjectsSortMode.None);
        voice=gameObject.AddComponent<AudioSource>();voice.playOnAwake=false;voice.spatialBlend=0.6f;voice.minDistance=2;voice.maxDistance=12;
        blip=MuseumExhibitInteractor.Tone("Blip",new[]{520f*voicePitch,700f*voicePitch},0.035f,0.22f);
        start=transform.position;homeRot=transform.rotation;
        if(bubble==null)MakeBubble();
    }

    void Update()
    {
        if(floats)transform.position=start+Vector3.up*(0.25f+Mathf.Sin(Time.time*1.3f+start.x)*0.08f);
        if(visitor==null)return;
        Vector3 to=visitor.transform.position-transform.position;to.y=0;
        float distance=to.magnitude;
        var camera=visitor.worldCamera;
        Vector3 look=camera?Vector3.ProjectOnPlane(camera.transform.forward,Vector3.up).normalized:visitor.transform.forward;
        bool available=!visitor.inputLocked && visitor.cameraOverride==null;
        // Explicit interaction is forgiving to approach and cannot interrupt a performance by itself.
        float best=float.MaxValue;MuseumAlienNPC target=null;
        if(available)foreach(var npc in otherVisitors){
            if(!npc || !npc.isActiveAndEnabled)continue;
            Vector3 offset=npc.transform.position-visitor.transform.position;offset.y=0;float d=offset.magnitude;
            if(d>Mathf.Min(npc.talkRange,2.8f) || (d>.1f && Vector3.Dot(look,offset/d)<.5f))continue;
            if(d<best){best=d;target=npc;}
        }
        focused=target==this;

        if(focused && Keyboard.current!=null && Keyboard.current.tKey.wasPressedThisFrame && lines!=null && lines.Length>0){
            Talk();
        }
        if((!available || distance>Mathf.Min(talkRange,2.8f)+.5f) && speaker==this){speaker=null;voice.Stop();if(bubble)bubble.gameObject.SetActive(false);}

        // Turn toward the visitor while talking, back to the exhibit afterwards.
        var want=speaker==this&&to.sqrMagnitude>0.01f?Quaternion.LookRotation(to):homeRot;
        transform.rotation=Quaternion.Slerp(transform.rotation,want,1-Mathf.Exp(-4*Time.deltaTime));

        if(speaker!=this||lines==null||lines.Length==0)return;
        // Typewriter + blips.
        string text=lines[line];int before=(int)typed;typed=Mathf.Min(text.Length,typed+Time.deltaTime*38f);
        if((int)typed>before && (int)typed<text.Length && (int)typed%3==0 && text[(int)typed]!=' ')voice.PlayOneShot(blip,0.5f);
        bubbleText.text=text.Substring(0,(int)typed);
        Place();
    }
    public void Talk()
    {
        if(!focused || lines==null || lines.Length==0)return;
        if(speaker && speaker!=this)speaker.voice.Stop();
        speaker=this;line=(line+1)%lines.Length;typed=0;
        if(animator)animator.SetTrigger("Talk");
    }
    void OnGUI()
    {
        if(!focused)return;
        var style=new GUIStyle(GUI.skin.box){fontSize=22,alignment=TextAnchor.MiddleCenter};
        GUI.Box(new Rect(Screen.width*.5f-220,Screen.height-140,440,42),speaker==this?"[T]  NEXT LINE":"[T]  TALK TO ALIEN",style);
    }

    // Keep the bubble above the alien's head on screen.
    void Place()
    {
        var cam=visitor.worldCamera;if(cam==null)return;
        Vector3 v=cam.WorldToViewportPoint(transform.position+Vector3.up*bubbleHeight);
        bool show=v.z>0;bubble.gameObject.SetActive(show);if(!show)return;
        v.x=Mathf.Clamp(v.x,0.18f,0.82f);v.y=Mathf.Clamp(v.y,0.25f,0.9f);
        bubble.anchorMin=bubble.anchorMax=new Vector2(v.x,v.y);
    }

    static void MakeBubble()
    {
        var canvas=GameObject.Find("Canvas");if(canvas==null)return;
        var g=new GameObject("AlienSpeechBubble",typeof(RectTransform),typeof(Image));g.transform.SetParent(canvas.transform,false);
        var menu=canvas.transform.Find("MuseumMenu");if(menu!=null)g.transform.SetSiblingIndex(menu.GetSiblingIndex()); // under the menus
        bubble=(RectTransform)g.transform;bubble.pivot=new Vector2(0.5f,0);bubble.sizeDelta=new Vector2(560,150);
        var img=g.GetComponent<Image>();img.color=new Color(0.04f,0.03f,0.1f,0.88f);img.raycastTarget=false;
        var edge=new GameObject("Edge",typeof(RectTransform),typeof(Image));edge.transform.SetParent(g.transform,false);
        var er=(RectTransform)edge.transform;er.anchorMin=new Vector2(0,0);er.anchorMax=new Vector2(1,0);er.offsetMin=Vector2.zero;er.offsetMax=new Vector2(0,5);
        edge.GetComponent<Image>().color=new Color(0.25f,0.95f,0.7f);edge.GetComponent<Image>().raycastTarget=false;
        var t=new GameObject("Text",typeof(RectTransform),typeof(TextMeshProUGUI));t.transform.SetParent(g.transform,false);
        var tr=(RectTransform)t.transform;tr.anchorMin=Vector2.zero;tr.anchorMax=Vector2.one;tr.offsetMin=new Vector2(22,14);tr.offsetMax=new Vector2(-22,-14);
        bubbleText=t.GetComponent<TextMeshProUGUI>();bubbleText.fontSize=24;bubbleText.color=new Color(0.85f,1f,0.92f);
        bubbleText.alignment=TextAlignmentOptions.TopLeft;bubbleText.textWrappingMode=TextWrappingModes.Normal;bubbleText.raycastTarget=false;
        g.SetActive(false);
    }

    void OnDisable(){if(speaker==this){speaker=null;if(bubble!=null)bubble.gameObject.SetActive(false);}}
}
