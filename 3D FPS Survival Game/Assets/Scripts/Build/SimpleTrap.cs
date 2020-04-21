using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTrap : MonoBehaviour
{
    private Rigidbody[] rigid;
    [SerializeField] private GameObject go_Meat;
    [SerializeField] private int damage;

    private bool isActivated = false;

    private AudioSource audio;
    [SerializeField] private AudioClip sound_Activate;


    private StatusController theStatusController;

    private void Start()
    {
        rigid = GetComponentsInChildren<Rigidbody>();
        audio = GetComponent<AudioSource>();
        theStatusController = FindObjectOfType<StatusController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActivated)
        {
            if(other.transform.tag != "Untagged")
            {
                isActivated = true;
                audio.clip = sound_Activate;
                audio.Play();

                Destroy(go_Meat);//고기 제거

                for (int i = 0; i < rigid.Length; i++)
                {
                    rigid[i].useGravity = true;
                    rigid[i].isKinematic = false;
                }

                if(other.transform.name == "Player")
                {
                    //other.transform.GetComponent<StatusController>().DecreaseHP(damage);
                    theStatusController.DecreaseHP(damage);
                }
            }
        }
    }
}
