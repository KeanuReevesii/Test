using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //炮弹伤害值和速度
    public int damage = 50;
    public float speed = 20;

   //炮弹爆炸特效
    public GameObject explosionEffectPref;
    //炮弹射击目标 
    private Transform target;

    //设定炮弹将击中的目标
    public void SetTarget(Transform _target)
    {
        this.target = _target;
    }

	// Update is called once per frame
	void Update () {
        //当目标为空，炮弹 自动爆炸 并退出函数
	    if (target == null)
	    {
            Die();
	        return;
	    }
        //炮弹朝向敌人并向敌人移动
	    transform.LookAt(target.position);
        transform .Translate(Vector3.forward*speed*Time.deltaTime);
    }

    //当炮弹 击中 敌人 
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Enemy")
        {//调用敌人身上的 扣血函数
            col.GetComponent<Enemy>().TakeDamage(damage);
            //爆炸
            Die();
        }
    }

    //炮弹爆炸函数
    void Die()
    {
        //播放和摧毁特效，摧毁炮弹
        GameObject explosion = GameObject.Instantiate(explosionEffectPref, transform.position, transform.rotation);
        Destroy(this.gameObject);
        Destroy(explosion, 0.5f);
    }

    
}
