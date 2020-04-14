using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : CloseWeaponController
{
    // 활성화
    public static bool isActivate = false;

    void Update()
    {
        if (isActivate)
            TryAttack();
    }
    /*
        // 활성화
        public static bool isActivate = false;


        // 현재 장착된 Hand형 타입(무기)
        [SerializeField]
        private CloseWeapon currentCloseWeapon;

        // 공격 확인 변수
        private bool isAttack = false;
        private bool isSwing = false;

        private RaycastHit hitInfo; // Ray에 닿은 컴포넌트의 정보를 얻어올 수 있는 변수

        // Update is called once per frame
        void Update()
        {
            if (isActivate) { TryAttack(); }

        }

        private void TryAttack()
        {
            if (Input.GetButton("Fire1")) // Fire1 -> 좌클릭 //mouse 0
            {
                Debug.Log("Attack!");
                if (!isAttack)
                {
                    StartCoroutine(AttackCoroutine()); 
                }
            }
        }

        IEnumerator AttackCoroutine()
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

        IEnumerator HitCoroutine()
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

        private bool CheckObject()
        {
            //충돌 여부 확인해서 True False 리턴
            if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range))
            {
                return true;
            }
            return false;
        }

        public void CloseWeaponChange(CloseWeapon _closeWeapon)
        {
            if (WeaponManager.currentWeapon != null)
                WeaponManager.currentWeapon.gameObject.SetActive(false);

            currentCloseWeapon = _closeWeapon;
            WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
            WeaponManager.currentWeaponAnim = currentCloseWeapon.animator;

            currentCloseWeapon.transform.transform.localPosition = Vector3.zero;
            currentCloseWeapon.gameObject.SetActive(true);
            isActivate = true;
        }*/
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
