using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // 클릭 처리를 위해

//클래스는 다중상속이 불가능 하다 but 인터페이스는 다중상속이 가능하다
public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, 
    IDragHandler, IEndDragHandler, IDropHandler // 클릭을 담당하는 인터페이스 , 드래그 담당
{

    //private Vector3 originPos;

    public Item item; //획득한 아이템
    public int itemCount; //획득한 아이템의 갯수
    public Image itemImage; // 아이템의 이미지

    // 컴포넌트
    [SerializeField]
    private Text text_Count;
    [SerializeField]
    private GameObject go_Count_Image; // 파란색 원

    private WeaponManager theWeaponManager;

    private void Start()
    {
        //originPos = transform.position;
        theWeaponManager = FindObjectOfType<WeaponManager>();
    }

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

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right) // 우클릭 시에
        {
            if(item != null)
            {
                if(item.itemType == Item.ItemType.Equipment)
                {
                    // 장착
                    StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(item.weaponType, item.itemName));
                }
                else
                {
                    // 소모
                    Debug.Log(item.itemName + " 을 사용했습니다.");
                    SetSlotCount(-1);
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(item != null)
        {
            DragSlot.instansce.dragSlot = this;
            DragSlot.instansce.DragSetImage(itemImage);    
            DragSlot.instansce.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            //transform.position = eventData.position;

            DragSlot.instansce.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("EndDrag");
        DragSlot.instansce.SetColor(0);
        DragSlot.instansce.dragSlot = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");

        if(DragSlot.instansce.dragSlot != null)
            ChangeSlot();
    }

    //EndDrag - OnDrop 의 차이 Debug를 띄워 확인! 
    // 다른 슬롯 위에서 드래그를 놓았을때는 Drop
    // 그냥 아무렇게나 Drag후 놓았을때는 EndDrag

    private void ChangeSlot()
    {
        // a->b 일때 swap 하는 방식으로 구현

        Item _tempItem = item;
        int _tempItemCount = itemCount;

        AddItem(DragSlot.instansce.dragSlot.item,DragSlot.instansce.dragSlot.itemCount);

        if(_tempItem != null)
        {
            DragSlot.instansce.dragSlot.AddItem(_tempItem, _tempItemCount);
        }
        else
        {
            DragSlot.instansce.dragSlot.ClearSlot();
        }
    }
}
