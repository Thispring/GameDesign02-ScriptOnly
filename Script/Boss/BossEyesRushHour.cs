using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BossEyesRushHour : MonoBehaviour
{
    public Animator playerAni;
    public GameManger gameManger;
    public PlayerState playerState;
    public PlayerMove playerMove;
    public Light spotlight;
    [SerializeField]
    private List<Transform> rushHourPoints = new List<Transform>();
    public int currentPointIndex = 0; // 현재 이동할 리스트 인덱스
    private bool isMoving = false; // 이동 상태 확인
    private float moveDuration = 1.65f; // 이동 시간 (3초)
    public bool isRespawn;
    public Vector3 startPosition;

    void Start()
    {
        // "RushHourPoint" 태그가 붙은 모든 오브젝트를 찾아 정렬
        GameObject[] points = GameObject.FindGameObjectsWithTag("RushHourPoint");
        rushHourPoints = points
            .OrderBy(point => ExtractNumber(point.name)) // LINQ를 사용해 이름의 숫자를 기준으로 정렬
            .Select(point => point.transform)
            .ToList();

        startPosition = this.gameObject.transform.position;
        if (rushHourPoints.Count == 0)
        {
            Debug.LogWarning("RushHourPoint 태그가 붙은 오브젝트가 없습니다!");
        }
    }

    void Update()
    {
        // 특정 조건에서 이동 시작
        if (gameManger.point == 1 && !isMoving && isRespawn == false && playerState.respawnComplete == false)
        {
            StartCoroutine(MoveThroughPoints());
        }
        else if (gameManger.point == 1 && playerState.playerDeath && playerState.respawnComplete == true)
        {
            StartCoroutine(RushHourRespawn());
        }
        else if (gameManger.point == 2)
        {
            spotlight.intensity = 0f;
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // 충돌한 오브젝트가 Player 태그를 가진 경우
        if (other.CompareTag("Player"))
        {
            // 테스트
            playerAni.SetTrigger("isMoveD"); // 움직임 애니메이션
            playerState.playerDeath = true;
            playerMove.isIdleDeath = 0;
        }
    }
    IEnumerator MoveThroughPoints()
    {
        spotlight.intensity = 30f;
        isMoving = true;
        isRespawn = true;

        while (currentPointIndex < rushHourPoints.Count)
        {
            // 플레이어가 죽었는지 확인
            if (playerState.playerDeath)
            {
                Debug.Log("플레이어가 사망하여 이동을 중단합니다.");
                isMoving = false;
                yield return StartCoroutine(RushHourRespawn()); // 리스폰 루틴 실행
                yield break; // 코루틴 종료
            }

            // 현재 이동할 목표 지점
            Transform targetPoint = rushHourPoints[currentPointIndex];
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = targetPoint.position;
            float elapsedTime = 0f;

            // 목표 지점으로 이동
            while (elapsedTime < moveDuration)
            {
                if (playerState.playerDeath)
                {
                    Debug.Log("플레이어가 사망하여 이동 중단");
                    isMoving = false;
                    yield return StartCoroutine(RushHourRespawn()); // 리스폰 루틴 실행
                    yield break; // 코루틴 종료
                }

                transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 이동 완료 후 위치를 정확히 설정
            transform.position = targetPosition;

            // 다음 포인트로 이동
            currentPointIndex++;
            moveDuration = Mathf.Max(moveDuration - 0.1f, 0.5f); // 이동 시간 감소 (최소 0.5초 제한)
        }

        // 모든 지점을 다 이동한 후 루틴 종료
        isMoving = false;
        Debug.Log("모든 지점 이동 완료");
    }

    private IEnumerator RushHourRespawn()
    {
        Debug.Log("RushHourRespawn: 초기 위치로 복귀 후 재시작 대기");

        // 시작 위치로 이동
        transform.position = startPosition;

        // 현재 포인트 초기화
        currentPointIndex = 0;

        // 이동 시간 초기화
        moveDuration = 1.65f;

        // 플레이어 상태 초기화
        playerState.playerDeath = false;

        // RushHourPoints가 유효한지 확인
        if (rushHourPoints == null || rushHourPoints.Count == 0)
        {
            Debug.LogError("RushHourRespawn: rushHourPoints 리스트가 비어 있습니다!");
            yield break; // 코루틴 종료
        }

        yield return new WaitForSeconds(2.5f); // 10초 대기

        // 이동 재개
        isMoving = false;
        StartCoroutine(MoveThroughPoints());
    }
    private int ExtractNumber(string name)
    {
        string[] parts = name.Split('_');
        if (parts.Length > 1 && int.TryParse(parts[1], out int number))
        {
            return number;
        }
        return 0;
    }
}
