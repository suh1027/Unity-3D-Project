using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : CloseWeaponController
{
    // 활성화
    public static bool isActivate = false;
    
    void Start()
    {
        //WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        //WeaponManager.currentWeaponAnim = currentCloseWeapon.animator;
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
                // 적중한 것이 있으면 isSwing을 꺼줌
                isSwing = false;

                // 충돌함
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
