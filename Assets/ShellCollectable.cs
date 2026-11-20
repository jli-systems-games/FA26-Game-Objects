using UnityEngine;

public class ShellCollectible : MonoBehaviour
{
    private bool collected;

    public bool Collect()
    {
        if (collected)
            return false;

        collected = true;
        gameObject.SetActive(false);
        return true;
    }
}