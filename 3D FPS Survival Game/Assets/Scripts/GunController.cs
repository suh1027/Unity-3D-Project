using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    // 활성화
    public static bool isActivate = false;

    [SerializeField]
    private Gun currentGun;

    private float currentFireRate;

    // 효과음 재생
    private AudioSource audioSource;

    // 재장전 bool 변수
    private bool isReload = false;
    // 정조준 bool 변수
    private bool isFineSightMode = false;

    // 본래 Position 값
    [SerializeField]
    private Vector3 originPos;

    // 총알 충돌 정보
    private RaycastHit hitInfo;

    // 크로스헤어를 위한 카메라 정보
    [SerializeField]
    private Camera theCam;
    private CrossHair theCrossHair;


    // 피격 이펙트
    [SerializeField]
    private GameObject hit_effect_prefab;


    //뛸때는 shoot()이 안되게 anim 조작을 위해 curruentgun의 애니메이션의 ..

    void Start()
    {
        originPos = Vector3.zero;
        audioSource = GetComponent<AudioSource>();
        theCrossHair = FindObjectOfType<CrossHair>();

/*        WeaponManager.currentWeapon = currentGun.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentGun.animator;*/
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivate)
        {
            GunFireRateCalc();
            TryFire();
            TryReload();
            TryFineSight();
        }
    }



    // #1. 연사속도 계산

    private void GunFireRateCalc()
    {
        if(currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime; // 1초 / 역수 대략 60분의 1
        }
    }


    // #2. 총알 발사


    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            // 누르고 있으면 총 발사
            Fire();
        }
    }

    private void Fire() // 방아쇠를 당기고 나서 연사속도 만큼 총알 한발 처리 과정
    {
        if (!isReload && !currentGun.animator.GetBool("Run")) { // 뛰고있을때는 발사가 되게는 하되 애니메이션을 중단시켜야..하는지
            if (currentGun.currentBulletCount > 0)
                Shoot();
            else 
            {
                CancelFineSight(); // 총알이 0 발일때 발사하면 정조준 해제
                StartCoroutine(ReloadCoroutine());
            }
            //Reload();
                
    
        }
    }
    private void Shoot()
    {
        // 크로스헤어 애니메이션 
        theCrossHair.FireAnimation();
        
        //연사속도 재계산
        currentFireRate = currentGun.fireRate;

        // 총 발사
        // 1) MuzzlePlay ParticleSystem 실행
        currentGun.muzzleFlash.Play();

        // 2) Audio 실행
        PlaySE(currentGun.fire_Sound);

        // 3) 탄알집 총알 -1
        currentGun.currentBulletCount--;

        Hit(); 
        //오브젝트 풀링을 사용하면 렉이 다운되긴 하지만 그냥 구현

        // 4) 총기 발사 반동
        StopAllCoroutines(); // while 문 두개가 경쟁하는것을 막기위해 coroutine을 stop 시킴
        StartCoroutine(RetroActionCoroutine());

    }

    private void Hit() // 적중되는 위치
    {
        if(Physics.Raycast(theCam.transform.position, 
            theCam.transform.forward +  
            new Vector3(
            Random.Range(-theCrossHair.GetAccuracy() - currentGun.accuracy, theCrossHair.GetAccuracy() - currentGun.accuracy),
            Random.Range(-theCrossHair.GetAccuracy() - currentGun.accuracy, theCrossHair.GetAccuracy() - currentGun.accuracy), // Random.Range를 통해 반동 구현
            0), // 랜덤하게 수치 변화 
            out hitInfo, 
            currentGun.range))
        {
            //Debug.Log(hitInfo.transform.name);
            //Quaternion.LookRotation(hitInfo.normal) -> 위를 바라보는 곳으로 생성
            GameObject clone = Instantiate(hit_effect_prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            // 이해가 잘 되진 않은...
            // 메모리에 계속 쌓여 이걸 제거하는 함수 필요
            Destroy(clone, 2f); // 2초뒤에 clone을 파괴
        }
    }

    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    IEnumerator RetroActionCoroutine()
    {
        // 평상시 반동 x축을 흔들어 반동을 줌
        Vector3 recoilBack = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z);

        // 정조준시 반동
        Vector3 retroActionRecoilBack = new Vector3(
            currentGun.retroActionFineSightForce, 
            currentGun.fineSightOriginPos.y, 
            currentGun.fineSightOriginPos.z);
        // 이 Vector3는 미리 선언한뒤, Start함수에서 값을 매겨주는 것이 좋다 -> 메모리 단편화 방지 목적

        if (!isFineSightMode)
        {
            // 정조준이 아닐경우
            // 발사때마다 반동이 처음으로 되돌려서 눈에 띄게 하기위해             
            currentGun.transform.localPosition = originPos;

            // 반동 시작
            while (currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f) 
                // 0.02만큼의 여유를 주어 while을 빠져나오게 함
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }

            // 원 위치

            while (currentGun.transform.localPosition != originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
                yield return null; 
            }
        }
        else
        {
            //정조준시 위치로 되돌림
            currentGun.transform.localPosition = currentGun.fineSightOriginPos;

            // 반동 시작
            while (currentGun.transform.localPosition.x <= currentGun.retroActionFineSightForce - 0.02f)
            // 0.02만큼의 여유를 주어 while을 빠져나오게 함
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }

            // 원 위치
            while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(
                    currentGun.transform.localPosition,
                    currentGun.fineSightOriginPos, 0.1f);
                yield return null;
            }
        }

    }



    // #3. 재장전


    // -> *************** 재장전 할때마다 정조준 위치가 바뀌는 버그 발생 ************ 
    // -> 위치는 바뀌지 않았는데 화면상 위치가 변경되는 ???...
    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) 
            && !isReload 
            && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            //StartCoroutine(FineSightDeactivateCoroutine());
            CancelFineSight(); // 정조준을 해제하면서 Reload
            StartCoroutine(ReloadCoroutine());
        }
    }

    IEnumerator ReloadCoroutine() 
        // private void Reload() 
        // 이렇게 구현시에 대기시간을 주지않아 재장전 도중 발사가 되는 오류 발생
        // -> Coroutine 으로 구현
    {
        isReload = true;

        if (currentGun.carryBulletCount > 0)
        {
            //Reload trigger 실행 -> animation 발동 트리거
            currentGun.animator.SetTrigger("Reload");
            
            //현실적인 전체총알의 갯수 계산을 위해
            // 전체 탄알 갯수는 += 현재 장착중인 총알
            // 장착중인 총알 = 0
            currentGun.carryBulletCount += currentGun.currentBulletCount;
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime); // 대기시간 지정

            if(currentGun.carryBulletCount >= currentGun.reloadBulletCount)
            // 현재 소유하고 있는 총알의 갯수 >= 한번에 재장전 되는 총알의 갯수
            {
                // 탄알집의 갯수 = 총알 재장전 갯수
                // 전체 탄알 갯수는 = 전체탄알갯수 - 재장전되는 총알 갯수
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;
            }
            else
            {
                //현재 탄알집의 총알 수 = 전체 탄알집의 총알 수
                //전체 탄알집의 총알 수 = 0
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                currentGun.carryBulletCount = 0;
            }

            isReload = false;
        }
        else
        {
            // 총알 없을때
            Debug.Log("소유한 총알이 없습니다.");
        }       
    }

    public void CancelReload()
    {
        if (isReload)
        {
            StopAllCoroutines();
            isReload = false;
        }
    }



    // #4. 정조준


    private void TryFineSight()
    {
        if (Input.GetButtonDown("Fire2") && !isReload) //마우스 우측 클릭시에
        {
            FineSight();
        }

    }


    private void FineSight()
    {
        isFineSightMode = !isFineSightMode;
        currentGun.animator.SetBool("FineSightMode", isFineSightMode);
        theCrossHair.FineSightAnimation(isFineSightMode);

        if (isFineSightMode)
        {
            //while문에서 실행되는 originPos값이 바뀌는것을 방지
            StopAllCoroutines();

            //Coroutine을 통해 정조준 가동
            StartCoroutine(FineSightActiveCoroutine());
        }
        else
        {
            //while문에서 실행되는 originPos값이 바뀌는것을 방지
            StopAllCoroutines();

            //정조준 푸는 Coroutine
            StartCoroutine(FineSightDeactivateCoroutine());
        }
    }

    IEnumerator FineSightActiveCoroutine()
    {
        // 정조준 위치가 될때 까지 => 반복 자세를 갖출 때까지
        while(currentGun.transform.localPosition != currentGun.fineSightOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, 
                currentGun.fineSightOriginPos, 
                0.2f); // Lerp ( 시작위치, 도착위치, 세기)

            yield return null;
        } 
    }

    IEnumerator FineSightDeactivateCoroutine() // 정조준을 푸는 coroutine
    {
        // 정조준 위치가 원래 위치로 될때 까지 => 자세를 갖출 때까지
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition,
                originPos,
                0.2f); // Lerp ( 시작위치, 도착위치, 세기)

            yield return null;
        }
    }

    // 재장전시 캔슬
    public void CancelFineSight()
    {
        if (isFineSightMode)
            FineSight();
    }

    public Gun GetGun()
    {
        return currentGun;
    }

    public bool GetFineSightMode()
    {
        return isFineSightMode;
    }

    public void GunChange(Gun _gun)
    {
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);

        currentGun = _gun;
        WeaponManager.currentWeapon = currentGun.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentGun.animator;

        currentGun.transform.transform.localPosition = Vector3.zero; 
        currentGun.gameObject.SetActive(true);
        isActivate = true;
    }
}
