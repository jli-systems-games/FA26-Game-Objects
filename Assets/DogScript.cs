using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class DogScript : MonoBehaviour
{
    public List<Sprite> dogSprites; //Will contain a list of all the dog sprites I want to use. 
    public Image spriteUIHolder; //This will be a reference to my Image component on the gameobject this script is attached to. 
    
    
    //Example variable for the loop demonstration in Loop Container function.
    public float timer = 30;

    void Start()
    {
        spriteUIHolder = GetComponent<Image>();//Automatically grabs a reference to my Image component. 
        StartCoroutine(SwapImages());//This will start the dog swap coroutine.
    }

    // Update is called once per frame
    void Update()
    {
        //Dummy timer meant to showcase different loops in the LoopContainer Function. 
        //timer -= Time.deltaTime;    
    }
    public IEnumerator SwapImages() 
    {
        //In this coroutine, I use a 'foreach' loop to increment up the dog sprite list. Each time that happens.
        // The sprite within my Image component will change to the current dogImage sprite. 

        print("Coroutine has been called.");//Console message to let me know that my function is being called.

        foreach (Sprite dogImage in dogSprites)//"for reach image in my 'dogSprites' list...
        {
            spriteUIHolder.sprite = dogImage;//Change the current sprite within my Image component to the current 'dogimage' sprite in my 'foreach' loop...
            print("The current image is " +  dogImage.name);//Prints a console message telling me what the current image is within my Image component. 
            yield return new WaitForSeconds(2f);//Waits two seconds before repeating the 'foreach' loop again.
        }
       
        StartCoroutine(SwapImages());//I call the coroutine again so this loop goes on 'forever'. 
    }

    public void LoopContainer() 
    {
        //This function simply contains examples of other Loops. They are not plugged into anything functional.

        while (timer > 0)
        {
            //Do something awesome.
        }
        for (int index = 0; index <= timer; index++)
        {
            //Everytime index goes up, something happens. 
        }
    }
}
