using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour {
    //道路拐点数组
    public static Transform[] positions;

     void Awake()
    {
        //脚本挂在拐点集合上，所以用这种方式来初始化数组
        positions = new Transform[transform.childCount];
        //遍历，得到子物体即拐点赋给数组
        for (int i=0;i<positions.Length;i++)
        {
            positions[i] = transform.GetChild(i);
        }
    }
}
