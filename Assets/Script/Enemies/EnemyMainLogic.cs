using UnityEngine;

public class EnemyMainLogic : MonoBehaviour
{
    public int weaponID = -1;  //-1 means random weapon
    public GameObject[] weapons;

    void OnEnable()
    {
        if (weapons.Length == 0) return;

        int id = weaponID;
        if (id == -1)
            id = Random.Range(0, weapons.Length);

        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(i == id);
        }
    }
}