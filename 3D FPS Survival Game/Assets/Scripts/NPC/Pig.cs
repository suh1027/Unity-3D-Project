using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    [SerializeField] private string animalName;//동물이름
    [SerializeField] private int hp; //체력
    [SerializeField] private float walkSpeed; // 걷는 속도
    [SerializeField] private float runSpeed; // 맞았을때 뛰는 속도
    private float applySpeed;

    //상태변수
    private bool isWalking;//걷는지 판별
    private bool isAction; //행동중 판별
    private bool isRunning; // 뛰는중 판별
    private bool isDead;

    [SerializeField] private float walkTime;
    [SerializeField] private float waitTime;
    [SerializeField] private float runTime;

    private float currentTime;

    // 컴포넌트
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private BoxCollider boxCol;

    private Vector3 direction; // 랜덤방향을 위한 변수

    private AudioSource theAudio;
    [SerializeField] private AudioClip[] sound_pig_normal;
    [SerializeField] private AudioClip sound_ping_hurt;
    [SerializeField] private AudioClip sound_pig_dead;

    private void Start()
    {
        theAudio = GetComponent<AudioSource>();

        currentTime = waitTime;
        isAction = true;

    }

    private void Update()
    {
        if (!isDead) { 
            Move();
            Rotation();
            ElapseTime(); // currenttime 시간이 흐르도록 설정하는 함수 
        }  
    }
    private void Move()
    {
        if (isWalking || isRunning)
        {
            rigid.MovePosition(transform.position + transform.forward * applySpeed * Time.deltaTime); //1초에 walkspeed만큼 나아감
        }
    }

    private void Rotation()
    {
        if (isWalking || isRunning) // transform.eulerAngles => 인스펙터창의 rotation 값
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f,direction.y,0f), 0.01f);
            rigid.MoveRotation(Quaternion.Euler(_rotation)); 
            // Vector3 -> Quaternion.Euler()로 Quaternion으로 바꾸는 함수
        }
    }

    private void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if(currentTime < 0)
            {
                // 다음 랜덤 행동 개시
                ReSet();
            }
        }
    }
    private void ReSet()
    {
        isWalking = false;
        isAction = true;
        isRunning = false;
        
        anim.SetBool("Running", isRunning);
        anim.SetBool("Walking", isWalking);

        applySpeed = walkSpeed;

        direction.Set(0f, Random.Range(0f, 360f), 0f);
        
        RandomAction();
    }

    private void RandomAction()
    {
        //랜덤 난수 설정
        // int형 Random.Range는 최소값 포함, 최대값 미포함 
        // float일때는 최소 최대값 둘다 inclusive(포함)      

        RandomSound();

        int _random = Random.Range(0, 4); // 대기(0), 풀뜯기(1), 두리번(2), 걷기(3)


        if(_random == 0)
        {
            //Debug.Log("대기");
            Wait();
        }
        else if (_random == 1)
        {
            //Debug.Log("풀뜯기");
            Eat();
        }
        else if (_random == 2)
        {
            //Debug.Log("두리번");
            Peek();
        }
        else if (_random == 3)
        {
            //Debug.Log("걷기");
            TryWalk();
        }
    }



    private void Wait()
    {
        currentTime = waitTime;
        Debug.Log("대기");
    }
    private void Eat()
    {
        currentTime = waitTime;
        Debug.Log("풀뜯기");
        anim.SetTrigger("Eat");
    }
    private void Peek()
    {
        currentTime = waitTime;
        anim.SetTrigger("Peek");
        Debug.Log("두리번");
    }
    private void TryWalk()
    {
        isWalking = true;
        currentTime = walkTime;

        applySpeed = walkSpeed;

        anim.SetBool("Walking",isWalking);
        Debug.Log("걷기");
    }

    private void Run(Vector3 _targetPos)
    {
        // 맞았을때, 위협이 되는 대상 반대로 뛰도록 만듬

        direction = Quaternion.LookRotation(transform.position - _targetPos).eulerAngles; // 반대방향을 바라보게 만듬

        
        currentTime = runTime;
        isWalking = false;
        isRunning = true;

        applySpeed = runSpeed;
        Debug.Log("뛰기");
        anim.SetBool("Running", isRunning);
    }

    public void Damage(int _dmg, Vector3 _targetPos)
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

            PlaySE(sound_ping_hurt);
            anim.SetTrigger("Hurt");
            Run(_targetPos);
        }
       
    }

    private void RandomSound()
    {
        int _random = Random.Range(0, 3); // 일상 사운드 3가지( 랜덤 )
        PlaySE(sound_pig_normal[_random]);

    }

    private void PlaySE(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }

    private void Dead()
    {
        PlaySE(sound_pig_dead);

        isWalking = false;
        isRunning = false;
        isDead = true;

        anim.SetTrigger("Dead");
    }
}
