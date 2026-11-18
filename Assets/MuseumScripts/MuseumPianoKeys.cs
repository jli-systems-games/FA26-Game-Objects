using UnityEngine;

// Decorative playing gesture, deliberately not a transcription of the concert.
public class MuseumPianoKeys : MonoBehaviour
{
    public MuseumPiano piano;
    public Transform[] keys;
    Vector3[] rest;
    public int PressedKeyCount { get; private set; }
    void Awake(){rest=new Vector3[keys.Length];for(int i=0;i<keys.Length;i++)rest[i]=keys[i].localPosition;}
    void LateUpdate()
    {
        if(rest==null)return;
        bool playing=piano && piano.IsPlaying;
        float time=Time.time;
        PressedKeyCount=0;
        for(int i=0;i<keys.Length;i++){
            if(!keys[i])continue;
            float phase=Mathf.Repeat(time*2.8f+i*.618034f,5f);
            float press=playing && phase<.8f?Mathf.Sin(phase/.8f*Mathf.PI):0;
            if(press>.1f)PressedKeyCount++;
            Vector3 down=keys[i].parent.InverseTransformVector(Vector3.down*.014f);
            keys[i].localPosition=rest[i]+down*press;
        }
    }
    void OnDisable(){if(rest!=null)for(int i=0;i<keys.Length;i++)if(keys[i])keys[i].localPosition=rest[i];}
}
