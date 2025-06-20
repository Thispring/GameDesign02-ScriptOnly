using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 보스의 움직임만을 제어
public class BossControll : MonoBehaviour
{
    public GameManger gameManger;
    private Vector3 targetPosition = new Vector3(30.5f, -58.16f, -30.4f); // 이동할 목표 위치
    public Vector3 targetRotation = new Vector3(0, 0, 0);  // 목표 회전값 (예: 0, 550, 0 등)

    private Vector3 targetPosition2 = new Vector3(60f, -58.16f, -30.4f); // 이동할 목표 위치
    public Vector3 targetRotation2 = new Vector3(0, -150, 0);  // 목표 회전값 (예: 0, 550, 0 등)

    private Vector3 EyestargetPosition2 = new Vector3(61f, -32f, -37f); // 이동할 목표 위치
    public float rotationSpeed = 2f;  // 회전 속도
    [SerializeField]
    private float moveDistance = 0;
    public bool phase = false;
    public bool detectionStart = false;

    // BossEyes와 BossEyesLight의 Transform 참조 추가
    public Transform bossEyes;
    public GameObject bossPointLight;
    // 사운드
    public AudioSource slowSound;
    public AudioSource fastSound;
    private bool hasPlayedSlow = false; // slowSound 재생 여부
    private bool hasPlayedFast = false; // fastSound 재생 여부
    public Light rightLight;
    public Light leftLight;
    
    void Start()
    {
        rightLight.intensity = 0;
        leftLight.intensity = 0;
    }

    void Update()
    {
        //VolumeTest();

        if (gameManger.point == 5 && phase == true && moveDistance <= 10000000)
        {
            rightLight.intensity = 20;
            leftLight.intensity = 20;
            
            if (!hasPlayedSlow) // slowSound가 아직 재생되지 않았다면
            {
                slowSound.volume = 1;
                slowSound.Play();
                hasPlayedSlow = true; // 재생 상태를 true로 설정
            }
            phaseOne();
        }
        else if (gameManger.point == 6 && phase == true && moveDistance <= 20000000)
        {
            if (!hasPlayedFast) // fastSound가 아직 재생되지 않았다면
            {
                fastSound.volume = 1;
                fastSound.Play();
                hasPlayedFast = true; // 재생 상태를 true로 설정
            }
            phaseTwo();
        }
        else
        {
            slowSound.Stop();
            fastSound.Stop();
        }
    }
    private void VolumeTest()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            slowSound.volume = 1;
            slowSound.Play();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            fastSound.volume = 1;
            fastSound.Play();
        }
    }
    private void phaseOne()
    {
        // 목표 위치로 이동하는 벡터 계산
        Vector3 direction = (targetPosition - transform.position).normalized;

        // 이동 속도 설정
        float moveSpeed = 0.09f; // 한 번에 이동할 거리

        // 목표 위치까지 남은 거리 계산
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        // 목표 위치에 도달하지 않았고 moveDistance 조건을 만족하면 이동
        if (distanceToTarget > moveSpeed && moveDistance <= 1000000)
        {
            transform.position += direction * moveSpeed;
            moveDistance++;
        }
        else if (moveDistance <= 1000000)
        {
            // 목표 위치에 거의 도달하면 위치를 정확히 목표 위치로 설정
            moveDistance = 1000000; // 이동을 종료하기 위해 moveDistance를 최대값으로 설정
            detectionStart = true;
        }
    }

    private void phaseTwo()
    {
        // 목표 위치로 이동하는 벡터 계산
        Vector3 direction = (targetPosition2 - transform.position).normalized;

        // 이동 속도 설정
        float moveSpeed = 0.09f; // 한 번에 이동할 거리

        // 목표 위치까지 남은 거리 계산
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition2);

        // 목표 위치에 도달하지 않았고 moveDistance 조건을 만족하면 이동
        if (distanceToTarget > moveSpeed && moveDistance <= 2000000)
        {
            transform.position += direction * moveSpeed;
            moveDistance++;

            // BossEyes와 BossEyesLight도 이동
            if (bossEyes != null)
                bossEyes.position += direction * moveSpeed;
        }
        else if (distanceToTarget <= moveSpeed || moveDistance > 2000000)
        {
            // 목표 위치에 거의 도달하면 위치를 정확히 목표 위치로 설정
            transform.position = targetPosition2;  // 정확한 위치로 설정
            moveDistance = 2000000; // 이동을 종료하기 위해 moveDistance를 최대값으로 설정
            detectionStart = true; // phaseTwo 완료 후 detection 시작

            // BossEyes와 BossEyesLight도 정확한 위치로 설정
            if (bossEyes != null)
                bossEyes.position = EyestargetPosition2;
        }
    }
}
