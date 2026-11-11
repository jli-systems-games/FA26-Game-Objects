using UnityEngine;

public class SingletonDemo : MonoBehaviour
{
    public enum PlayerMovement 
    { 
        canMove,
        cantMove
    }

    public PlayerMovement canPlayerMove;


    public static SingletonDemo singletonScript;
    public int playerHealth = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Awake()
    {
        if ( singletonScript == null) 
        {
            singletonScript = this;
        }
        else if ( singletonScript != this) 
        { 
            Destroy(singletonScript);   
        }

        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Interactable") 
        {
            canPlayerMove = PlayerMovement.cantMove;
        }
    }
}
