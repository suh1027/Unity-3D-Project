using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private GunController theGunController;

    // 크로스 헤어 상태에 따른 총의 정확도
    private float gunAccuracy;

    // 크로스헤어 비활성화를 위한 부모 객체
    [SerializeField]
    private GameObject go_CrossHairHUD;

    public void WalkingAnimation(bool _flag)
    {
        animator.SetBool("isWalking", _flag);
    }
    public void RunningAnimation(bool _flag)
    {
        animator.SetBool("isRun", _flag);
    }
    public void CrouchingAnimation(bool _flag)
    {
        animator.SetBool("Crouching", _flag);
    }

    public void FineSightAnimation(bool _flag)
    {
        animator.SetBool("FineSight", _flag);
    }

    public void FireAnimation()
    {
        if (animator.GetBool("isWalking"))
        {
            animator.SetTrigger("Walk_Fire");
        }
        else if (animator.GetBool("Crouching"))
        {
            animator.SetTrigger("Crouch_Fire");
        }
        else 
        { 
            animator.SetTrigger("Idle_Fire");
        }
    }

    // 크로스헤어에 따른 정확도 수치화
    public float GetAccuracy()
    {
        if (animator.GetBool("isWalking"))
        {
            gunAccuracy = 0.06f;
        }
        else if (animator.GetBool("Crouching"))
        {
            gunAccuracy = 0.015f;
        }
        else if (theGunController.GetFineSightMode())
        {
            gunAccuracy = 0.001f;
        }
        else
        {
            gunAccuracy = 0.035f;
        }

        return gunAccuracy;
    } 
    
}
