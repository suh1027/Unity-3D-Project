using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Water : MonoBehaviour
{
    //private static bool isWater = false; // => GameManager로
    
    [SerializeField] private float waterDrag;// 물속 중력
    private float originDrag;

    [SerializeField] private Color waterColor; // 물속 색깔
    [SerializeField] private float waterFogDensity; // 물의 탁한 정도

    private Color originColor;
    private float originFogDensity; // 원래대로 되돌아갈 변수들

    [SerializeField] private Color originNightColor;
    [SerializeField] private float originNightFogDensity;

    [SerializeField] private Color waterNightColor; // 밤 일때 물색
    [SerializeField] private float waterNightFogDensity; // 밤 일때 물의 fogdensity

    // SoundManager
    [SerializeField] private string sound_waterOut;
    [SerializeField] private string sound_waterIn;
    [SerializeField] private string sound_waterBreath;

    [SerializeField] private float breathTime;
    private float currentBreathTime;

    // 호흡관련 변수들
    [SerializeField] private float totalOxygen;
    private float currentOxygen;
    private float temp;

    [SerializeField] private GameObject go_BaseUI;
    [SerializeField] private Text text_totalOxygen;
    [SerializeField] private Text text_currentOxygen;
    [SerializeField] private Image image_Gauge;


    // 컴포넌트
    private StatusController thePlayerStat;

    void Start()
    {
        thePlayerStat = FindObjectOfType<StatusController>();
        
        originColor = RenderSettings.fogColor;
        originFogDensity = RenderSettings.fogDensity;

        currentOxygen = totalOxygen;
        text_totalOxygen.text = totalOxygen.ToString();

        originDrag = 0; // player의 rigidbody의 drag값
    }

    void Update()
    {
        if (GameManager.isWater)
        {
            currentBreathTime += Time.deltaTime;
            
            if (currentBreathTime >= breathTime)
            {
                SoundManager.instance.PlaySE(sound_waterBreath);
                currentBreathTime = 0;
            }
        }

        DecreaseOxygen();
    }

    private void DecreaseOxygen()
    {
        if (GameManager.isWater)
        {
            currentOxygen -= Time.deltaTime;

            text_currentOxygen.text = Mathf.RoundToInt(currentOxygen).ToString();
            image_Gauge.fillAmount = currentOxygen / totalOxygen; // fillamount 는 0~1 사이의 값

            if (currentOxygen <= 0)
            {
                //체력 감소
                //빠르게 감소하는것을 막기위해
                currentOxygen = 0;

                temp += Time.deltaTime;
                if(temp >= 1)
                {
                    thePlayerStat.DecreaseHP(1);
                    temp = 0;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            GetWater(other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            GetOutWater(other);
        }
    }

    private void GetWater(Collider _player)
    {
        go_BaseUI.SetActive(true);

        SoundManager.instance.PlaySE(sound_waterIn);

        GameManager.isWater = true;
        _player.transform.GetComponent<Rigidbody>().drag = waterDrag;

        if (!GameManager.isNight)
        {
            RenderSettings.fogColor = waterColor;
            RenderSettings.fogDensity = waterFogDensity;
        }
        else
        {
            RenderSettings.fogColor = waterNightColor;
            RenderSettings.fogDensity = waterNightFogDensity;
        }

    }

    private void GetOutWater(Collider _player)
    {

        if (GameManager.isWater)
        {
            go_BaseUI.SetActive(false);

            currentOxygen = totalOxygen;

            SoundManager.instance.PlaySE(sound_waterOut);

            GameManager.isWater = false;
            _player.transform.GetComponent<Rigidbody>().drag = originDrag;

            if (!GameManager.isNight)
            {
                RenderSettings.fogColor = originColor;
                RenderSettings.fogDensity = originFogDensity;
            }
            else
            {
                RenderSettings.fogColor = originNightColor;
                RenderSettings.fogDensity = originNightFogDensity;
            }
        }
    }
}
