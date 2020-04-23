using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string sceneName = "GameStage"; // 다음 씬으로 넘어갈 함수

    public static Title instance;

    private SaveAndLoad theSaveAndLoad;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this.gameObject); // 싱글톤화 => ..,..?
    }

    public void ClickStart()
    {
        Debug.Log("로딩");
        Destroy(this.gameObject);
        SceneManager.LoadScene(sceneName);
    }

    public void ClickLoad()
    {
        Debug.Log("로드");
        
        //SceneManager.LoadScene(sceneName);

        //로딩시간을 정해줘야..
        StartCoroutine(LoadCoroutine());

        
    }

    IEnumerator LoadCoroutine()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while(!operation.isDone){
            yield return null;
        }
        
        theSaveAndLoad = FindObjectOfType<SaveAndLoad>();
        
        Destroy(gameObject);
        theSaveAndLoad.LoadData();// 싱글톤화 했기떄문에 이함수가 실행됨..?
        

    }

    public void ClickExit()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }
}
