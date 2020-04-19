using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotToolTip : MonoBehaviour
{
    [SerializeField]
    private GameObject go_Base;

    // 컴포넌트
    [SerializeField]
    private Text text_ItemName;
    [SerializeField]
    private Text text_ItemDesc;
    [SerializeField]
    private Text text_HowToUsed;

    public void ShowToolTip(Item _item, Vector3 _position)
    {
        go_Base.SetActive(true);
        _position += new Vector3(go_Base.GetComponent<RectTransform>().rect.width * 0.5f,
            -go_Base.GetComponent<RectTransform>().rect.height,
            0f);// 너비의 반 높의의 반만큼 더해줌 + 아래로 좀더 이동(수정)
        go_Base.transform.position = _position;

        text_ItemName.text = _item.itemName;
        text_ItemDesc.text = _item.itemDesc;

        if (_item.itemType == Item.ItemType.Equipment) // 장비 아이템
        {
            text_HowToUsed.text = "우클릭 - 장착";
        }
        else if (_item.itemType == Item.ItemType.Used) // 사용아이템
        {
            text_HowToUsed.text = "우클릭 - 먹기";
        }
        else // 재료, 기타아이템
        {
            text_HowToUsed.text = "";
        } 
    }

    public void HideToolTip()
    {
        go_Base.SetActive(false);
    }
}
