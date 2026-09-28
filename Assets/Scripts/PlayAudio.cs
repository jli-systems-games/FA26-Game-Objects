using UnityEngine;

public class PlayAudio : MonoBehaviour
// {
//     public AudioSource audioSource;
//     public AudioClip[] audioClips;

//     public void PlayRandomMusic()
//         {
//             int randomIndex = Random.Range(0, audioClips.Length);
//             AudioClip selectedClip = audioClips[randomIndex];

//             audioSource.PlayOneShot(selectedClip);
//         }
    
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }

{
    private AudioSource a_source;
    public AudioClip[] a_clips;

void Start ()
        {        
        a_source = gameObject.AddComponent<AudioSource>();      
        //can also switch add to get if object already has an audio source
        }	
	
void Update ()
         {
        if(Input.GetMouseButtonDown(0))        
            PlayRandomSound();        	
	     }

public void PlayRandomSound()
        {
        int selection = Random.Range(0, a_clips.Length);
        a_source.PlayOneShot(a_clips[selection]);
        }
}