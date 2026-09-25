using UnityEngine;

public class AnimationDemo : MonoBehaviour
{
    public Animator cubeAnimator;
    
    void Start()
    {
        cubeAnimator = GetComponent<Animator>();
    }

    
    void Update()
    {
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
