using System.ComponentModel.Design;
using UnityEngine;

public class AnimationDemo : MonoBehaviour
{
    public Animator cubeAnimator; //Will contain a reference to my cube animator component.
    
    void Start()
    {
        cubeAnimator = GetComponent<Animator>();//Automatically grabs a reference to the gameobjects animator.
    }

    // Update is called once per frame
    void Update()
    {
        //If I press the 'R' key, it will check what the current state of my cube is
        // then switch to the opposite. 
        if (Input.GetKeyUp(KeyCode.R)) 
        { 
            if (cubeAnimator.GetInteger("CubeState") == 0) 
            {
                cubeAnimator.SetInteger("CubeState", 1);
            }
            else if (cubeAnimator.GetInteger("CubeState") == 1) 
            {
                cubeAnimator.SetInteger("CubeState", 0);
            }
        }
        
        
    }
}
