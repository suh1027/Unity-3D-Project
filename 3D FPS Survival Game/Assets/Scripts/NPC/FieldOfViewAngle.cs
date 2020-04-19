using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewAngle : MonoBehaviour
{
    [SerializeField]
    private float viewAngle; // 시야각 (120도)
    [SerializeField]
    private float viewDistance; // 시야 거리 (10미터)
    [SerializeField]
    private LayerMask targetMask; // 타겟 마스크(플레이어);

    //컴포넌트

    private Pig thePig;

    private void Start()
    {
        thePig = GetComponent<Pig>();
    }

    private void Update()
    {
        View();
    }

    private Vector3 BoundaryAngle(float _angle)
    {
        _angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(_angle * Mathf.Deg2Rad),0f,Mathf.Cos(_angle * Mathf.Deg2Rad));
        // Deg2Rad -> degree -> rad
    }

    private void View()
    {
        //좌측 60도 우측 60도 꺽어서 120도

        Vector3 _leftBoundary = BoundaryAngle(-viewAngle * 0.5f);
        Vector3 _rightBoundary = BoundaryAngle(viewAngle * 0.5f);

        // Debug용으로 Ray 출력해봄
        Debug.DrawRay(transform.position + transform.up, _leftBoundary, Color.red);
        Debug.DrawRay(transform.position + transform.up, _rightBoundary, Color.red);// transform up -> 살짝 올려서 보이게

        // OverlapSphere => 주변에 있는 컬라이더들을 뽑아내서 저장시키는데 사용하는 함수
        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        for (int i = 0; i < _target.Length; i++)
        {
            Transform _targetTf = _target[i].transform;
            if (_targetTf.name == "Player") 
            {

                Vector3 _direction = (_targetTf.position - transform.position).normalized;
                float _angle = Vector3.Angle(_direction,transform.forward);

                if(_angle < viewAngle * 0.5f) // 시야내에 있는경우
                {
                    // 정보를 받아줄 Ray 발사
                    RaycastHit _hit;

                    if(Physics.Raycast(transform.position+transform.up, _direction, out _hit, viewDistance))
                    {
                        if(_hit.transform.name == "Player")
                        {
                            Debug.Log("플레이어가 NPC 시야각 내에 있습니다.");
                            Debug.DrawRay(transform.position + transform.up, _direction, Color.blue);
                            thePig.Run(_hit.transform.position);
                        }
                    }
                }
            }
        }

    }
}
