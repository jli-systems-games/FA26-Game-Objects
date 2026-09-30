using UnityEngine;

public class AnimationDemo : MonoBehaviour
{
    public Animator cubeAnimator;//Will contain a reference to my cube animator component
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        //cubeAnimator = GetComponent<Animator>();//automaticall finds the component, wont work if i have no animator component
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyUp(KeyCode.R))//did i press r key
        {
            if (cubeAnimator.GetInteger("CubeState") == 0)//id cube state peramiter is 0, switch it to 1
            {
                cubeAnimator.SetInteger("CubeState", 1);//change it to 1. integer nbeacuse my perameter i made is an integer, can be bool,float etc.
            }
            else if(cubeAnimator.GetInteger("CubeState") == 1)//if its 1
            {
                cubeAnimator.SetInteger("CubeState", 0);//go back to 0. etc.
            }
        }
        cubeAnimator.SetInteger("CubeState",1);//integer nbeacuse my perameter i made is an integer, can be bool,float etc.
    }
}

