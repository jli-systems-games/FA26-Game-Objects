using UnityEngine;

public class BlasphamousCharacterAnimation : MonoBehaviour
{
    public GameObject leftStatic, left1, left2;
    public GameObject rightStatic, right1, right2;

    public int action = 1;
    public float frameTime = 0.15f;
    private float lastX, timer;

    void OnEnable()
    {
        lastX = transform.position.x;
        timer = 0;

        if (action <= 3) action = 1;
        else action = 4;
        ShowAction();
    }

    void LateUpdate()
    {
        float x = transform.position.x;

        if (x == lastX)
        {
            if (action <= 3) action = 1;
            else action = 4;

            timer = 0;
        }
        else
        {
            int first = 2;              //right 1

            if (x < lastX) first = 5;  // left 1

            timer += Time.deltaTime;


            if (action != first && action != first + 1)
            {
                action = first;
                timer = 0;
            }
            else if (timer >= frameTime)
            {
    
                if (action == first) action = first + 1;
                else action = first;

                timer = 0;
            }
        }

        ShowAction();
        lastX = x;
    }

    void ShowAction()
    {
        rightStatic.SetActive(action == 1);
        right1.SetActive(action == 2);
        right2.SetActive(action == 3);

        leftStatic.SetActive(action == 4);
        left1.SetActive(action == 5);
        left2.SetActive(action == 6);
    }
}
