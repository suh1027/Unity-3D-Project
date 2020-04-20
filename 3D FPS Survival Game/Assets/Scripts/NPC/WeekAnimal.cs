using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeekAnimal : Animal
{
    public void Run(Vector3 _targetPos)
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

    public override void Damage(int _dmg, Vector3 _targetPos)
    {
        base.Damage(_dmg, _targetPos);
        if (!isDead)
            Run(_targetPos);
    }

}
