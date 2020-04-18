using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour // 자연스러운 애니메이션 효과
{
    // 기존 위치
    private Vector3 originPos;

    // 현재 위치
    private Vector3 currentPos;

    // Sway의 최대 이동되는 위치 ,  정조준 sway 한계
    [SerializeField]
    private Vector3 limitPos;

    [SerializeField]
    private Vector3 fineSightLimitPos;

    // 부드러운 움직임 정도
    [SerializeField]
    private Vector3 smoothSway;

    // 컴포넌트
    [SerializeField]
    private GunController theGunController;


    // Start is called before the first frame update
    void Start()
    {
        originPos = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Inventory.inventoryActivated) { 
            TrySway();
        }
    }

    private void TrySway()
    {
        if(Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0) //움직였을때
        {
            Swaying();
        }
        else // 움직이지 않았을때
        {
            BackToOriginPos();
        }
    }

    private void Swaying()
    {
        float _moveX = Input.GetAxisRaw("Mouse X");
        float _moveY = Input.GetAxisRaw("Mouse Y");

        if (!theGunController.GetFineSightMode()) // 정조준 상태가 아닐때
        {
            // -로 하는거 주의 Clamp(가두기)  Lerp(천천히움직이게)  로 하는거 -_moveX 생각해보기 !
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.x), -limitPos.x, limitPos.x),
                Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.y), -limitPos.y, limitPos.y),
                originPos.z);
        }
        else // 정조준 상태일 때
        {
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.x), -fineSightLimitPos.x, fineSightLimitPos.x),
                Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.y), -fineSightLimitPos.y, fineSightLimitPos.y),
                originPos.z);
        }

        transform.localPosition = currentPos;
    }

    private void BackToOriginPos()
    {
        currentPos = Vector3.Lerp(currentPos, originPos, smoothSway.x);
        transform.localPosition = currentPos;
    }
}
