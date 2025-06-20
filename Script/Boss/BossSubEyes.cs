using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSubEyes : MonoBehaviour
{
    public PlayerState playerState;
    public bool radioDestroy;

    void Start()
    {
        
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 "Player" 태그를 가지고 있는지 확인
        if (other.CompareTag("Player"))
        {
            // 테스트
            playerState.playerDeath = true;
                
            Debug.Log("Player Detected directly");
        }
        if (other.CompareTag("Radio"))
        {
            radioDestroy = true;
            Destroy(this.gameObject);
            Debug.Log("라디오");
        }
    }

}
