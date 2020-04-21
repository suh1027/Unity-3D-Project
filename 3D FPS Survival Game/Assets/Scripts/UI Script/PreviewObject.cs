using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    //충돌한 오브젝트의 콜라이더
    private List<Collider> colliderList = new List<Collider>();

    [SerializeField]
    private int layerGround;
    private const int IGNORE_RAYCAST_LAYER = 2;

    [SerializeField]
    private Material green;
    [SerializeField]
    private Material red;

    private void Update()
    {
        ChangeColor();
    }

    private void ChangeColor()
    {
        if(colliderList.Count > 0)
        {
            //red
            SetColor(red);
        }
        else
        {
            //green
            SetColor(green);
        }
    }

    private void SetColor(Material mat) // wall(1)부터 (4)까지 material 변경 green <-> red
    {
        foreach (Transform tf_Child in this.transform)
        {
            var newMaterials = new Material[tf_Child.GetComponent<Renderer>().materials.Length];
            
            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = mat;
            }

            tf_Child.GetComponent<Renderer>().materials = newMaterials;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
            colliderList.Add(other);//부딛힌 콜라이더를 넣어줌
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
            colliderList.Remove(other);//부딛힌 콜라이더를 넣어줌
    }


    public bool isBuildable()
    {
        return colliderList.Count == 0;
    }
}
