using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour {
    //每种炮台储存的数据
    public TurretData laserTurretDate;
    public TurretData missileTurretDate;
    public TurretData standardTurretData; 
    //表示选定将要建造的炮台
    public TurretData selectedTurretData;

    //金钱数目文本
    public Text moneyText;
    //金钱UI动画
    public Animator moneyFlicker;
    //金钱数量
    private int money = 1000;

    //炮台升级或拆除的UI
    public GameObject optionCanvas;
    //升级按钮
    public Button upgradeBtn;
    //表示选中场景中已建造炮台的方块身上的MapCube脚本组件
    private MapCube selectedMapCube;
    //炮台拆除或升级的UI动画
    private Animator optionCanvasAnimator;

    //控制金钱数量变化
    void ChangeMoney(int change = 0)
    {
        money += change;
        moneyText.text = "￥" + money;
    }

    void Start()
    {
        optionCanvasAnimator = optionCanvas.GetComponent<Animator>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //判别 UI是否遮挡游戏物体：EventSystem.current.IsPointerOverGameObject
            //==false表示没有遮挡
            if (EventSystem.current.IsPointerOverGameObject()==false)
            {
                //开发炮台的建造
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                //检测碰撞到的层是否为地板方块
                bool isCollider= Physics.Raycast(ray,out hit, 1000, LayerMask.GetMask("MapCube"));
                if (isCollider)
                {
                    //获取MapCube类对象 
                    MapCube mapCube = hit.collider.GetComponent<MapCube>();
                    //如果方块上面没有炮台，并且已经选中要建造的炮台了
                    if(mapCube.turretGo==null && selectedTurretData.turretPrefab != null)
                    {
                        //可以创建，并先判断金钱数目
                        if(money>selectedTurretData.cost)
                        {
                            //减少金钱数目并调用建造方法
                            ChangeMoney(-selectedTurretData.cost);
                            mapCube.BuildTurret(selectedTurretData);    
                        }
                        else
                        {//金钱不足时播放UI警告动画
                            moneyFlicker.SetTrigger("Flicker");
                        }
                    }
                    //如果方块上已经建有炮台了
                    else if(mapCube.turretGo!=null)
                    {
                        //当再点击一下炮台且升级或拆除炮台的UI还未隐藏 
                        if(mapCube==selectedMapCube&&optionCanvas.activeInHierarchy)
                        {
                            //开启携程来隐藏UI
                            StartCoroutine(HideOptionUI());
                        }
                        else
                        {//否则就显示UI ，传入显示位置，还有是否 升级的判定，用于控制升级按钮的弃用
                            ShowOptionUI(mapCube.transform.position, mapCube.isUpgraded);
                        }
                        selectedMapCube = mapCube;
                    }
         
                }
            }
        }
    }


   
    //在选建面板中选中激光事件
    public void OnLaserSelected(bool isOn)
    {
        if(isOn)
            //更新选定将要建造的炮台数据
            selectedTurretData = laserTurretDate;
    }
    //在选建面板中选中导弹事件
    public void OnMissileSelected(bool isOn)
    {
        if (isOn)
            selectedTurretData = missileTurretDate;
    }
    //在选建面板中选中标准炮台事件
    public void OnStandardSelected(bool isOn)
    {
        if (isOn) selectedTurretData = standardTurretData;
    }

    //显示升级或拆除UI的函数，第一个参数是显示位置 ，第二个是升级按钮是否弃用
    void ShowOptionUI(Vector3 pos, bool isDisable=false)
    {
        //结束携程是处理隐藏UI动画播放中又同时要显示UI的冲突
        StopAllCoroutines();
        //手动隐藏
        optionCanvas.SetActive(false);
        //再启用回来
        optionCanvas.SetActive(true);
        //让 UI显示在炮台位置
        optionCanvas.transform.position = pos;
        //将炮台的升级状态传给升级按钮来决定弃用与否
        upgradeBtn.interactable = !isDisable;
    }
    //携程控制升级或拆除炮台的UI隐藏动画的播放
    IEnumerator HideOptionUI()
    {
        optionCanvasAnimator.SetTrigger("Hide");
        //延迟 0.35s 等待动画播放完毕
        yield return new WaitForSeconds(0.35f);
        //隐藏UI
        optionCanvas.SetActive(false);
    }

    //炮台升级按钮事件
    public void OnUpgradeBtnDown()
    {
        //selectedMapCube作为MapCube实例调用其中的升级函数
        selectedMapCube.UpgradeTurret();
        //升级完隐藏UI
        StartCoroutine( HideOptionUI());
    }
    //炮台销毁事件
    public void OnRemoveBtnDown()
    {
        selectedMapCube.RemoveTurret();
       StartCoroutine( HideOptionUI());
    }

}
