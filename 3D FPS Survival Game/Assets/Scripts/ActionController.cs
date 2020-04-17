using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range; // 습득 가능한 최대 거리.

    private bool pickUpActivated = false; // 습득 가능할 시 True;
    
    // Ray
    private RaycastHit hitInfo; // 충돌체 정보 저장
    
    [SerializeField]
    private LayerMask layerMask; // Layermask???? -> 아이템 레이어에만 반응하도록 레이어 마스크를 설정 ??

    [SerializeField]
    private Text actionText;

    [SerializeField]
    private Inventory theInventory;

    void Update()
    {
        TryAction();
        CheckItem();
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckItem();
            CanPickUp();
        }
    }

    private void CheckItem()
    {
        //transform.forward == transform.TransformDirection(Vector3.forward)
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
        }
        else
            InfoDisappear();
    }

    private void ItemInfoAppear()
    {
        pickUpActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName 
            + " 획득" + "<color = yellow>" + "(E)" + "</color>";
    }

    private void InfoDisappear()
    {
        pickUpActivated = false;
        actionText.gameObject.SetActive(false);
    }

    private void CanPickUp()
    {
        if (pickUpActivated)
        {
            if(hitInfo.transform != null)
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName
            + " 획득 했습니다.");


                theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);


                Destroy(hitInfo.transform.gameObject);
                InfoDisappear();

            }
        }
    }
}
