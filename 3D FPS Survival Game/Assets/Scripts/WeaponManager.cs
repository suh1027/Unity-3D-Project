﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponManager : MonoBehaviour
{
    public static bool isChangeWeapon;
    // static으로 선언하여 공유자원으로 설정 , 클래스 변수 = 정적 변수

    // 현재 무기와 현재 무기의 애니메이션
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;

    // 현재 무기 타입
    [SerializeField]
    private string currentWeaponType;


    // 무기교체 딜레이 타임
    [SerializeField]
    private float changeWeaponDelayTime;
    // 무기교체가 완전히 끝난 시점

    [SerializeField]
    private float changeWeaponEndDelayTime;


    // 무기종류를 배열로 관리
    [SerializeField]
    private Gun[] guns;
    [SerializeField]
    private CloseWeapon[] hands;
    [SerializeField]
    private CloseWeapon[] axes; // 관리상 편하기 위해 구분
    [SerializeField]
    private CloseWeapon[] pickaxes;

    // Key Value 로 관리
    // 관리 차원에서 무기접근이 쉽게 가능하도록 만듬
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, CloseWeapon> handDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> axeDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> pickaxeDictionary = new Dictionary<string, CloseWeapon>();

    [SerializeField]
    private GunController theGunController;
    [SerializeField]
    private HandController theHandController;
    [SerializeField]
    private AxeController theAxeController;
    [SerializeField]
    private PickAxeController thePickAxeController;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }
        for (int i = 0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].closeWeaponName, hands[i]);
        }
        for (int i = 0; i < axes.Length; i++)
        {
            axeDictionary.Add(axes[i].closeWeaponName, axes[i]);
        }
        for (int i = 0; i < pickaxes.Length; i++)
        {
            pickaxeDictionary.Add(pickaxes[i].closeWeaponName, pickaxes[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                // 숫자 1을 눌렀을때 무기교체 실행
                StartCoroutine(ChangeWeaponCoroutine("HAND","맨손"));
            }

            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                // 숫자 2를 눌렀을때 구현 -> Coroutine으로 구현
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));
            }

            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                // 숫자 3를 눌렀을때 구현 -> Coroutine으로 구현
                StartCoroutine(ChangeWeaponCoroutine("AXE", "Axe"));
            }

            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                // 숫자 3를 눌렀을때 구현 -> Coroutine으로 구현
                StartCoroutine(ChangeWeaponCoroutine("PICKAXE", "PickAxe"));
            }

        }

    }

    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeWeaponDelayTime);

        CancelPreWeaponAction();
        WeaponChange(_type,_name);

        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        currentWeaponType = _type;

        isChangeWeapon = false;
    }

    private void CancelPreWeaponAction()
    {
        switch (currentWeaponType)
        {
            case "GUN":
                theGunController.CancelFineSight(); // 정조준상태 해제
                //재장전시 스왑요청시 캔슬하는 경우 필요
                theGunController.CancelReload();
                GunController.isActivate = false;
                break;
            case "HAND":
                HandController.isActivate = false;
                break;
            case "AXE":
                AxeController.isActivate = false;
                break;
            case "PICKAXE":
                PickAxeController.isActivate = false;
                break;
        }
    }

    private void WeaponChange(string _type, string _name)
    {
        if(_type == "GUN")
            theGunController.GunChange(gunDictionary[_name]);
        else if(_type == "HAND")
            theHandController.CloseWeaponChange(handDictionary[_name]);
        else if (_type == "AXE")
            theAxeController.CloseWeaponChange(axeDictionary[_name]);
        else if (_type == "PICKAXE")
            thePickAxeController.CloseWeaponChange(pickaxeDictionary[_name]);

        // pick axe 추가--
    }

    // 물에서 무기를 집어넣는 함수
    public IEnumerator WeaponInCoroutine()
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeWeaponDelayTime);

        currentWeapon.gameObject.SetActive(false);
    }

    //물에서 나올때

    public void WeaponOut()
    {
        isChangeWeapon = false;

        currentWeapon.gameObject.SetActive(true);
    }
}
