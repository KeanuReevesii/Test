using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    //加载游戏的loadingUI
    public GameObject loading;

    //开始游戏按钮
    public GameObject start;

    //开始游戏按钮的事件
    public void OnStartGame()
    {//携程调用场景 加载
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        loading.SetActive(true);
        //用土办法使按钮动画常播放
        yield return new WaitForSeconds(0.5f);
        start.SetActive(false);
        start.SetActive(true);
        //等待三秒后异步加载游戏场景 
        yield return new WaitForSeconds(3); ;
        SceneManager.LoadSceneAsync(1);
       
    }

    //宏定义？？？
    //结束游戏按钮事件
    public void OnExitGame()
    {
        //宏定义，在unity编辑器里面的游戏退出
        #if  UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; 
        //导出exe之后的游戏退出
       #else
              Application.Quit();
        #endif

    }
}
