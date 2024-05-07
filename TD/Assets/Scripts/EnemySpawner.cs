using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    //敌存活数
    public static int countEnemyAlive = 0;
    //敌进攻波次的数组 
    public Wave[] waves;
    //敌人生成点
    public Transform start;
    //波次速率
    public float waveRate = 0.2f;
    //携程
    private Coroutine coroutine;

    // Use this for initialization
    private void Start()
    {
        //开始携程，并将接收返回值
        coroutine=StartCoroutine(SpawnEnemy());
    }

    //游戏失败后，停止敌人的生成
    public void Stop()
    {
        //StopAllCoroutines();
        //利用携程返回值来停止携程的方法
        StopCoroutine(coroutine);
    }

    //携程，控制敌人波次
    IEnumerator SpawnEnemy()
    {
        //遍历每一波
        foreach(Wave  wave in waves)
        {//遍历每一波敌数量
            for(int i=0;i<wave.count;i++)
            {//生成敌人，敌存活数++
                GameObject.Instantiate(wave.enemyPrefab, start.position, Quaternion.identity);
                countEnemyAlive++;
                //敌数量未满
                if(i!=wave.count-1)
                    //携程等待，然后再生成下一个敌人
                   yield return new WaitForSeconds(wave.rate);
            }
            //当敌还有存活
            while(countEnemyAlive>0)
            //携程暂时返回，直到这一波敌人死光
                yield return 0;
            //敌人死光了，就等待生成下一波敌人
            yield return new WaitForSeconds(waveRate);
        }
        //当波次完了，敌人还有存活
        while (countEnemyAlive>0)
        {
            //携程返回
            yield return 0;
        }
        //当波次完了，敌人死光，调用胜利函数
        GameManager.Instance.Win();
    }
   
}
