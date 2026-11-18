using UnityEngine;

public class transition : MonoBehaviour
{
    private int lastScene;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);
        lastScene = MainLogic.scene;


    }


    void Update()
    {
        if(MainLogic.scene != lastScene){
            SwitchScene();
            lastScene = MainLogic.scene;
            Debug.LogError(MainLogic.scene);
        }
    }

    // void lateUpdate(){

    // }

    public void SwitchScene(){
        Debug.LogError("SwitchScene");
    }

}
