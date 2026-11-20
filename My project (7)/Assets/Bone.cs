using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : MonoBehaviour
{
    [Header("拾取音效")]
    public AudioClip collectSfx;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //碰到玩家小狗
        if (col.CompareTag("Player"))
        {
            BoneCollector collector = col.GetComponent<BoneCollector>();
            if (collector != null)
            {
                // 骨头播放音效
                if (collectSfx != null)
                {
                    audioSource.PlayOneShot(collectSfx);
                }

                collector.CollectBone();
                Destroy(gameObject, collectSfx.length);
            }
        }
    }
}