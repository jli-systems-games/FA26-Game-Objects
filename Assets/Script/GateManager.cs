using UnityEngine;

public class GateManager : MonoBehaviour

{
    public GameObject g1,g2,g3,g4,g5;
    private GameObject[] allGates;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        allGates = new[] {g1,g2,g3,g4,g5};
        foreach(GameObject g in allGates) g.SetActive (false);

    }

    // Update is called once per frame
    void Update()
    {
        if(MainLogic.key1) g1.SetActive(true);
        if(MainLogic.key2) g2.SetActive(true);
        if(MainLogic.key3) g3.SetActive(true);
        if(MainLogic.key4) g4.SetActive(true);
        if(MainLogic.key5) g5.SetActive(true);
    }
}
