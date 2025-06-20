using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEyesRadio : MonoBehaviour
{
    public RadioObject radioObject;
    public PlayerState playerState;
    public float moveSpeed = 100f; // BossEyes 이동 속도

    private RadioObject currentTargetRadio; // 추적 중인 RadioObject
    private bool isChasing = false; // 현재 추적 상태

    void Update()
    {
        // 활성화된 RadioObject를 찾음
        FindActiveRadio();

        if (isChasing && currentTargetRadio != null)
        {
            // RadioObject를 추적
            ChaseRadio(currentTargetRadio);
        }
        else if (radioObject.radioOut == true)
        {
            Destroy(this.gameObject);
        }
    }

    private void FindActiveRadio()
    {
        // 모든 RadioObject 검색
        RadioObject[] radioObjects = FindObjectsOfType<RadioObject>();
        foreach (RadioObject radio in radioObjects)
        {
            if (radio.radioStart)
            {
                currentTargetRadio = radio; // 활성화된 RadioObject를 타겟으로 설정
                isChasing = true;
                return;
            }
        }

        // 활성화된 RadioObject가 없으면 추적 중단
        currentTargetRadio = null;
        isChasing = false;
    }

    private void ChaseRadio(RadioObject targetRadio)
    {
        // RadioObject의 위치 가져오기
        Vector3 targetPosition = targetRadio.transform.position;

        // Y축 값 유지
        targetPosition.y = transform.position.y;

        // 최종 위치 업데이트
        transform.position = targetPosition;
    }

    private void OnTriggerStay(Collider other)
    {
        // 충돌한 오브젝트가 Player 태그를 가진 경우
        if (other.CompareTag("Player") && radioObject.radioStart)
        {
            // 테스트
            playerState.playerDeath = true;
        }
    }
}
