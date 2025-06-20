using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRay : MonoBehaviour
{
    public float maxDistance = 100;
    public float rotationAngle = 45f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCreate() { }
    void OnUpdate() { }

    void OnDrawGizmos()
    {
        // 지정된 각도로 회전된 방향을 계산
        Vector3 rotatedDirection = Quaternion.Euler(0, rotationAngle, 0) * transform.forward;

        RaycastHit hit;
        // SphereCast 수행 (발사 위치, 구의 반경, 회전된 방향, 충돌 결과, 최대 거리)
        bool isHit = Physics.SphereCast(transform.position, transform.lossyScale.x / 2, rotatedDirection, out hit, maxDistance);

        Gizmos.color = Color.red;
        if (isHit)
        {
            Gizmos.DrawRay(transform.position, rotatedDirection * hit.distance);
            Gizmos.DrawWireSphere(transform.position + rotatedDirection * hit.distance, transform.lossyScale.x / 2);
        }
        else
        {
            Gizmos.DrawRay(transform.position, rotatedDirection * maxDistance);
        }
    }
}
