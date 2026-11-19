using UnityEngine;

public class BossKick : MonoBehaviour
{
    public float speed = 5f, rotateSpeed = 1080f;
    private float nowSpeed = 0f, spin = 0f;
    public GameObject DeadCell,blood;

    void Update()
    {
        DeadCell.transform.Translate(speed * Time.deltaTime, 0, 0, Space.World);
        this.transform.Translate(nowSpeed * Time.deltaTime, 0, 0, Space.World);
        this.transform.Rotate(0, 0, spin * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeadCell"))
        {
            nowSpeed = speed;
            spin = rotateSpeed;
            blood.SetActive(false);

        }
    }
}