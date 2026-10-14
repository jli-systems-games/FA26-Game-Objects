using UnityEngine;
using System.Collections.Generic;

public class InstantiateDemo : MonoBehaviour
{
    //USING RANDOMIZATION OT INSTANTIATE PREFABS USING LISTS.

    public List<GameObject> shapePrefabs;//STEP 1: Create a list of prefab gameobjects you want to Instantiate

    public List<Transform> spawnLocations;//STEP 2: If you have multiple desired spawnpoints, use a list to keep a reference for all of them.
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //My prefabs will spawn when I press and lift up the spacebar key.
        if (Input.GetKeyUp(KeyCode.Space)) 
        {
            PrefabSpawner();
        }
    }

    public void PrefabSpawner() 
    {
        print("You are spawning the gameobjects");//Always use 'print'/debug.log to make sure your function is being called :)

       
        foreach (Transform spawnPoints in spawnLocations) //I use a 'foreach' loop to iterate through my 'spawnLocations' list.
        {
            //For each location in my list, this loop will randomly assign a prefab to the local variable 'currentShape' then place it at the current 'spawnpoint' position.
            GameObject currentShape = shapePrefabs[Random.Range(0, shapePrefabs.Count)];
            Instantiate(currentShape, spawnPoints.position, Quaternion.identity);
        }
    }
}
