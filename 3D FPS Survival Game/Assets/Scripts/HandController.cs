﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    // 현재 장착된 Hand형 타입(무기)
    [SerializeField]
    private Hand currentHand;

    // 공격 확인 변수
    private bool isAttack = false;
    private bool isSwing = false;

    private RaycastHit hitInfo; // Ray에 닿은 컴포넌트의 정보를 얻어올 수 있는 변수

    // Update is called once per frame
    void Update()
    {
        TryAttack();
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
        currentHand.animator.SetTrigger("Attack"); // Attack trigger를 발동

        yield return new WaitForSeconds(currentHand.attackDelayA);
        isSwing = true;

        //공격 활성화 시점
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentHand.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currentHand.attackDelay - currentHand.attackDelayA - currentHand.attackDelayB);
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
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentHand.range))
        {
            return true;
        }
        return false;
    }
}
