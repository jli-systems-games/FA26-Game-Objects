using UnityEngine;

// The spacesuit exhibit can be worn. Choosing mode 1 ("Try on the suit") swaps the visitor's alien body for the suit;
// mode 0 swaps back. The suit model has no skeleton, so it drifts gently instead of walking, like floating in zero gravity.
public class MuseumWearableSuit : MonoBehaviour
{
    public MuseumVisitorController visitor;
    public GameObject alienBody;       // MuseumVisitor/AlienBody_Visual
    public GameObject suitBody;        // MuseumVisitor/SuitBody_Visual (hidden until worn)
    public GameObject exhibitFigure;   // the suit standing next to the plinth (hidden while you wear it)
    public float bob=0.06f;

    Vector3 suitStart;
    void Awake(){if(suitBody!=null)suitStart=suitBody.transform.localPosition;}

    // Hooked to MuseumExhibit.onModeChanged
    public void OnModeChanged(int mode)
    {
        bool wear=mode==1;
        if(alienBody!=null)alienBody.SetActive(!wear);
        if(suitBody!=null)suitBody.SetActive(wear);
        if(exhibitFigure!=null)exhibitFigure.SetActive(!wear);
        if(visitor!=null)visitor.RefreshVisuals();
    }

    void Update()
    {
        if(suitBody!=null && suitBody.activeSelf)
            suitBody.transform.localPosition=suitStart+Vector3.up*(Mathf.Sin(Time.time*1.6f)*bob+bob);
    }
}
