using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TurretData {
    public GameObject turretPrefab; //炮台预制体 
    public int cost;    //炮台的建造花费
    public GameObject turretUpgradedPrefab; //升级版炮台预制体
    public int Upgradedcost;   //升级花费 
    public TurretType type; //炮台类型，然而定义了变量却没有使用
}

//炮台类型枚举，然而本游戏却并没有用到
//用来判断炮台类型还是蛮不错的
public enum TurretType
{
    LaserTurret,
    MissileTurret,
    StandardTurret
}
