using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static bool canPlayerMove = true;    // 플레이어 움직임 제어
    public static bool isOpenInventory = false; // 인벤토리 활성화
    public static bool isCraftManual = false; // 건축 메뉴창 활성화

    public static bool isNight = false;
    public static bool isWater = false;


    private WeaponManager theWeaponManager;
    private bool flag = false; // 물에서 계속 실행되지 않게 하기위해

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        theWeaponManager = FindObjectOfType<WeaponManager>();
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

        if (isWater)
        {
            if (!flag)
            {
                StopAllCoroutines(); // 코루틴 중복실행을 막음
                flag = true;
                StartCoroutine(theWeaponManager.WeaponInCoroutine());
            }
        }
        else
        {
            if (flag)
            {
                flag = false;
                theWeaponManager.WeaponOut();
            }
        }
    }

    
}
