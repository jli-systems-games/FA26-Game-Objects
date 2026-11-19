using UnityEngine;

public class TP : MonoBehaviour
{
public int keyNumber;
    public GameObject player, point;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController cc = player.GetComponent<CharacterController>();

            cc.enabled = false;  //麻痹角色控制器强制移动
            player.transform.position = point.transform.position;
            player.transform.rotation = point.transform.rotation;
            cc.enabled = true;
            Debug.Log("Player: " + player.transform.position + " / Anchor: " + point.transform.position);


            MainLogic.scene = 0;
            Time.timeScale = 1f;
        }
    }

}
