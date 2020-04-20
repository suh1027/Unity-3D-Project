using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CloseWeaponController : MonoBehaviour // 추상클래스
{


    // 현재 장착된 Hand형 타입(무기)
    [SerializeField]
    protected CloseWeapon currentCloseWeapon; // protected -> 상속받은 클래스만 사용가능한 접근제한자

    // 공격 확인 변수
    protected bool isAttack = false;
    protected bool isSwing = false;

    protected RaycastHit hitInfo; // Ray에 닿은 컴포넌트의 정보를 얻어올 수 있는 변수
    [SerializeField]
    protected LayerMask layerMask;

    // Update is called once per frame
    /*protected void Update()
    {
        if (isActivate) { TryAttack(); }

    }*/

    protected void TryAttack()
    {
        if (!Inventory.inventoryActivated)
        {
            if (Input.GetButton("Fire1")) // Fire1 -> 좌클릭 //mouse 0
            {
                //Debug.Log("Attack!");
                if (!isAttack)
                {
                    StartCoroutine(AttackCoroutine());
                }
            }
        }
    }

    protected IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentCloseWeapon.animator.SetTrigger("Attack"); // Attack trigger를 발동

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayA);
        isSwing = true;

        //공격 활성화 시점
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayA - currentCloseWeapon.attackDelayB);
        isAttack = false;
    }

    protected abstract IEnumerator HitCoroutine(); // 역할이 달라지는 기능 => 추상 코루틴으로 구현
    /*{
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
    }*/

    protected bool CheckObject()
    {
        //충돌 여부 확인해서 True False 리턴
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range, layerMask))
        {
            return true;
        }
        return false;
    }

    public virtual void CloseWeaponChange(CloseWeapon _closeWeapon) 
        // public virtual 완성 함수 이지만 , 추가 편집이 가능한 함수
    {
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);

        currentCloseWeapon = _closeWeapon;
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.animator;

        currentCloseWeapon.transform.transform.localPosition = Vector3.zero;
        currentCloseWeapon.gameObject.SetActive(true);
        
    }
}

