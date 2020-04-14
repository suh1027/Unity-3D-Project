using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    // 필요 컴포넌트
    [SerializeField]
    private GunController theGunController;
    private Gun currentGun;

    // 필요하면 HUD 호출, 필요없으면 HUD 비활성화 하는 역할
    [SerializeField]
    private GameObject go_BulletHUD;

    // Bullet UI 내 총알 갯수 수정 Text
    [SerializeField]
    private Text[] text_Bullet;

    void Update()
    {
        CheckBullet();        
    }

    private void CheckBullet()
    {
        currentGun = theGunController.GetGun();
        
        text_Bullet[0].text = currentGun.carryBulletCount.ToString();
        text_Bullet[1].text = currentGun.reloadBulletCount.ToString();
        text_Bullet[2].text = currentGun.currentBulletCount.ToString();

    }
}
