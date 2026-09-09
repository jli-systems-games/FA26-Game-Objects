using UnityEngine;
//what i want to do with these cubes is that I want the goal to be making the moveable midorichan to collect other cubes
//which means the game needs to judge when midorichan collide with other midori series and the moment they collide the other midoris should disappear
public class CollectCubes : MonoBehaviour
{
    public int totalTargets = 4;
    private int collectedCount = 0;
    //collected count is something decided by the game so it doesnt have to be adjusted in the inspector
    //btw just ignore my comments because im just taking notes from the tutorial
    public UnityEngine.GameObject finalCube;
    //i added this because i want something to appear after the player finish the collection as an output but idk what to do with it yet
    public AudioClip hitSound;
    public AudioClip finishSound;
    private AudioSource audioSource;



    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            if (finalCube != null)
            //if the finalcube is not null(if i have a final cube, hide it)
            {
            finalCube.SetActive(false);
            }
        }

        void OnCollisionEnter(Collision collision)
        //activate the following whent he playercube collides with any target cube
        {
            if (collision.gameObject.CompareTag("Target"))
            //if the one we collide with is one of the target tagged cubes
            {
             if (hitSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(hitSound);
                //wait why do i hear nothing 
            }
             Destroy(collision.gameObject);
             //kaimetsuuuuu
             collectedCount++;
             //+1 collect count
             if (collectedCount >= totalTargets)
             //collectcount not less than total targets
                {
                FinishLevel();
                //yay
                }
            }   
        }

        void FinishLevel()
        {   
            if (finalCube != null)
            {
            finalCube.SetActive(true);
            }
            if (finishSound != null && audioSource != null)
            {
            audioSource.PlayOneShot(finishSound);
            //toriwa nande sorawo toberuno nekowa nande zutto neteruno kimiwa donna kyouwo ikiruno zenbu zenbu zenbu
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }


