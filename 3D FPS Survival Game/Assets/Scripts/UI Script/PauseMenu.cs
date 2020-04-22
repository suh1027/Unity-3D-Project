using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject go_BaseUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!GameManager.isPause)
            {
                CallMenu();
            }
            else
            {
                CloseMenu();
            }
        }
    }

    private void CallMenu()
    {
        GameManager.isPause = true;
        go_BaseUI.SetActive(true);

        //시간이 정지하도록 설정!
        Time.timeScale = 0f;
    }
    private void CloseMenu()
    {
        GameManager.isPause = false;
        go_BaseUI.SetActive(false);

        Time.timeScale = 1f; //1배속으로 다시 시간 흐르도록 설정
    }

    public void ClickSave() // 저장
    {
        Debug.Log("저장");
    }

    public void ClickLoad() // 로딩
    {
        Debug.Log("로드");
    }

    public void ClickExit() // 종료
    {
        Debug.Log("종료");
    }
}
