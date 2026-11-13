using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

// Lives on the visitor. Finds the exhibit in front, shows the E prompt, opens/closes the label panel.
// Runs after MuseumVisitorController so Esc/E used here are not also handled there in the same frame.
[DefaultExecutionOrder(100)]
public class MuseumExhibitInteractor : MonoBehaviour
{
    public MuseumVisitorController visitor;
    public GameObject promptRoot;
    public TMP_Text promptText;
    public GameObject panelRoot;
    public TMP_Text titleText, metaText, bodyText, themeText, captionText, creditText;
    public Button[] modeButtons;
    public Button closeButton;
    public ScrollRect scroll;   // optional: panel text scrolls with the mouse wheel / trackpad
    public AudioSource audioSource;

    MuseumExhibit[] exhibits;
    MuseumExhibit focused, open;
    public bool IsReading => open!=null;   // used by the pause menu
    AudioClip focusClip, openClip, closeClip, modeClip;

    void Awake()
    {
        if(visitor==null)visitor=GetComponent<MuseumVisitorController>();
        if(audioSource==null)audioSource=gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake=false;audioSource.spatialBlend=0;
        focusClip=Tone("Focus",new[]{880f},0.08f,0.18f);
        openClip=Tone("Open",new[]{523f,784f,1047f},0.11f,0.25f);
        closeClip=Tone("Close",new[]{784f,523f},0.09f,0.22f);
        modeClip=Tone("Mode",new[]{660f,990f},0.06f,0.2f);
        for(int i=0;i<modeButtons.Length;i++){int index=i;if(modeButtons[i]!=null)modeButtons[i].onClick.AddListener(()=>SelectMode(index));}
        if(closeButton!=null)closeButton.onClick.AddListener(Close);
        if(panelRoot!=null)panelRoot.SetActive(false);
        if(promptRoot!=null)promptRoot.SetActive(false);
    }

    void Start(){exhibits=FindObjectsByType<MuseumExhibit>(FindObjectsSortMode.None);}

    void Update()
    {
        var keys=Keyboard.current;
        if(open!=null){
            ScrollPanel(keys);
            if(keys!=null){
                if(keys.eKey.wasPressedThisFrame||keys.escapeKey.wasPressedThisFrame){Close();return;}
                if(keys.digit1Key.wasPressedThisFrame)SelectMode(0);
                if(keys.digit2Key.wasPressedThisFrame)SelectMode(1);
                if(keys.digit3Key.wasPressedThisFrame)SelectMode(2);
                if(keys.digit4Key.wasPressedThisFrame)SelectMode(3);
            }
            return;
        }
        var next=FindFocus();
        if(next!=focused){
            if(focused!=null)focused.SetFocused(false);
            focused=next;
            if(focused!=null){focused.SetFocused(true);Play(focusClip,0.35f);}
        }
        if(promptRoot!=null)promptRoot.SetActive(focused!=null);
        if(focused!=null && promptText!=null)promptText.text="<b>[E]</b>  INSPECT  "+focused.title.ToUpper();
        if(focused!=null && keys!=null && keys.eKey.wasPressedThisFrame)Open(focused);
    }

    MuseumExhibit FindFocus()
    {
        if(exhibits==null)return null;
        var cam=visitor!=null && visitor.worldCamera!=null ? visitor.worldCamera.transform : transform;
        Vector3 look=Vector3.ProjectOnPlane(cam.forward,Vector3.up).normalized;
        MuseumExhibit best=null;float bestScore=float.MaxValue;
        foreach(var e in exhibits){
            if(e==null||!e.isActiveAndEnabled)continue;
            Vector3 to=e.interactPoint.position-transform.position;to.y=0;
            float d=to.magnitude;
            if(d>e.interactRange)continue;
            if(d>0.6f && Vector3.Dot(look,to/d)<0.35f)continue; // roughly facing it
            if(d<bestScore){best=e;bestScore=d;}
        }
        return best;
    }

    void Open(MuseumExhibit e)
    {
        open=e;e.SetFocused(false);e.SetInspecting(true);
        if(visitor!=null){visitor.inputLocked=true;visitor.SetLocked(false);}
        if(promptRoot!=null)promptRoot.SetActive(false);
        if(titleText)titleText.text=e.title;
        if(metaText)metaText.text=e.meta;
        if(bodyText)bodyText.text=e.body;
        if(themeText)themeText.text="<b>WHY IT IS HERE</b>\n"+e.themeLink;
        if(creditText)creditText.text=e.credit;
        for(int i=0;i<modeButtons.Length;i++){
            if(modeButtons[i]==null)continue;
            bool has=e.modes!=null && i<e.modes.Length;
            modeButtons[i].gameObject.SetActive(has);
            var label=modeButtons[i].GetComponentInChildren<TMP_Text>();
            if(has && label!=null)label.text=(i+1)+"  "+e.modes[i].label;
        }
        // Simple exhibits leave Modes empty: the panel is then just title + text + close.
        bool interactive=e.modes!=null && e.modes.Length>0;
        if(captionText)captionText.gameObject.SetActive(interactive);
        // In the scrollable layout the buttons sit in their own row: hide the whole row for simple exhibits.
        if(modeButtons.Length>0 && modeButtons[0]!=null && modeButtons[0].transform.parent.name=="ModeButtons")modeButtons[0].transform.parent.gameObject.SetActive(interactive);
        var hint=panelRoot!=null?panelRoot.transform.Find("Content/KeysHint"):null;
        if(hint)hint.gameObject.SetActive(interactive);
        RefreshMode();
        if(panelRoot!=null)panelRoot.SetActive(true);
        if(scroll!=null){Canvas.ForceUpdateCanvases();scroll.verticalNormalizedPosition=1;} // start at the top
        Play(openClip,0.5f);
    }

    public void Close()
    {
        if(open==null)return;
        open.SetInspecting(false);open=null;focused=null;
        if(panelRoot!=null)panelRoot.SetActive(false);
        if(visitor!=null){visitor.inputLocked=false;visitor.SetLocked(true);}
        Play(closeClip,0.45f);
    }

    public void SelectMode(int index)
    {
        if(open==null||open.modes==null||index>=open.modes.Length)return;
        open.SetMode(index);RefreshMode();Play(modeClip,0.4f);
    }

    void RefreshMode()
    {
        if(open==null)return;
        var m=open.Current;
        if(captionText)captionText.text=m!=null?m.caption:"";
        for(int i=0;i<modeButtons.Length;i++){
            if(modeButtons[i]==null)continue;
            var img=modeButtons[i].GetComponent<Image>();
            if(img)img.color=i==open.CurrentMode && open.IsPlayingMode(i)?new Color(0.25f,0.85f,0.9f,0.95f):new Color(0.16f,0.12f,0.3f,0.95f);
        }
    }

    // Scroll the open panel with the mouse wheel / trackpad, or W/S and the arrow keys.
    // Done in code so it works wherever the mouse is (the text itself does not catch UI clicks).
    void ScrollPanel(Keyboard keys)
    {
        if(scroll==null||scroll.content==null||scroll.viewport==null)return;
        float extra=scroll.content.rect.height-scroll.viewport.rect.height;if(extra<=1)return;
        float delta=0;
        var mouse=Mouse.current;if(mouse!=null)delta+=mouse.scroll.ReadValue().y*0.5f;
        if(keys!=null){if(keys.wKey.isPressed||keys.upArrowKey.isPressed)delta+=900*Time.unscaledDeltaTime;if(keys.sKey.isPressed||keys.downArrowKey.isPressed)delta-=900*Time.unscaledDeltaTime;}
        if(delta!=0)scroll.verticalNormalizedPosition=Mathf.Clamp01(scroll.verticalNormalizedPosition+delta/extra);
    }

    void Play(AudioClip clip,float volume){if(audioSource!=null&&clip!=null)audioSource.PlayOneShot(clip,volume);}

    // Small synthesized chimes so the project has no unlicensed audio files.
    public static AudioClip Tone(string name,float[] notes,float noteLength,float amp)
    {
        const int rate=44100;int per=(int)(rate*noteLength);var data=new float[per*notes.Length+rate/10];
        for(int n=0;n<notes.Length;n++)for(int i=0;i<per+rate/10 && n*per+i<data.Length;i++){
            float t=i/(float)rate;float env=Mathf.Exp(-t*18)*Mathf.Clamp01(t*400);
            data[n*per+i]+=amp*env*(Mathf.Sin(2*Mathf.PI*notes[n]*t)+0.3f*Mathf.Sin(4*Mathf.PI*notes[n]*t));
        }
        var clip=AudioClip.Create(name,data.Length,1,rate,false);clip.SetData(data,0);return clip;
    }
}
