using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeapon : MonoBehaviour
{
    public string closeWeaponName;

    // Weapon 유형
    public bool isHand;
    public bool isAxe;
    public bool isPickAxe;

    public string handName; //맨손, 너클 구분
    public float range; // 공격 범위
    public float damage; // 공격력
    public float workSpeed; // 작업 속도
    public float attackDelay; // 공격 딜레이
    public float attackDelayA; // 공격 활성화 시점 => 주먹을 뻗는 시점
    public float attackDelayB; // 공격 비활성화 시점 => 주먹을 빼는 시점

    public Animator animator;
    
    //BoxCollider를 안쓰는 이유 -> 1인칭 시점에서 실제 접촉은 화면상 가운데 에임에 맞도록 구현해야 하기 떄문
    //팔을 BoxCollider를 설정하여 구성하면 화면상 안맞는부분이 발생할 가능성이 있음
}
