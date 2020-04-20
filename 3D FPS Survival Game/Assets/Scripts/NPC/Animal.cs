using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField] protected string animalName;//동물이름
    [SerializeField] protected int hp; //체력
    [SerializeField] protected float walkSpeed; // 걷는 속도
    [SerializeField] protected float runSpeed; // 맞았을때 뛰는 속도
    [SerializeField] protected float turningSpeed; // 회전속도
    protected float applySpeed;

    //상태변수
    protected bool isWalking;//걷는지 판별
    protected bool isAction; //행동중 판별
    protected bool isRunning; // 뛰는중 판별
    protected bool isDead;

    [SerializeField] protected float walkTime;
    [SerializeField] protected float waitTime;
    [SerializeField] protected float runTime;

    protected float currentTime;

    // 컴포넌트
    [SerializeField] protected Animator anim;
    [SerializeField] protected Rigidbody rigid;
    [SerializeField] protected BoxCollider boxCol;

    protected Vector3 direction; // 랜덤방향을 위한 변수

    protected AudioSource theAudio;
    [SerializeField] protected AudioClip[] sound_animal_normal;
    [SerializeField] protected AudioClip sound_animal_hurt;
    [SerializeField] protected AudioClip sound_animal_dead;

    private void Start()
    {
        theAudio = GetComponent<AudioSource>();

        currentTime = waitTime;
        isAction = true;

    }

    private void Update()
    {
        if (!isDead)
        {
            Move();
            Rotation();
            ElapseTime(); // currenttime 시간이 흐르도록 설정하는 함수 
        }
    }
    protected void Move()
    {
        if (isWalking || isRunning)
        {
            rigid.MovePosition(transform.position + transform.forward * applySpeed * Time.deltaTime); //1초에 walkspeed만큼 나아감
        }
    }

    protected void Rotation()
    {
        if (isWalking || isRunning) // transform.eulerAngles => 인스펙터창의 rotation 값
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), turningSpeed);
            rigid.MoveRotation(Quaternion.Euler(_rotation));
            // Vector3 -> Quaternion.Euler()로 Quaternion으로 바꾸는 함수
        }
    }

    protected void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime < 0)
            {
                // 다음 랜덤 행동 개시
                ReSet();
            }
        }
    }
    protected virtual void ReSet()
    {
        isWalking = false;
        isAction = true;
        isRunning = false;

        anim.SetBool("Running", isRunning);
        anim.SetBool("Walking", isWalking);

        applySpeed = walkSpeed;

        direction.Set(0f, Random.Range(0f, 360f), 0f);

        //RandomAction();
    }




    protected void TryWalk()
    {
        isWalking = true;
        currentTime = walkTime;

        applySpeed = walkSpeed;

        anim.SetBool("Walking", isWalking);
        Debug.Log("걷기");
    }


    public virtual void Damage(int _dmg, Vector3 _targetPos)
    {
        if (!isDead)
        {
            hp -= _dmg;

            if (hp <= 0)
            {
                //Debug.Log("체력 0 이하"); //Dead 부분
                Dead();
                return;
            }

            PlaySE(sound_animal_hurt);
            anim.SetTrigger("Hurt");
            // Run(_targetPos);
        }

    }

    protected void RandomSound()
    {
        int _random = Random.Range(0, 3); // 일상 사운드 3가지( 랜덤 )
        PlaySE(sound_animal_normal[_random]);

    }

    protected void PlaySE(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }

    protected void Dead()
    {
        PlaySE(sound_animal_dead);

        isWalking = false;
        isRunning = false;
        isDead = true;

        anim.SetTrigger("Dead");
    }

}
