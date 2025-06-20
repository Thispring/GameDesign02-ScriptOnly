using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField]
    private PlayerState playerState;
    [SerializeField]
    private BossControll bossControll;
    [SerializeField]
    private GameManger gameManger;

    void Start()
    {
        bossControll = FindObjectOfType<BossControll>();
        playerState = FindObjectOfType<PlayerState>();
        gameManger = FindObjectOfType<GameManger>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 "Player" 태그를 가지고 있는지 확인
        if (other.CompareTag("Player"))
        {
            // 체크포인트 위치를 리스트에 저장
            Vector3 checkpointPosition = transform.position;
            gameManger.checkpointPositions.Add(checkpointPosition);

            // 플레이어의 마지막 체크포인트 위치를 저장
            if (playerState != null)
            {
                bossControll.phase = true;
                gameManger.point++;
                playerState.SetLastCheckpoint(checkpointPosition);
                Destroy(this.gameObject);
            }
        }
        if (other.CompareTag("Object"))
        {
            // 체크포인트 위치를 리스트에 저장
            Vector3 checkpointPosition = transform.position;
            gameManger.checkpointPositions.Add(checkpointPosition);
            
            gameManger.point += 1;
            playerState.SetLastCheckpoint(checkpointPosition);
            Destroy(this.gameObject);
        }
    }
}
