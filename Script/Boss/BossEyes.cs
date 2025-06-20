using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossEyes : MonoBehaviour
{
    public Light spotlight_L;
    public Light spotlight_R;
    public Light pointlight_M;

    public PlayerState playerState;
    public BossControll bossControll;
    public GameManger gameManger;
    public GameObject leftEyeObj;
    public GameObject rightEyeObj;
    public LeftEye leftEye;
    public RightEye rightEye;
    private Vector3 eyeStartPosition;
    public float moveSpeed = 7f;    // Z축 이동 속도
    public float moveXSpeed = 2f;
    public float moveDistance = 0f; // 이동할 최대 거리
    public bool moveBool = false;
    private Vector3 rayDirection;

    void Start()
    {
        eyeStartPosition = transform.position;

        // 감지 방향을 아래로 45도 기울임
        rayDirection = Quaternion.Euler(-45, 0, 0) * transform.forward;

        spotlight_L.intensity = 0; // -> 30
        spotlight_R.intensity = 0; // -> 30
        pointlight_M.intensity = 0; // -> 10
    }

    void Update()
    {
        CheckPlayerDetection();

        if (bossControll.detectionStart == true && gameManger.point == 5) // 페이즈원
        {
            spotlight_L.intensity = 30; // -> 30
            spotlight_R.intensity = 30; // -> 30
            pointlight_M.intensity = 10; // -> 10

            // 이동 거리 조건에 따라 회전
            if (moveBool == false)
            {
                if (moveDistance < 250)
                {
                    spotlight_L.transform.rotation *= Quaternion.Euler(0, -moveSpeed * Time.deltaTime, 0);
                    spotlight_R.transform.rotation *= Quaternion.Euler(0, -moveSpeed * Time.deltaTime, 0);
                    pointlight_M.transform.position -= new Vector3(moveXSpeed * Time.deltaTime, 0, 0);
                    leftEyeObj.transform.rotation *= Quaternion.Euler(0, moveSpeed * Time.deltaTime, 0);
                    rightEyeObj.transform.rotation *= Quaternion.Euler(0, moveSpeed * Time.deltaTime, 0);
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
                    spotlight_L.transform.rotation *= Quaternion.Euler(0, moveSpeed * Time.deltaTime, 0);
                    spotlight_R.transform.rotation *= Quaternion.Euler(0, moveSpeed * Time.deltaTime, 0);
                    pointlight_M.transform.position += new Vector3(moveXSpeed * Time.deltaTime, 0, 0);
                    leftEyeObj.transform.rotation *= Quaternion.Euler(0, -moveSpeed * Time.deltaTime, 0);
                    rightEyeObj.transform.rotation *= Quaternion.Euler(0, -moveSpeed * Time.deltaTime, 0);
                    moveDistance--; // 이동 거리 감소
                }
                else
                {
                    // moveDistance가 0에 도달했을 때 moveBool을 false로 설정
                    moveBool = false;
                }
            }
        }
        else if (bossControll.detectionStart == true && gameManger.point == 6) // 페이즈투
        {
            // 이동 거리 조건에 따라 회전
            if (moveBool == false)
            {
                if (moveDistance < 100)
                {
                    spotlight_L.transform.rotation *= Quaternion.Euler(0, -moveSpeed * 2 * Time.deltaTime, 0);
                    spotlight_R.transform.rotation *= Quaternion.Euler(0, -moveSpeed * 2 * Time.deltaTime, 0);
                    pointlight_M.transform.position -= new Vector3(moveXSpeed * Time.deltaTime, 0, 0);
                    leftEyeObj.transform.rotation *= Quaternion.Euler(0, moveSpeed * 2 * Time.deltaTime, 0);
                    rightEyeObj.transform.rotation *= Quaternion.Euler(0, moveSpeed * 2 * Time.deltaTime, 0);
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
                if (moveDistance > -100) // moveDistance가 0보다 클 때만 감소
                {
                    spotlight_L.transform.rotation *= Quaternion.Euler(0, moveSpeed * 2 * Time.deltaTime, 0);
                    spotlight_R.transform.rotation *= Quaternion.Euler(0, moveSpeed * 2 * Time.deltaTime, 0);
                    pointlight_M.transform.position += new Vector3(moveXSpeed * Time.deltaTime, 0, 0);
                    leftEyeObj.transform.rotation *= Quaternion.Euler(0, -moveSpeed * 2 * Time.deltaTime, 0);
                    rightEyeObj.transform.rotation *= Quaternion.Euler(0, -moveSpeed * 2 * Time.deltaTime, 0);
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

    private void CheckPlayerDetection()
    {
        // 왼쪽 눈과 오른쪽 눈에서 플레이어가 감지된 경우
        if (leftEye.playerDetected || rightEye.playerDetected)
        {
            // 플레이어가 감지된 상태에서 사망 여부 판단
            // 테스트
            playerState.playerDeath = true;
        }
    }
}