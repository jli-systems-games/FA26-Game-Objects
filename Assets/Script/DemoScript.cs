using UnityEngine;

public class DemoScript : MonoBehaviour
{
    public GameObject blueCube;

    public float redSpeed = 2f;
    public float blueSpeed = 2f;
    public float blueMoveDistance = 3f;

    private Vector3 bluePositionA;
    private Vector3 bluePositionB;

    private bool blueIsMoving = false;
    private bool blueMovesToB = true;

    void Start()
    {
        if (blueCube != null)
        {
            bluePositionA = blueCube.transform.position;
            bluePositionB =
                bluePositionA + Vector3.right * blueMoveDistance;
        }
    }

    void Update()
    {
        if (blueCube == null)
        {
            return;
        }

        if (blueIsMoving)
        {
            Vector3 targetPosition;

            if (blueMovesToB)
            {
                targetPosition = bluePositionB;
            }
            else
            {
                targetPosition = bluePositionA;
            }

            blueCube.transform.position = Vector3.MoveTowards(
                blueCube.transform.position,
                targetPosition,
                blueSpeed * Time.deltaTime
            );

            if (
                Vector3.Distance(
                    blueCube.transform.position,
                    targetPosition
                ) < 0.01f
            )
            {
                blueIsMoving = false;
                blueMovesToB = !blueMovesToB;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                blueCube.transform.position,
                redSpeed * Time.deltaTime
            );

            if (
                Vector3.Distance(
                    transform.position,
                    blueCube.transform.position
                ) < 0.01f
            )
            {
                blueIsMoving = true;
            }
        }
    }
}