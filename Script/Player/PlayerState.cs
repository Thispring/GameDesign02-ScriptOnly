using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerState : MonoBehaviour
{
    public PlayerMove playerMove;
    public BossControll bossControll;
    public bool playerDeath = false;
    public bool playerRespawn = false;
    private Vector3 lastCheckpointPosition; // 마지막 체크포인트 위치 저장
    private Vector3 lastPosition;
    public bool detection = false;
    public bool respawnComplete;


    public TextMeshProUGUI textMeshProUGUI;
    public GameManger gameManger;

    void Start()
    {
        respawnComplete = false;
        lastCheckpointPosition = transform.position;
        lastPosition = transform.position; // 초기 위치 저장
    }

    void Update()
    {
        detection = false;

        lastPosition = transform.position; // 현재 위치 저장

        if (!playerDeath)
        {
            textMeshProUGUI.text = "Player 상태: live";
        }
        // 테스트
        else // playerDeath == true 상태
        {
            //animator.SetTrigger("isIdleD");
            playerMove.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            StartCoroutine(RespawnAfterDeath());
            textMeshProUGUI.text = "Player 상태: death";
            // playerMove.speed = 0;
        }
        
        //Test();
    }

    // 3초 뒤에 리스폰하는 코루틴
    private IEnumerator RespawnAfterDeath()
    {
        respawnComplete = true;
        yield return new WaitForSeconds(2f); // 3초 대기
        RespawnAtLastCheckpoint(); // 리스폰 처리
        playerMove.isIdleDeath = 0;
    }
    // 리스폰 높이 조정 변수
    [SerializeField]
    private float respawnHeightOffset = 2.5f;

    // 마지막 체크포인트 위치를 설정하는 메서드
    public void SetLastCheckpoint(Vector3 checkpointPosition)
    {
        lastCheckpointPosition = checkpointPosition;
    }

    private void RespawnAtLastCheckpoint()
    {
        // y 값에 5를 더해 더 높은 위치에서 시작
        Vector3 respawnPosition = new Vector3(lastCheckpointPosition.x, lastCheckpointPosition.y + respawnHeightOffset, lastCheckpointPosition.z);
        transform.position = respawnPosition; // 마지막 체크포인트 위치로 이동
        playerDeath = false; // 사망 상태 해제
        playerRespawn = false;
        playerMove.speed = 5f;
        respawnComplete = false;
    }

    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.M)) // 죽기
        {
            playerDeath = true;
        }

        if (Input.GetKeyDown(KeyCode.L)) // 리스폰
        {
            playerRespawn = true;
        }

        if (Input.GetKeyDown(KeyCode.O)) // 테스트 리스폰
        {
            playerDeath = false;
        }

        if (Input.GetKeyDown(KeyCode.V)) // 체크포인트 확인용
        {
            Debug.Log(lastCheckpointPosition);
        }

        if (Input.GetKeyDown(KeyCode.P)) // 체크포인트1 바로이동
        {
            transform.position = new Vector3(-1.35f, -51, -46f);
            gameManger.point = 3;
            bossControll.phase = true;
        }
    }
}
