using UnityEngine;

public class KeyUi : MonoBehaviour
{
    public GameObject k1,k2,k3,k4,k5;
    private GameObject[] allKeys;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        allKeys = new[] {k1,k2,k3,k4,k5};
        foreach (GameObject k in allKeys) k.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(MainLogic.key1) k1.SetActive(true);
        if(MainLogic.key2) k2.SetActive(true);
        if(MainLogic.key3) k3.SetActive(true);
        if(MainLogic.key4) k4.SetActive(true);
        if(MainLogic.key5) k5.SetActive(true);
    }
}
