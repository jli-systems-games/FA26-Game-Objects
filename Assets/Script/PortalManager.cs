using UnityEngine;

public class PortalManager : MonoBehaviour
{

    public GameObject correspondScene,ppt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter(Collider other){

        if(other.CompareTag("Player")){
            MainLogic.scene = int.Parse(gameObject.name);
            correspondScene.SetActive(true);
            ppt.SetActive(true);
            Debug.LogError(MainLogic.scene);

            
    }
        
    }
}
