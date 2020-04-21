using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static bool canPlayerMove = true;    // 플레이어 움직임 제어
    public static bool isOpenInventory = false; // 인벤토리 활성화
    public static bool isCraftManual = false; // 건축 메뉴창 활성화

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    
   private void Update()
    {
        if (isOpenInventory || isCraftManual)
        {
            canPlayerMove = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            canPlayerMove = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    
}
