using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [SerializeField] private float secondPerRealTimeSecond; // 게임세계의 100초 = 현실세계의 1초

    private bool isNight = false;

    [SerializeField] private float fogDensityCalc; // 증감량 비율


    [SerializeField] private float nightFogDensity; // 밤상태의 fog 밀도 
    //-> window -> Rendering -> Light setting -> other setting 
    private float dayFogDensity;
    private float currentFogDensity; // 현재 fog 밀도 (계산용)

    private void Start()
    {
        dayFogDensity = RenderSettings.fogDensity;
    }


    private void Update()
    {
        //Sun의 Rotation x축을 조정해서 낮과 밤을 설정
        transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecond * Time.deltaTime);

        if(transform.eulerAngles.x >= 170)
        {
            isNight = true;
        }
        else if(transform.eulerAngles.x <= 10) // 해가 뜨고있을때 rotation x 값 보면서 조정
        {
            isNight = false;
        }


        if (isNight)
        {
            if(currentFogDensity <= nightFogDensity) { 
                currentFogDensity += fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
        else
        {
            if (currentFogDensity >= dayFogDensity)
            {
                currentFogDensity -= fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
    }
}
