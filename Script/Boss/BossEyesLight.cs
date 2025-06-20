using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEyesLight : MonoBehaviour
{
    public GameObject spotlight_R;
    public GameObject spotlight_L;
    public GameObject pointlight_M;
    public GameObject pointlight_T;
    public float moveSpeed = 7f;                  // Z축 이동 속도
    public float moveXSpeed = 4f;
    public float moveDistance = 0f;              // 이동할 최대 거리
    public bool moveBool = false;
    public BossControll bossControll;
    [SerializeField]
    private GameManger gameManger;
    
    void Start()
    {
        gameManger = FindAnyObjectByType<GameManger>();
    }
    
    void Update()
    {
        if (bossControll.detectionStart == true && gameManger.point == 3)
        {
            // 이동 거리 조건에 따라 회전
            if (moveBool == false)
            {
                if (moveDistance < 250)
                {
                    // 오른쪽으로 회전
                    spotlight_R.transform.rotation *= Quaternion.Euler(0, -moveSpeed * Time.deltaTime, 0);
                    spotlight_L.transform.rotation *= Quaternion.Euler(0, -moveSpeed * Time.deltaTime, 0);
                    pointlight_M.transform.position -= new Vector3(moveXSpeed * Time.deltaTime, 0, 0);
                    pointlight_T.transform.position -= new Vector3(moveXSpeed * Time.deltaTime, 0, 0);
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
                if (moveDistance > -250) // moveDistance가 0보다 클 때만 감소
                {
                    // 왼쪽으로 회전
                    spotlight_R.transform.rotation *= Quaternion.Euler(0, moveSpeed * Time.deltaTime, 0);
                    spotlight_L.transform.rotation *= Quaternion.Euler(0, moveSpeed * Time.deltaTime, 0);
                    pointlight_M.transform.position += new Vector3(moveXSpeed * Time.deltaTime, 0, 0);
                    pointlight_T.transform.position += new Vector3(moveXSpeed * Time.deltaTime, 0, 0);
                    moveDistance--; // 이동 거리 감소
                }
                else
                {
                    // moveDistance가 0에 도달했을 때 moveBool을 false로 설정
                    moveBool = false;
                }
            }
        }
        else if (bossControll.detectionStart == true && gameManger.point == 4)
        {
            // 이동 거리 조건에 따라 회전
            if (moveBool == false)
            {
                if (moveDistance < 250)
                {
                    // 오른쪽으로 회전
                    spotlight_R.transform.rotation *= Quaternion.Euler(0, -moveSpeed * 2 * Time.deltaTime, 0);
                    spotlight_L.transform.rotation *= Quaternion.Euler(0, -moveSpeed * 2 * Time.deltaTime, 0);
                    pointlight_M.transform.position -= new Vector3(moveXSpeed * Time.deltaTime, 0, 0);
                    pointlight_T.transform.position -= new Vector3(moveXSpeed * Time.deltaTime, 0, 0);
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
                if (moveDistance > -250) // moveDistance가 0보다 클 때만 감소
                {
                    // 왼쪽으로 회전
                    spotlight_R.transform.rotation *= Quaternion.Euler(0, moveSpeed * 2 * Time.deltaTime, 0);
                    spotlight_L.transform.rotation *= Quaternion.Euler(0, moveSpeed * 2 * Time.deltaTime, 0);
                    pointlight_M.transform.position += new Vector3(moveXSpeed * Time.deltaTime, 0, 0);
                    pointlight_T.transform.position += new Vector3(moveXSpeed * Time.deltaTime, 0, 0);
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
}
