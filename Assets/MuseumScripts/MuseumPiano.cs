using System.Collections;
using UnityEngine;
using UnityEngine.Video;

// Local, user-supplied recordings only. Reference URLs are credits, never media URLs.
[RequireComponent(typeof(MuseumExhibit), typeof(AudioSource), typeof(VideoPlayer))]
public class MuseumPiano : MonoBehaviour
{
    [System.Serializable] public class Recording
    {
        public string title, referenceUrl;
        public AudioClip audio;
        public VideoClip video;
        public double startSeconds, endSeconds;
    }
    public Recording[] recordings;
    public Renderer performanceScreen;
    public TMPro.TMP_Text screenLabel;
    public float fadeSeconds = 0.6f;
    public string Status { get; private set; } = "Choose a performance";
    public bool IsPlaying => player && player.isPlaying || source && source.isPlaying;
    MuseumExhibit exhibit;
    MuseumVisitorController visitor;
    AudioSource source;
    VideoPlayer player;
    RenderTexture texture;
    Coroutine transition;
    int selected = -1;
    bool videoRequested;
    readonly System.Collections.Generic.Dictionary<AudioSource,float> background = new();

    // TextMeshPro's readable face points toward the keyboard side (local -Z).
    void OnEnable(){if(screenLabel)screenLabel.transform.localRotation=Quaternion.identity;}

    void Awake()
    {
        exhibit=GetComponent<MuseumExhibit>(); source=GetComponent<AudioSource>(); player=GetComponent<VideoPlayer>();
        visitor=FindFirstObjectByType<MuseumVisitorController>();
        source.playOnAwake=false; source.loop=false; source.volume=0; source.spatialBlend=0;
        player.playOnAwake=false; player.isLooping=false; player.renderMode=VideoRenderMode.RenderTexture;
        player.audioOutputMode=VideoAudioOutputMode.AudioSource; player.controlledAudioTrackCount=1;
        player.SetTargetAudioSource(0,source); player.aspectRatio=VideoAspectRatio.FitInside;
        texture=new RenderTexture(1280,720,0); texture.Create(); player.targetTexture=texture;
        if(performanceScreen)performanceScreen.material.mainTexture=texture;
        SetScreenVisible(false);
        player.prepareCompleted+=Prepared; player.errorReceived+=Failed; player.loopPointReached+=Finished;
        exhibit.onModeChanged ??= new UnityEngine.Events.UnityEvent<int>();
        exhibit.onModeChanged.AddListener(Select);
        SetStatus("Choose a performance · local recordings required");
    }
    public void Select(int index)
    {
        if(!exhibit.Inspecting || recordings==null || index<0 || index>3)return;
        if(transition!=null)StopCoroutine(transition);
        if(index==3){transition=StartCoroutine(Change(-1));return;}
        if(index>=recordings.Length)return;
        transition=StartCoroutine(Change(selected==index?-1:index));
    }
    IEnumerator Change(int index)
    {
        yield return Fade(0);
        player.Stop(); source.Stop(); source.clip=null; selected=-1; videoRequested=false;
        SetScreenVisible(false);
        ClearScreen(); RestoreBackground();
        if(index<0){SetStatus("Playback stopped");transition=null;yield break;}
        var recording=recordings[index];
        if(!recording.video && !recording.audio){SetStatus("Media not supplied: "+recording.title);transition=null;yield break;}
        selected=index;
        foreach(var other in FindObjectsByType<AudioSource>(FindObjectsSortMode.None))
            if(other!=source && other.isPlaying && other.loop){background[other]=other.volume;other.volume=0;}
        SetStatus("Loading: "+recording.title);
        if(recording.video){videoRequested=true;player.source=VideoSource.VideoClip;player.clip=recording.video;player.Prepare();}
        else {source.clip=recording.audio;source.Play();SetStatus(recording.title);yield return Fade(0.75f);}
        transition=null;
    }
    void Prepared(VideoPlayer video)
    {
        if(selected<0 || !videoRequested)return;
        player.Play();SetStatus(recordings[selected].title);
        SetScreenVisible(true);
        if(recordings[selected].startSeconds>0)player.time=recordings[selected].startSeconds;
        transition=StartCoroutine(Fade(0.75f));
    }
    void Failed(VideoPlayer video,string error){StopPlayback();SetStatus("Could not play this local video. Check its format.");Debug.LogWarning("Piano video: "+error,this);}
    void Finished(VideoPlayer video){StopPlayback();SetStatus("Performance finished");}
    IEnumerator Fade(float target)
    {
        float start=source.volume;
        for(float t=0;t<fadeSeconds;t+=Time.unscaledDeltaTime){source.volume=Mathf.Lerp(start,target,t/Mathf.Max(0.01f,fadeSeconds));yield return null;}
        source.volume=target;
    }
    void Update()
    {
        SetScreenVisible(player && player.isPlaying);
        if(selected>=0 && videoRequested && player.isPlaying && recordings[selected].endSeconds>0 && player.time>=recordings[selected].endSeconds){StopPlayback();SetStatus("Performance finished");}
        if(selected>=0 && visitor && Vector3.Distance(visitor.transform.position,transform.position)>exhibit.interactRange+1f){
            if(transition!=null)StopCoroutine(transition);selected=-1;videoRequested=false;transition=StartCoroutine(Change(-1));
        }
        if(selected>=0 && !videoRequested && !source.isPlaying && transition==null){StopPlayback();SetStatus("Performance finished");}
    }
    void SetStatus(string message){Status=message;if(screenLabel)screenLabel.text="SOUNDS OF EARTH\n"+message;}
    void SetScreenVisible(bool visible)
    {
        if(performanceScreen)performanceScreen.gameObject.SetActive(visible);
        var frame=transform.Find("ScreenFrame");if(frame)frame.gameObject.SetActive(visible);
        if(screenLabel)screenLabel.gameObject.SetActive(visible);
    }
    void ClearScreen(){var old=RenderTexture.active;RenderTexture.active=texture;GL.Clear(true,true,Color.black);RenderTexture.active=old;}
    void RestoreBackground(){foreach(var pair in background)if(pair.Key)pair.Key.volume=pair.Value;background.Clear();}
    public void StopPlayback(){if(transition!=null)StopCoroutine(transition);transition=null;selected=-1;videoRequested=false;if(player)player.Stop();if(source)source.Stop();SetScreenVisible(false);RestoreBackground();if(texture)ClearScreen();}
    void OnDisable(){StopPlayback();}
    void OnDestroy(){if(texture){texture.Release();Destroy(texture);}}
}
