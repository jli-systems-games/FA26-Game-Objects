using UnityEngine;

public class BoneSound : MonoBehaviour
{
    public AudioClip eatSound;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 检测碰到的物体标签是Player（小狗的Tag设置为Player）
        if (other.CompareTag("Player"))
        {
            if (eatSound != null)
            {
                audioSource.PlayOneShot(eatSound);
            }
            // 你原本就有的骨头消失代码
            gameObject.SetActive(false);
        }
    }
}
