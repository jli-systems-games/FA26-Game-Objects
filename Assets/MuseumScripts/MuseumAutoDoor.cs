using UnityEngine;

// Automatic sliding glass doors at the museum entrance.
// No key needed: when the visitor comes within 'range' the two panels slide apart, when they walk away the doors close.
public class MuseumAutoDoor : MonoBehaviour
{
    public Transform leftPanel, rightPanel;
    public float slide=2.1f;      // how far each panel moves sideways (local X)
    public float range=3.5f;
    public float speed=1.6f;      // open/close in about 0.6 s
    public Transform visitor;

    Vector3 leftClosed,rightClosed;
    float t; bool wasOpen;
    AudioSource sound; AudioClip whoosh;

    void Awake()
    {
        if(visitor==null){var v=FindFirstObjectByType<MuseumVisitorController>();if(v!=null)visitor=v.transform;}
        leftClosed=leftPanel.localPosition;rightClosed=rightPanel.localPosition;
        sound=gameObject.AddComponent<AudioSource>();sound.playOnAwake=false;sound.spatialBlend=0.8f;
        whoosh=MuseumExhibitInteractor.Tone("Door",new[]{392f,523f},0.08f,0.18f);
    }

    void Update()
    {
        bool open=visitor!=null && Vector3.Distance(visitor.position,transform.position)<range;
        if(open!=wasOpen){sound.PlayOneShot(whoosh,0.5f);wasOpen=open;}
        t=Mathf.MoveTowards(t,open?1:0,Time.deltaTime*speed);
        float s=Mathf.SmoothStep(0,1,t)*slide;
        leftPanel.localPosition=leftClosed+Vector3.left*s;
        rightPanel.localPosition=rightClosed+Vector3.right*s;
    }
}
