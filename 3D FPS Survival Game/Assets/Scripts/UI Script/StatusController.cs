using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{

    // #1. 체력
    [SerializeField]
    private int hp;
    private int currentHP;

    // #2. 스테미너
    [SerializeField]
    private int sp;
    private int currentSP;

    [SerializeField]
    private int spIncreaseSpeed;

    [SerializeField]
    private int spRechargeTime;    // 스테미너 재회복 딜레이
    private int currentSpRechargeTime; // 실제로 깎여 나가는 시간

    private bool spUsed; // 스테미너 감소 여부

    // #3. 방어력
    [SerializeField]
    private int dp;
    private int currentDP;

    // #4. 배고픔
    [SerializeField]
    private int hungry;
    private int currentHungry;

    [SerializeField]
    private int hungryDecreaseTime;
    private int currentHungryDecreaseTime;

    // #5. 목마름
    [SerializeField]
    private int thirsty;
    private int currentThirsty;

    [SerializeField]
    private int thirstyDecreaseTime;
    private int currentThirstyDecreaseTime;

    // #6.  만족도
    [SerializeField]
    private int satisfy;
    private int currentSatisfy;

    [SerializeField]
    private Image[] images_Gauge;

    private const int HP = 0, DP = 1, SP = 2, HUNGRY = 3, THIRSTY = 4, SATISFY = 5;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = hp;
        currentDP = dp;
        currentSP = sp;
        currentThirsty = thirsty;
        currentHungry = hungry;
        currentSatisfy = satisfy;

    }

    // Update is called once per frame
    void Update()
    {
        Hungry();
        Thirsty();

        //이미지 게이지 업데이트를 위한 함수
        GaugeUpdate();

        SPRechargeTime();
        SPRecover();
    }

    private void Hungry()
    {
        if(currentHungry > 0)
        {
            if (currentHungryDecreaseTime <= hungryDecreaseTime)
                currentHungryDecreaseTime++;
            else { 
                currentHungry--;
                currentHungryDecreaseTime = 0;
            }
        }
        else
            Debug.Log("배고픔 수치가 0이 되었습니다.");
    }

    private void Thirsty()
    {
        if (currentThirsty > 0)
        {
            if (currentThirstyDecreaseTime <= thirstyDecreaseTime)
                currentThirstyDecreaseTime++;
            else
            {
                currentThirsty--;
                currentThirstyDecreaseTime = 0;
            }
        }
        else
            Debug.Log("목마름 수치가 0이 되었습니다.");
    }

    private void GaugeUpdate()
    {
        images_Gauge[HP].fillAmount = (float) currentHP / hp;  //최대체력에서 현 체력을 나누어 백분율로 표현(fillAmount)
        images_Gauge[SP].fillAmount = (float)currentSP / sp;
        images_Gauge[DP].fillAmount = (float)currentDP / dp;
        images_Gauge[HUNGRY].fillAmount = (float)currentHungry / hungry;
        images_Gauge[THIRSTY].fillAmount = (float)currentThirsty / thirsty;
        images_Gauge[SATISFY].fillAmount = (float)currentSatisfy / satisfy;
    }

    public void DecreaseStamina(int _count)
    {
        spUsed = true;
        currentSpRechargeTime = 0;

        if (currentSP - _count > 0)
            currentSP -= _count;
        else
            currentSP = 0;
    }

    private void SPRechargeTime()
    {
        if(spUsed)
        {
            if (currentSpRechargeTime < spRechargeTime)
                currentSpRechargeTime++;
            else
                spUsed = false;
        }
    }

    private void SPRecover()
    {
        if(!spUsed && currentSP < sp)
        {
            currentSP += spIncreaseSpeed;
        }

    }

    public int GetCurrentSP()
    {
        return currentSP;
    }


    public void IncreaseHP(int _count)
    {
        if (currentHP + _count < hp)
            currentHP += _count;
        else
            currentHP = hp;
    }
    public void IncreaseSP(int _count)
    {
        if (currentSP + _count < sp)
            currentSP += _count;
        else
            currentSP = sp;
    }

    public void DecreaseHP(int _count)
    {
        if (currentDP > 0) //방어력이 0 이하일때만 체력이 달도록 설정
        {
            DecreaseDP(_count);
            return;
        }
        
        currentHP -= _count;
        
        if(currentHP <= 0)
        {
            Debug.Log("캐릭터의 hp가 0이 되었습니다.");
        }
    }

    public void IncreaseDP(int _count)
    {
        if (currentDP + _count < dp)
            currentDP += _count;
        else
            currentDP = dp;
    }

    public void DecreaseDP(int _count)
    {
        currentDP -= _count;
        if (currentDP <= 0)
        {
            Debug.Log("캐릭터의 방어력이 0이 되었습니다.");
        }
    }

    public void IncreaseHungry(int _count)
    {
        if (currentHungry + _count < hungry)
            currentHungry += _count;
        else
            currentHungry = hungry;
    }

    public void DecreaseHungry(int _count)
    {
        if (currentHungry - -_count < 0)
            currentHungry = 0;
            //Debug.Log("캐릭터의 배고픔이 0이 되었습니다.");
        else
            currentHungry -= _count;
        
    }
    public void IncreaseThirsty(int _count)
    {
        if (currentThirsty + _count < thirsty)
            currentThirsty += _count;
        else
            currentThirsty = thirsty;
    }

    public void DecreaseThirsty(int _count)
    {
        if (currentThirsty - -_count < 0)
            currentThirsty = 0;
        //Debug.Log("캐릭터의 목마름이 0이 되었습니다.");
        else
            currentHungry -= _count;

    }
}
