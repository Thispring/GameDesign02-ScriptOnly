using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    public GameObject obj01;
    public GameObject obj02;
    public GameObject obj03;
    public GameObject obj04;
    public float moveSpeed = 5f;                  // Z축 이동 속도
    public float XmoveSpeed = 2f;
    public float moveDistance = 0f;              // 이동할 최대 거리
    public bool moveBool = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 이동 거리 조건에 따라 회전
        if (moveBool == false)
        {
            if (moveDistance < 1800)
            {
                // 오른쪽으로 회전
                obj01.transform.rotation *= Quaternion.Euler(0, moveSpeed * Time.deltaTime, 0);
                obj02.transform.rotation *= Quaternion.Euler(0, moveSpeed * Time.deltaTime, 0);
                obj03.transform.position += new Vector3(XmoveSpeed * Time.deltaTime, 0, 0);
                obj04.transform.position += new Vector3(XmoveSpeed * Time.deltaTime, 0, 0);
                moveDistance++; // 이동 거리 증가
            }
            else
            {
                // moveDistance가 2500에 도달했을 때 moveBool을 true로 설정
                moveBool = true;
            }
        }
        else
        {
            if (moveDistance > -1800) // moveDistance가 0보다 클 때만 감소
            {
                // 왼쪽으로 회전
                obj01.transform.rotation *= Quaternion.Euler(0, -moveSpeed * Time.deltaTime, 0);
                obj02.transform.rotation *= Quaternion.Euler(0, -moveSpeed * Time.deltaTime, 0);
                obj03.transform.position -= new Vector3(XmoveSpeed * Time.deltaTime, 0, 0);
                obj04.transform.position -= new Vector3(XmoveSpeed * Time.deltaTime, 0, 0);
                moveDistance--; // 이동 거리 감소
            }
            else
            {
                // moveDistance가 0에 도달했을 때 moveBool을 false로 설정
                moveBool = false;
            }
        }
    }
}
