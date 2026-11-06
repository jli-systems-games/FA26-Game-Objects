using UnityEngine;

// One curated exhibit: label text, interaction modes and the scene objects each mode drives.
public class MuseumExhibit : MonoBehaviour
{
    [System.Serializable]
    public class Mode
    {
        public string label;
        [TextArea(2,5)] public string caption;
        public Color screenColor=Color.cyan;
        public GameObject showObject;   // optional: visible only in this mode
        public float lift;              // how far the model rises in this mode
        public float spinSpeed=18;
        public AudioClip music;         // optional: a record this mode plays (record player exhibit)
    }

    [Header("Label")]
    public string title="Exhibit";
    public string meta="";
    [TextArea(4,12)] public string body="";
    [TextArea(2,6)] public string themeLink="";
    [TextArea(1,3)] public string credit="";

    [Header("Scene hooks")]
    public Transform interactPoint;
    public float interactRange=3f;
    public Transform model;
    public Renderer screen;
    public GameObject focusHighlight;
    public AudioSource musicSource;     // plays Mode.music; keeps playing after the panel closes

    public Mode[] modes;
    public int CurrentMode { get; private set; }
    public bool Inspecting { get; private set; }
    public Mode Current => modes!=null && modes.Length>0 ? modes[CurrentMode] : null;

    Vector3 modelStart;
    float lift;
    MaterialPropertyBlock block;

    void Awake()
    {
        if(interactPoint==null)interactPoint=transform;
        if(model!=null)modelStart=model.localPosition;
        SetFocused(false);
        SetMode(0);
    }

    public void SetFocused(bool value){if(focusHighlight!=null)focusHighlight.SetActive(value);}
    bool HasMusic => musicSource!=null && modes!=null && System.Array.Exists(modes,m=>m.music!=null);
    public bool IsPlayingMode(int i)=>!HasMusic || (musicSource.isPlaying && musicSource.clip==modes[i].music);

    // Record player: opening/closing the panel does not change the record.
    public void SetInspecting(bool value){Inspecting=value;if(!HasMusic)SetMode(0);}

    public void SetMode(int index)
    {
        if(modes==null || modes.Length==0)return;
        CurrentMode=Mathf.Clamp(index,0,modes.Length-1);
        if(HasMusic && Inspecting){
            var clip=modes[CurrentMode].music;
            if(musicSource.isPlaying && musicSource.clip==clip)musicSource.Stop(); // same record again = stop
            else{musicSource.clip=clip;musicSource.loop=true;musicSource.Play();}
        }
        for(int i=0;i<modes.Length;i++)
            if(modes[i].showObject!=null)modes[i].showObject.SetActive(Inspecting && i==CurrentMode);
        if(screen!=null){
            block??=new MaterialPropertyBlock();
            screen.GetPropertyBlock(block);
            var c=modes[CurrentMode].screenColor;
            block.SetColor("_BaseColor",c);block.SetColor("_EmissionColor",c*2.2f);
            screen.SetPropertyBlock(block);
        }
    }

    void Update()
    {
        if(model==null)return;
        var m=Current;
        float target=Inspecting && m!=null ? m.lift : 0;
        float spin=Inspecting && m!=null ? m.spinSpeed : 10;
        lift=Mathf.Lerp(lift,target,1-Mathf.Exp(-4*Time.deltaTime));
        model.localPosition=modelStart+Vector3.up*(lift+Mathf.Sin(Time.time*1.3f)*0.015f);
        model.Rotate(0,spin*Time.deltaTime,0,Space.World);
    }
}
