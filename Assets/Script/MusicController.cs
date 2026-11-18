
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioClip[] music;
    private AudioSource audio;
    private int lastScene = -2;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        int s = Mathf.Max(0, MainLogic.scene);

        if (s != lastScene)
        {
            audio.clip = music[s];
            audio.Play();
            lastScene = s;
        }
    }
}
