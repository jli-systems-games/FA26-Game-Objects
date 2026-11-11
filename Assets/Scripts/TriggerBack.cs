using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerBack : MonoBehaviour

{

                // COMMENTED OUT CODE BELOW IS FOR SINGLETON IMPLEMENTATION (USE FOR BGM LATER)
   // public static TriggerBack scriptInstance;

    // private void Awake()
    // {
    //     if (scriptInstance == null) 
    //     {
    //         scriptInstance = this;
    //     }
    //     else
    //     {
    //         Destroy(gameObject);
    //     }
    // }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       // scriptInstance = GetComponent<TriggerBack>();
    }

    // Update is called once per frame
    void Update()
    {
      //  Scene currentScene = SceneManager.GetActiveScene();
    }

    private void OnTriggerEnter(Collider other)
    {
      Debug.Log("triggered");
      SceneManager.LoadScene(0);
      
    }

}
