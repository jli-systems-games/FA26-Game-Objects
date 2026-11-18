using UnityEngine;

public class Key : MonoBehaviour
{
public int keyNumber;
    public GameObject player, miniGame;
    public GameObject p1, p2, p3, p4, p5;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject point = null;

            if (keyNumber == 1) { MainLogic.key1 = true; point = p1; }
            if (keyNumber == 2) { MainLogic.key2 = true; point = p2; }
            if (keyNumber == 3) { MainLogic.key3 = true; point = p3; }
            if (keyNumber == 4) { MainLogic.key4 = true; point = p4; }
            if (keyNumber == 5) { MainLogic.key5 = true; point = p5; }
            if (point == null) return;

    CharacterController cc = player.GetComponent<CharacterController>();

    cc.enabled = false;  //麻痹角色控制器强制移动
    player.transform.position = point.transform.position;
    player.transform.rotation = point.transform.rotation;
    cc.enabled = true;
            Debug.Log("Player: " + player.transform.position + " / Anchor: " + point.transform.position);


            MainLogic.scene = 0;
            Time.timeScale = 1f;
            miniGame.SetActive(false);
        }
    }
}
