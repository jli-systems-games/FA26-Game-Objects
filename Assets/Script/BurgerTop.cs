using UnityEngine;

public class BurgerTop : MonoBehaviour
{
    public float distance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void LateUpdate()
    {
        distance = (Time.time % 1)/3 ;

        transform.position = MainCharacter.MainCharacterPosition + new Vector3(0, distance, 0);
    }
}
