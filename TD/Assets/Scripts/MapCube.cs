using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.EventSystems;

public class MapCube : MonoBehaviour {

   [HideInInspector]
    public  GameObject turretGo; //保存当前Cube身上的炮台物体 
    [HideInInspector]
    public  bool isUpgraded = false;  //判断Cube身上 的炮台升级与否 
    public GameObject buildEffect;  //建造的粒子特效
    private TurretData turretData;    //炮台数据
    private MeshRenderer meshRenderer;  //renderer用来获取材质
    // Use this for initialization
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    //炮台建造方法
    public void BuildTurret(TurretData turretData)
    {//储存参数的炮台数据
        this.turretData = turretData;
        isUpgraded = false;
        //生成炮台物体，其中的turretData保存了炮台的预制体
        turretGo = GameObject.Instantiate(turretData.turretPrefab, this.transform.position, Quaternion.identity);
        //生成粒子特效
        GameObject effect = GameObject.Instantiate(buildEffect, new Vector3( this.transform.position.x,this .transform.position.y+0.5f,this.transform.position.z), Quaternion.identity);
        //特效播放完之后销毁
        Destroy(effect, 1.5f);
    }

    //升级炮台方法
    public void UpgradeTurret( )
    {
        //保险起见先判断下炮台要是升级过了 ，就直接跳出
        if (isUpgraded == true) return;
        //炮台升级过了
        isUpgraded = true;
        //销毁原来的炮台 
        Destroy(turretGo);
        //生成新的炮台，其中的turretData保存了升级版炮台的预制体
        turretGo = GameObject.Instantiate(turretData.turretUpgradedPrefab, this.transform.position, Quaternion.identity);
        //生成粒子特效
        GameObject effect = GameObject.Instantiate(buildEffect, new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z), Quaternion.identity);
        //播完销毁
        Destroy(effect, 1.5f);
    }

    //移除炮台的方法
    public void RemoveTurret()
    {
        //销毁炮台并置空一些变量
        Destroy(turretGo);
        turretGo = null;
        isUpgraded = false;
        turretData = null;
        //粒子特效 的生成和销毁
        GameObject effect = GameObject.Instantiate(buildEffect, new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z), Quaternion.identity);
        Destroy(effect, 1.5f);
    }

    //在地砖（cube）身上 添加鼠标滑过就变红的效果
    void OnMouseEnter()
    {//当cube没有炮台，且鼠标不被UI阻挡的情况
        if (turretGo == null&&EventSystem.current.IsPointerOverGameObject()==false)
        {   //让renderer组件中的 材质颜色变红
            meshRenderer.material.color=Color.red;
        }
    }

    //鼠标移出地砖
    void OnMouseExit()
    {//变白
        meshRenderer.material.color = Color.white;
    }

	

}
