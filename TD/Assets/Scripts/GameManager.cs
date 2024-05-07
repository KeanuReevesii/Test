using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using UnityEngine;
using  UnityEngine.UI;
using  UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    //loadingUI
    public GameObject loading;
    //游戏结束的UI
    public GameObject endUI;
    //结束信息
    public Text endMessage;

   //单例模式 
    public static GameManager Instance;

    //控制敌人波次的对象
    private EnemySpawner enemySpawner;

	// Use this for initialization
	void Awake ()
	{
	    Instance = this;
	    enemySpawner = GetComponent<EnemySpawner>();
	}

    //胜利方法
  public   void Win()
    {
        //显示UI，输出胜利信息 
        endUI.SetActive(true);
        endMessage.text = "YOU WIN!!";
    }

    //失败方法
     public   void Lose()
    {
        //停止敌人生成
        enemySpawner.Stop();
        endUI.SetActive(true);
        endMessage.text = "GAME OVER!!";
    }

    //再玩一遍按钮的事件
    public void OnReplayBtn()
    {
        StartCoroutine(LoadScene());
    }

    //返回菜单的按钮事件
    public void OnMenuBtn()
    {
        StartCoroutine(LoadMenu());
    }


    IEnumerator LoadScene()
    {
        loading.SetActive(true);
        endUI.SetActive(false);
        //等待三秒后异步加载游戏场景 
        yield return new WaitForSeconds(3); 
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        //获取 当前活跃 场景的索引再load一遍
    }

    IEnumerator LoadMenu()
    {
        loading.SetActive(true);
        endUI.SetActive(false);
        //等待三秒后异步加载菜单场景 
        yield return new WaitForSeconds(3); 
        SceneManager.LoadSceneAsync(0);
      
    }
}
