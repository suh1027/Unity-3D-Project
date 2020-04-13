using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName; // 총 이름
    public float range; // 사정거리
    public float accuracy; // 총의 정확도
    public float fireRate; // 연사 속도
    public float reloadTime; // 재장전 속도
    
    
    public int damage; // 총의 데미지

    //총알 관리
    public int reloadBulletCount; // 총알 재장전 갯수
    public int currentBulletCount; // 현재 탄알집에 남아있는 총알의 갯수
    public int maxBulletCount; // 최대 소유 가능 총알 갯수
    public int carryBulletCount; // 현재 소유하고 있는 총알의 갯수


    // 반동 관리
    public float retroActionForce; // 반동 세기
    public float retroActionFineSightForce; // 정조준 반동 세기

    public Vector3 fineSightOriginPos;
    public Animator animator;
    //총알 이펙트 관리 (총구 섬광) #ParticleSystem...
    public ParticleSystem muzzleFlash;

    //사운드관리
    public AudioClip fire_Sound;
}
