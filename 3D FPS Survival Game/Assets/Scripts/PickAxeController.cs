using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAxeController : CloseWeaponController
{
    public static bool isActivate = true;

    void Start()
    {
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.animator;
    }

    void Update()
    {
        if (isActivate)
            TryAttack();
    }
    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                if(hitInfo.transform.tag == "Rock")
                {
                    hitInfo.transform.GetComponent<Rock>().Mining();
                }

                else if(hitInfo.transform.tag == "NPC")
                {
                    SoundManager.instance.PlaySE("AnimalHit");
                    hitInfo.transform.GetComponent<Pig>().Damage(1, transform.position); 
                    // pig -> Animal로 만들어 pig를 상속받게 만들어서 일반화 !
                }
                
                // 적중한 것이 있으면 isSwing을 꺼줌
                isSwing = false;

                

                // 충돌하는지 확인 디버그
                Debug.Log(hitInfo.transform.name);
            }

            else
            {
                // 충돌하지 않음

            }
            yield return null; // while문 한번당 1프레임 대기
        }
    }

    public override void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        base.CloseWeaponChange(_closeWeapon);
        isActivate = true;
    }
}
