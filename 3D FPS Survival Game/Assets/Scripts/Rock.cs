using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{

    [SerializeField]
    private int hp; // 바위의 체력

    [SerializeField]
    private float destroyTime; // 파편이 제거되는 시간

    [SerializeField]
    private SphereCollider col; // spherecollider 컴포넌트
    //부서지면 Rock의 구체콜라이더를 제거해 플레이어가 지나갈 수 있도록 설정

    [SerializeField]
    private GameObject go_rock; // 일반 바위
    [SerializeField]
    private GameObject go_debris; // 깨진 바위
    [SerializeField]
    private GameObject go_effect_prefabs; // 채굴 이펙트 프리팹

    [SerializeField]
    private string strike_sound;
    [SerializeField]
    private string destroy_sound;

/*    // 소리 구현
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip effectClip; // 이펙트 사운드
    [SerializeField]
    private AudioClip effectClip2; // 파괴 사운드*/

    public void Mining()
    {
        // 소리 구현
        /*        audioSource.clip = effectClip;
                audioSource.Play();*/
        SoundManager.instance.PlaySE(strike_sound);
        
        var clone = Instantiate(go_effect_prefabs, col.bounds.center,Quaternion.identity); //quaternion identity -> 기본회전값

        Destroy(clone, destroyTime);

        hp--;
        if (hp <= 0)
            Destruction();
    }

    private void Destruction()
    {
        // 파괴시 사운드 구현
        /*        audioSource.clip = effectClip2;
                audioSource.Play();*/

        SoundManager.instance.PlaySE(destroy_sound);

        col.enabled = false; // 비활성화
        Destroy(go_rock);

        go_debris.SetActive(true);
        Destroy(go_debris, destroyTime);
        //destroy time 이후에 삭제
    }
}
