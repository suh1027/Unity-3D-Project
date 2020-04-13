﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //스피드 조정 변수
    [SerializeField]//inspector창에서 수정가능
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    private float appplySpeed;

    // 점프
    [SerializeField]
    private float jumpForce;

    // 앉기
    [SerializeField]
    private float crouchSpeed;

    // 상태변수
    private bool isRun = false;
    private bool isGround = true;
    private bool isCrouch = false;

    // 앉았을때 얼마나 앉을지 결정하는 변수
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    // 카메라 민감도
    [SerializeField]
    private float lookSensitivity;

    // 카메라 회전 한계
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0;

    // 필요 컴포넌트
    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigid;
    private CapsuleCollider capsuleCollider; //지면과 맞닿아 있을때만 점프가 가능하도록 사용

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        appplySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y; //localPosition -> world기준이 아닌 player기준으로 position값을 받는것
        applyCrouchPosY = originPosY;
    }

    void Update()
    {
        IsGround();
        TryJump();

        TryRun();

        TryCrounch();
        // theCamera = FindObjectOfType<Camera>(); 전체검색

        Move();
        CameraRotation();
        CharacterRotation();
    }

    // #1. 앉기 관련 함수



    private void TryCrounch() 
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    private void Crouch()
    {
        isCrouch = !isCrouch; //true <-> false

        if (isCrouch)
        {
            appplySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            appplySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }
        /*theCamera.transform.localPosition = new Vector3(theCamera.transform.localPosition.x, 
            applyCrouchPosY, 
            theCamera.transform.localPosition.z);*/

        StartCoroutine(CrounchCoroutine());
    }

    //부드럽게 카메라를 이동하기 위해
    IEnumerator CrounchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y;
        
        int count = 0;

        while(_posY != applyCrouchPosY)
        {
            count++;
            //보간사용해서 자연스럽게 카메라 이동
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.3f);
            theCamera.transform.localPosition = new Vector3(0,_posY,0);
            
            if(count > 15) //보간의 단점 -> 0 과 1에 거의 근사한값으로 계속적으로 반복하는 단점이 있음
                break; //보간의 단점을 보완 0 과 1로 딱떨어지게 만들어 주는것
            
            yield return null;
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0f);

        //흐름을 끊을수가 있음, x초동안 대기, 병렬처리를 위해 만들어진 개념
    }

    
    
    // #2. 점프관련 함수



    private void IsGround()
    {
        // Vector3.down 을 쓰는 이유 -> transform.up을 쓰면 Player가 회전했을때 방향이 잘못됨
        // capsuleCollider.bounds.extents.y bounds.extents.y => 외부영역의.반사이즈.y값 지면과 딱 닿을 만큼 쏘는것
        // 계단이나 오르막길에서 ray가 바닥에 닿지않는 문제가 발생 약간의 여유를 더 주어야 함 (0.1f)
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }

    private void TryJump()
    {
        if (isGround && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void Jump()
    {
        if (isCrouch) // 앉은상태에서 점프시도시 일어서게 만듬
            Crouch();
        myRigid.velocity = transform.up * jumpForce;
    }




    // #3. 달리기 관련 함수



    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }

    private void Running()
    {

        if (isCrouch)
            Crouch(); //앉았을때 달리기 시도시에 앉은상태 해제

        isRun = true;
        appplySpeed = runSpeed;
    }

    private void RunningCancel()
    {

        isRun = false;
        appplySpeed = walkSpeed;
    }




    // #4. 이동 관련 함수



    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX; // (right => (1,0,0))
        Vector3 _moveVertical = transform.forward * _moveDirZ; //(foward => (0,0,1))

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * appplySpeed; // 단위벡터로

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);

    }




    // #5. 카메라 관련 함수



    //카메라 상하회전
    private void CameraRotation() 
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;

        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, 
            -cameraRotationLimit, //최소값
            cameraRotationLimit); //최대값 고정 => clamp 함수

        // 오일러 앵글 -> transform 의 rotation이라고 생각하면 편함
        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    //좌우 캐릭터 회전
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY)); //Quaternion.Euler -> rotation으로!

        //Debug.Log(myRigid.rotation);
        //Debug.Log(myRigid.rotation.eulerAngles); //Quaternion 값
    }
}
