using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject 
    // Scriptableobject -> 게임 오브젝트에 붙여서 사용할 필요가 없는 오브젝트를 만들기 위해 씀
    // monobehavi.. 게임 오브젝트에 붙여 사용
{
    public string itemName;
    public ItemType itemType; // 아이템의 유형 itemtype.Equipment ....
    public Sprite itemImage; // 아이템 이미지 (인벤토리에서)
    public GameObject itemPrefab; // 아이템의 프리팹

    public string weaponType; // 무기 유형.

    public enum ItemType // 아이템 타입 열거형 변수
    {
        Equipment,
        Used,
        Ingredient,
        ETC
    }

}
