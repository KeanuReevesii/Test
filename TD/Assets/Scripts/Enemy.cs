using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
    //道路拐点数组 
    private Transform[] positions;
    //经过拐点即计数 ，计数器
    private int index = 0;
    //行进速度 
    public float speed = 10;
    //当前血量
    public float hp = 150f;
    //总血量 
    private float totalHp;
    //爆炸 特效 
    public GameObject explosionEffect;
    //血条 UI
    private Slider hpSlider;
	// Use this for initialization
	void Start () {
        //得到WayPoints物体底下的拐点数组 
        positions = WayPoints.positions;
	    totalHp = hp;
	    hpSlider = GetComponentInChildren<Slider>();
	}
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    //敌人移动方法 
    void Move()
    {
        //保险起见，计数器大过拐点索引，退出 调用 
        if (index > positions.Length - 1) return;
        //向朝着拐点移动  
        transform.Translate((positions[index].position - transform.position).normalized * Time.deltaTime * speed);
        //当到达 拐点，计数器++
        if (Vector3.Distance(positions[index].position, transform.position) < 0.2f)
            index++;
        //计数器大过拐点数组索引，处理游戏结果
        if(index>positions.Length-1)
            ReachDestination();
    }

    //敌人到达终点的处理方法
    void ReachDestination()
    {//销毁到达终点的敌人
        GameObject.Destroy(this.gameObject);
        //调用游戏失败方法
        GameManager.Instance.Lose();
    }

    //当有敌人被 销毁的时候
     void OnDestroy()
    {//敌存活数--
        EnemySpawner.countEnemyAlive--;
    }

    //敌人的扣血方法
    public void TakeDamage(float damage)
    {
        //敌人血量<=0 ，直接退出调用，保险起见
        if (hp <= 0) return;
        hp -= damage;
        //改变血条UI的显示
        hpSlider.value = (float)hp / totalHp;
        //死亡
        if (hp <= 0)
            Die();
    }

    //敌死亡
    void Die()
    {
        GameObject effect = GameObject.Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(effect, 1.5f);
        Destroy(this.gameObject);

    }
}
