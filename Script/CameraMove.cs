using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject player;
    [SerializeField]
    public GameManger gameManger;
    [SerializeField]
    private PlayerMove playerMove;

    private Vector3 pos = new Vector3(0, 1, -6.5f); // 기본 카메라 위치
    private Vector3 skypos = new Vector3(0, 7.5f, -40); // 스카이뷰 위치
    private Vector3 rushHourpos = new Vector3(-20.5f, -40f, -65.5f); // 러시 아워 위치

    public bool skyView = false;
    public float transitionSpeed = 0.5f; // 이동 속도
    public bool isTransitioning = false; // 코루틴 실행 여부 확인
    public CamPoint camPoint;

    void Start()
    {
        transitionSpeed = 0.5f;
    }

    void Awake()
    {
        playerMove = FindAnyObjectByType<PlayerMove>();
        gameManger = FindAnyObjectByType<GameManger>();
    }

    void Update()
    {
        // 러시 아워 이동
        if (camPoint.CamMoveStart && !isTransitioning || gameManger.point == 1 && !isTransitioning)
        {
            if (gameManger.point == 1)
            {
                camPoint.CamMoveStart = false;
                Debug.Log(camPoint.CamMoveStart);
            }
            StartCoroutine(MoveToPosition(rushHourpos, Quaternion.Euler(90, 0, 0), null));
        }
        // 기본 시야 복귀
        else if (gameManger.point == 2 && !isTransitioning && !isCamActive)
        {
            transitionSpeed = 2f;
            Debug.Log(camPoint.CamMoveStart);
            Debug.Log("기본 시야로 복귀 중");
            playerMove.speed = 0;
            StartCoroutine(MoveToPosition(player.transform.position + pos, Quaternion.Euler(0, 0, 0), () =>
            {
                StartCoroutine(ChangeCamDelay(1f * Time.deltaTime));
                // 기본 시야로 전환
                Debug.Log("기본 시야로 복귀");
            }));
        }
        // 기본 시야 또는 스카이뷰 상태
        else if (!isTransitioning || !isCamActive)
        {
            if (!skyView)
            {
                // 기본 시야
                this.gameObject.transform.position = player.transform.position + pos;
                this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                // 스카이뷰
                this.gameObject.transform.position = skypos;
                this.gameObject.transform.rotation = Quaternion.Euler(90, 0, 0);
            }
        }
    }

    IEnumerator MoveToPosition(Vector3 targetPosition, Quaternion targetRotation, System.Action onComplete)
    {
        isTransitioning = true; // 코루틴 실행 중 상태 설정

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * transitionSpeed;

            // 위치와 회전 보간
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime);
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime);

            yield return null;
        }

        transform.position = targetPosition;
        transform.rotation = targetRotation;

        // 이동 완료 후 추가 작업 실행
        onComplete?.Invoke();

        isTransitioning = false; // 코루틴 실행 상태 해제
    }
    public bool isCamActive = false;
    // 1초 뒤에 bool 상태를 변경하는 코루틴
    IEnumerator ChangeCamDelay(float delay)
    {
        // delay 시간만큼 대기
        yield return new WaitForSeconds(delay);

        // bool 상태 변경
        isCamActive = true;
        playerMove.speed = 5f;
    }
}