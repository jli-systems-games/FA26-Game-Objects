using System.Collections.Generic;using UnityEngine;

public class PickDinner : MonoBehaviour
{
    [SerializeField] private List<GameObject> foodImages;


    public void ChooseDinner()
    {
        int index = Random.Range(0, foodImages.Count);        

        for (int i = 0; i < foodImages.Count; i++) foodImages[i].SetActive(i == index);   // show the chosen picture, and hide everything else
    }
}