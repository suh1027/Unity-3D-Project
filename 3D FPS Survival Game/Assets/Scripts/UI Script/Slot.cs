using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{

    public Item item; //획득한 아이템
    public int itemCount; //획득한 아이템의 갯수
    public Image itemImage; // 아이템의 이미지

    // 컴포넌트
    [SerializeField]
    private Text text_Count;
    [SerializeField]
    private GameObject go_Count_Image; // 파란색 원

    // alpha값 변경 메소드 구현 slot 이미지 투명도 조절
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    //아이템 획득
    public void AddItem(Item _item, int _count = 1) // 기본값을 1을 주어 생략가능하도록 생성
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = _item.itemImage;

        if(item.itemType != Item.ItemType.Equipment) 
            // 장비가 아닐경우에만 갯수표시 파란색 원 이미지 Active 되도록 설정
        {
            go_Count_Image.SetActive(true);
            text_Count.text = itemCount.ToString();
        }
        else
        {
            text_Count.text = "0";
            go_Count_Image.SetActive(false);
        }
        SetColor(1);
    }

    //아이템 갯수 조정
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();

        if (itemCount <= 0)
            ClearSlot();
    }

    //슬롯 초기화
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        go_Count_Image.SetActive(false);
        text_Count.text = "0";
    }
}
