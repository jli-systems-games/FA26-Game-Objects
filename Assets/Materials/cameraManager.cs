using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject[] cameras;

    void Start()
    {
        SwitchCamera(0);
    }

    public void SwitchCamera(int targetIndex)
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].SetActive(i == targetIndex);
        }
    }
}
