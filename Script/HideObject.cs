using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObject : MonoBehaviour
{
    public bool behindHideObject = false; // 플레이어가 UI 뒤에 숨기
    private Rigidbody rb;
    public float boxHalfExtentsX = 0.5f; // 박스의 반 가로 길이
    public float boxHalfExtentsY = 0.5f; // 박스의 반 세로 길이
    public float boxHalfExtentsZ = 0.5f; // 박스의 반 깊이 길이
    public float maxDistance = 1f; // 박스 레이의 최대 거리
    public float rotationAngle = 0f; // 회전 각도
    public LayerMask detectionLayer; // Player 레이어만 감지하도록 설정
    public PlayerState playerState;
    public PlayerMove playerMove;
    private RaycastHit hit;
    private Vector3 objStartPosition;
    private bool playerCollided = false; // Player로 인한 x, z축 제어

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        objStartPosition = transform.position; // 초기 위치 설정
    }
    void Update()
    {
        DetectPlayer();
    }
    
    private void DetectPlayer()
    {
        Vector3 boxHalfExtents = new Vector3(boxHalfExtentsX, boxHalfExtentsY, boxHalfExtentsZ);
        Vector3 direction = Quaternion.Euler(0, rotationAngle, 0) * transform.forward;

        // BoxCast로 Player 레이어 감지
        if (Physics.BoxCast(transform.position, boxHalfExtents, direction, out hit, transform.rotation, maxDistance, detectionLayer))
        {
            if (hit.collider.CompareTag("Player"))
            {
                behindHideObject = true;
            }
            else
            {
                behindHideObject = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 boxHalfExtents = new Vector3(boxHalfExtentsX, boxHalfExtentsY, boxHalfExtentsZ);
        Vector3 direction = Quaternion.Euler(0, rotationAngle, 0) * transform.forward;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, boxHalfExtents * 2); // 시작 위치 표시

        // BoxCast로 충돌 감지 시 경로와 충돌 지점 표시
        if (Physics.BoxCast(transform.position, boxHalfExtents, direction, out hit, transform.rotation, maxDistance, detectionLayer))
        {
            // 충돌이 발생한 경우 빨간색 박스를 그려 충돌 지점을 표시
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + direction * hit.distance, boxHalfExtents * 2);
            Gizmos.DrawLine(transform.position, transform.position + direction * hit.distance); // 시작부터 충돌 지점까지의 선
        }
        else
        {
            // 충돌이 없을 때 최대 거리까지 파란색 박스를 그려 경로를 표시
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position + direction * maxDistance, boxHalfExtents * 2);
            Gizmos.DrawLine(transform.position, transform.position + direction * maxDistance); // 시작부터 최대 거리까지의 선
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 Player 태그를 가진 경우
        if (other.CompareTag("Player"))
        {
            playerCollided = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Player가 떠났을 때 움직임 초기화
        if (other.CompareTag("Player"))
        {
            playerCollided = false;
        }
    }

    // 수정된 메서드: 동적으로 상태 설정
    public void SetState(bool state)
    {
        behindHideObject = state;
    }
    /*
    private void FixedUpdate()
    {
        if (playerCollided)
        {
            Vector3 playerVelocity = GameObject.FindWithTag("Player").GetComponent<Rigidbody>().velocity;

            if (Mathf.Abs(playerVelocity.x) > Mathf.Abs(playerVelocity.z))
            {
                // 플레이어가 x축으로 이동할 때
                rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
                rb.velocity = new Vector3(playerVelocity.x, rb.velocity.y, 0);
            }
            else if (Mathf.Abs(playerVelocity.z) > Mathf.Abs(playerVelocity.x))
            {
                // 플레이어가 z축으로 이동할 때
                rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
                rb.velocity = new Vector3(0, rb.velocity.y, playerVelocity.z);
            }
        }
        else
        {
            // 플레이어가 닿지 않았을 경우 제약 해제
            rb.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
        }
    }
    */
}
