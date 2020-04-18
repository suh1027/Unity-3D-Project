using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class itemEffect
{
    public string itemName;//아이템의 이름 (키값으로 사용)
    [Tooltip("HP, SP, DP, HUNGRY, THIRSTY, SATISFY 만 가능합니다.")] // 툴팁을 띄워 실수 방지
    public string[] part; // 부위. 
    public int[] num; // 수치.

}
public class ItemEffectDataBase : MonoBehaviour
{
    [SerializeField]
    private itemEffect[] itemEffects;

    private const string HP = "HP", SP = "SP", DP = "DP", HUNGRY = "HUNGRY", THIRSTY = "THIRSTY", SATISFY = "SATISFY";

    // 컴포넌트
    [SerializeField]
    private StatusController thePlayerStatus;
    [SerializeField]
    private WeaponManager theWeaponManager;



    public void UsedItem(Item _item)
    {
        if (_item.itemType == Item.ItemType.Equipment)
        {
            // 장착
            StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(_item.weaponType, _item.itemName));
        }

        else if (_item.itemType == Item.ItemType.Used) //사용 아이템일 경우
        {
            for (int i = 0; i < itemEffects.Length; i++)
            {
                if(itemEffects[i].itemName == _item.itemName)
                {
                    for (int y = 0; y < itemEffects[i].part.Length; y++)
                    {
                        switch (itemEffects[i].part[y])
                        {
                            case HP:
                                thePlayerStatus.IncreaseHP(itemEffects[i].num[y]);
                                break;
                            case SP: //스테미너는 자동으로 차게 만들어 놨음 -> StatusController 함수추가 후 다시구현
                                thePlayerStatus.IncreaseSP(itemEffects[i].num[y]);
                                break;
                            case DP:
                                thePlayerStatus.IncreaseDP(itemEffects[i].num[y]);
                                break;
                            case HUNGRY:
                                thePlayerStatus.IncreaseHungry(itemEffects[i].num[y]);
                                break;
                            case THIRSTY:
                                thePlayerStatus.IncreaseThirsty(itemEffects[i].num[y]);
                                break;
                            case SATISFY:
                                
                                break;
                            default:
                                Debug.Log("잘못된 Status 부위 HP, SP, DP, HUNGRY, THIRSTY, SATISFY만 가능합니다.");
                                break;

                        }
                        Debug.Log(_item.itemName + " 을 사용했습니다.");
                    }

                    return;
                }
            }

            Debug.Log("DB에 일치하는 ItemName이 없습니다.");

        }
    }
}
