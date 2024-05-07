using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

   //进入攻击范围的敌人列表
    private List<GameObject> enemys = new List<GameObject>();

    //当敌人进入炮台攻击范围触发器
    private void OnTriggerEnter(Collider col)
    {
        //如果tag(标签)是敌人
        if (col.tag == "Enemy")
            //添加至敌人列表
            enemys.Add(col.gameObject);
    }

    //当敌人 离开炮台攻击范围
    private void OnTriggerExit(Collider col)
    {//从列表中移除敌人
        if (col.tag == "Enemy")
            enemys.Remove(col.gameObject);
    }

    //线渲染器，渲染激光
    private LineRenderer laserRenderer;

    //攻速，炮台攻击冷却时间
    public float attackRatetime = 1;
    //定时器，用来计算冷却时间的流逝
    private float timer = 0;

    //炮弹预制体 
    public GameObject bulletPrefab;
    //炮弹生成位置，即开火位置
    public Transform firePosition;
    //炮台头转动部分
    public Transform head;

    //判断是否为激光（感觉应该使用turretData的枚举类更好，但是游戏架构得大改。。）
    public bool isLaser = false;
    //激光伤害速率
    public float laserDamageRate = 70f;
    //激光击中敌人特效
    public GameObject laserEffect;

    // Use this for initialization
    void Start () {
        timer = attackRatetime;
        laserRenderer = GetComponentInChildren<LineRenderer>();
      //  laserEffect =GameObject.Find("LaserEffect");
    }
	
	// Update is called once per frame
	void Update () {
        //当敌人列表不为空
	    if (enemys.Count > 0 && enemys[0] != null)
	    {
            //定义目标，让炮头始终转向目标
	        Vector3 targetPos = enemys[0].transform.position;
            try  //异常处理，目标可能被打死了，会报空指针异常 
            {
                //y相等可以保持炮头相对水平
                targetPos.y = head.position.y;
                head.LookAt(targetPos);
            }
            catch (Exception) { }    
	    }
        //如果不是激光 
	    if (!isLaser)
	    {
            //定时器计时
	        timer += Time.deltaTime;
            //敌人死光了，重置定时器
	        if (enemys.Count == 0)
	            timer = attackRatetime;
            //敌人还有的话 ，且定时器达到冷却尽头
	        else if (enemys.Count > 0 && timer >= attackRatetime)
	        {
                //让定时器进入冷却状态
	            timer -= attackRatetime;
                //攻击
	            Attack();
	        }
	    }

        //如果是激光且敌人还有
	    else if(enemys.Count>0)
	    {
            //激光攻击
	        LaserAttack();
        }
        //如果敌人死光了 
	    else
	    {//关闭激光特效和渲染器
            laserEffect.SetActive(false);
	        laserRenderer.enabled = false;
        }
	


	}

    //激光攻击方法
    void LaserAttack()
    {
        //启用激光渲染器和特效
        if (laserRenderer.enabled == false)
            laserRenderer.enabled = true;
        laserEffect.SetActive(true);
        //第一个敌人被打死了
        if (enemys[0] == null)
        //换下一个
            UpdateEnemys();
        //敌人还没死光
        if (enemys.Count > 0)
        {//画线，即画激光的起点和终点
            laserRenderer.SetPositions(new Vector3[]{firePosition.position ,enemys[0].transform.position});
            //敌人每秒受伤，调用Enemy类的扣血方法 
            enemys[0].GetComponent<Enemy>().TakeDamage(laserDamageRate*Time.deltaTime);
            //设定特效播放位置
            laserEffect.transform.position = enemys[0].transform.position;
            //设定炮头朝向敌人
            Vector3 pos = transform.position;
            pos.y = enemys[0].transform.position.y;
            laserEffect.transform.LookAt(pos);
        }
        
    }

    //炮弹攻击方法
    void Attack()
    {//第一个敌人被打死，换下一个
        if (enemys[0] == null)
            UpdateEnemys();
        //敌人还没死完
        if (enemys.Count > 0)
        {//生成炮弹
            GameObject bullet = GameObject.Instantiate(bulletPrefab, firePosition.position, firePosition.rotation);
            //调用炮弹目标设定
            bullet.GetComponent<Bullet>().SetTarget(enemys[0].transform);
        }
        else //敌人死完了，重置定时器 
            timer = attackRatetime;
    }

    //第一个敌人被打死之后 更新敌人列表的第一个敌人
    void UpdateEnemys()
    {
        //储存当前敌人列表中敌人为空的元素索引
        List<int> emptyIndex = new List<int>();
        //遍历，空就添加到索引
        for (int i = 0; i < enemys.Count; i++)
            if (enemys[i] == null)  
                emptyIndex.Add(i);
        //遍历，更新敌人列表
        for (int j = 0; j < emptyIndex.Count; j++)
            //emptyIndex[j]-j,表示为空的敌人在前面已经移除了j个，所以空索引列表(emptyIndex)里面的数字也要更新
            enemys.RemoveAt(emptyIndex[j]-j);
    }
}
