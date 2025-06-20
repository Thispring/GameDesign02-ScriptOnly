using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightEye : MonoBehaviour
{
    public PlayerState playerState;
    public ObjectMove objectMove;
    public float rotationAngle = 0;
    public bool playerDetected;
    public List<HideObject> hideObjects = new List<HideObject>();

    // 현재 활성화된 HideObject를 추적
    private HideObject currentActiveHideObject = null;

    void Start()
    {
        // 씬에서 모든 HideObject를 찾아 리스트에 추가
        HideObject[] foundObjects = FindObjectsOfType<HideObject>();

        foreach (HideObject obj in foundObjects)
        {
            hideObjects.Add(obj);
        }
    }

    void Update()
    {
        DetectAndDrawRays();
    }

    private void DetectAndDrawRays()
    {
        float sphereRadius = transform.lossyScale.x / 2;
        float maxDistance = 100f;
        float currentDistance = 0f;

        Vector3 rotatedDirection = Quaternion.Euler(0, rotationAngle, 0) * transform.forward;

        RaycastHit hit;
        playerDetected = false; // 플레이어 감지 상태 초기화

        while (Physics.SphereCast(transform.position + rotatedDirection * currentDistance, sphereRadius, rotatedDirection, out hit, maxDistance - currentDistance))
        {
            currentDistance += hit.distance;

            // MoveObject 탐지 처리
            if (hit.collider.CompareTag("MoveObject"))
            {
                if (objectMove.behindObjectMove)
                {
                    Debug.Log("플레이어가 MoveObject에 의해 숨겨짐");
                    playerDetected = false;
                    ResetHideObjectsState(); // 모든 HideObject의 상태를 초기화
                    break;
                }
            }
            // HideObject 탐지 처리
            else if (hit.collider.CompareTag("HideObject"))
            {
                var detectedHideObject = hit.collider.GetComponent<HideObject>();
                if (detectedHideObject != null)
                {
                    // 이전에 활성화된 HideObject를 비활성화
                    if (currentActiveHideObject != null && currentActiveHideObject != detectedHideObject)
                    {
                        currentActiveHideObject.SetState(false);
                    }

                    // 현재 HideObject를 활성화
                    detectedHideObject.SetState(true);
                    currentActiveHideObject = detectedHideObject; // 현재 활성화된 HideObject 업데이트

                    Debug.Log($"플레이어가 {detectedHideObject.gameObject.name}에 의해 숨겨짐");
                }

                // 리스트 전체 검사: 하나라도 true이면 playerDetected = false
                playerDetected = true; // 초기값은 true로 설정
                foreach (var hideObject in hideObjects)
                {
                    if (hideObject.behindHideObject)
                    {
                        playerDetected = false; // 하나라도 true면 감지되지 않음
                        break; // 더 이상 확인할 필요 없음
                    }
                }

                break; // 탐지 처리 종료
            }
            // Player 감지 처리
            else if (hit.collider.CompareTag("Player"))
            {
                bool isHidden = false;

                // ObjectMove 상태 확인
                if (objectMove != null && objectMove.behindObjectMove)
                {
                    isHidden = true;
                }

                // HideObject 리스트 순회하여 상태 확인
                foreach (var hideObject in hideObjects)
                {
                    if (hideObject.behindHideObject)
                    {
                        isHidden = true;
                        break; // 하나라도 숨겨진 상태면 즉시 탈출
                    }
                }

                // 숨겨진 상태가 아니면 플레이어 감지
                if (!isHidden)
                {
                    playerDetected = true;
                    Debug.Log("플레이어 감지");
                    break;
                }
                else
                {
                    playerDetected = false;
                    Debug.Log("플레이어가 숨겨진 상태");
                }
            }

            // 남은 거리 갱신
            currentDistance += 0.1f;
            if (currentDistance >= maxDistance) break;
        }

        // 모든 HideObject 상태 디버그 출력
        foreach (var obj in hideObjects)
        {
            Debug.Log($"{obj.gameObject.name} - behindHideObject: {obj.behindHideObject}");
        }
    }

    // 모든 HideObject의 상태를 초기화
    private void ResetHideObjectsState()
    {
        foreach (var obj in hideObjects)
        {
            obj.SetState(false);
        }
        currentActiveHideObject = null;
    }

    private void OnDrawGizmos()
    {
        float sphereRadius = transform.lossyScale.x / 2;
        float maxDistance = 100f;
        Vector3 rotatedDirection = Quaternion.Euler(0, rotationAngle, 0) * transform.forward;

        float currentDistance = 0f;
        RaycastHit hit;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sphereRadius); // 시작점에 원형 Gizmo

        while (Physics.SphereCast(transform.position + rotatedDirection * currentDistance, sphereRadius, rotatedDirection, out hit, maxDistance - currentDistance))
        {
            Vector3 startPoint = transform.position + rotatedDirection * currentDistance;
            Vector3 hitPoint = hit.point;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(startPoint, hitPoint); // 충돌 지점까지의 선

            if (hit.collider.CompareTag("UI"))
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(hitPoint, sphereRadius); // UI 충돌 위치
            }
            else if (hit.collider.CompareTag("Player"))
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(hitPoint, sphereRadius); // Player 충돌 위치
            }

            currentDistance += hit.distance;
            currentDistance += 0.1f; // 무한 루프 방지용 소량 추가
            if (currentDistance >= maxDistance) break;
        }
    }
}
