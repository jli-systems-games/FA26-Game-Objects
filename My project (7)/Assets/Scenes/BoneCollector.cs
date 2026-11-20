using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneCollector : MonoBehaviour
{
    public int boneCount = 0;
    public int winBoneAmount = 5; //收集4根骨头获胜
    public GameObject winPanel; //面板，拖进这个框

    void Start()
    {

    }

    // 被骨头调用，收集骨头
    public void CollectBone()
    {
        boneCount++;
        Debug.Log("收集骨头，当前数量：" + boneCount);

        if (boneCount >= winBoneAmount)
        {
            //开启胜利面板
            winPanel.SetActive(true);
        }
    }
}