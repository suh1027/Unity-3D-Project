using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound // : Monobehaviour 붙어야 컴포넌트를 가져다 쓸 수 있음
{
    public string name; // 곡 이름
    public AudioClip clip; // 곡
}

public class SoundManager : MonoBehaviour
{

    static public SoundManager instance;
    #region singleton
    void Awake() // 객체 생성시 최초 실행
    {
        // Singleton으로 만들어 주어야함!
        // singleton(싱글톤) -> Awake() 개념 이해 다시 복습
        // 한개의 인스턴스로 유지 하는..

        if (instance == null) { 
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this.gameObject);
    }
    #endregion singleton

    public AudioSource[] audioSourceEffects;
    public AudioSource audioSourceBGM;

    public Sound[] effectSounds;
    public Sound[] bgmSounds;

    public string[] playSoundName;


    void Start() // 시작하자마자 playSoundName의 배열 갯수를 audioSourceEffects의 갯수를 일치시킴
    {
        playSoundName = new string[audioSourceEffects.Length];

    }

    //void OnEnable() // 매번 활성화가 되면 실행, Coroutine 실행 불가능
    //void Start() // 매번 활성화 된 실행, Coroutine 실행 가능

    public void PlaySE(string _name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if(_name == effectSounds[i].name)
            {
                for (int j = 0; j < audioSourceEffects.Length; j++)
                {
                    if (!audioSourceEffects[j].isPlaying)
                    {
                        playSoundName[j] = effectSounds[i].name;
                        audioSourceEffects[j].clip = effectSounds[i].clip;
                        audioSourceEffects[j].Play();
                        return;
                    }
                }
                Debug.Log("모든 가용 AudioSource가 사용 중입니다.");
                return;
            }
        }
        Debug.Log(_name + " 사운드가 SoundManager에 등록되어 있지 않습니다.");
    }

    public void StopAllSE()
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].Stop();
        }
    }

    public void StopSE(string _name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            if(playSoundName[i] == _name) 
            {
                audioSourceEffects[i].Stop();
                return;
            } 
        }
        Debug.Log("재생 중인 " + _name + " 사운드가 없습니다.");
    }
}
