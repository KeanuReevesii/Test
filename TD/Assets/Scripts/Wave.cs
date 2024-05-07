using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//保存每一波敌人生成所需属性
[System.Serializable]
public class Wave  {
    //敌预制体
    public GameObject enemyPrefab;
    //当波数量
    public int count;
    //敌生成速率
    public float rate;
}
