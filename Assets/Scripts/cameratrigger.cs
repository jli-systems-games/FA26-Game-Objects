using UnityEngine;

public class cameratrigger : MonoBehaviour;
public UnityEngine.GameObject mainCameraRef;
public UnityEngine.GameObject switchCamera;
public UnityEngine.GameObject currentCamera;
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCameraRef = UnityEngine.GameObject.Find("Main Camera");
        currentCamera = mainCameraRef;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(collide other)
    {
        currentCamera.SetActive(false);
        switchCamera.SetActive(true);
    }
}
