using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallChecker : MonoBehaviour
{
    public PlayerState playerState;
    public GameManger gameManger;
    public AudioSource fallObj;
    public GameObject checkPointObject;
    
    void Start()
    {
        
    }

    void Update()
    {
        //VolumeTest();
    }
    private void VolumeTest()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            fallObj.volume = 1;
            fallObj.Play();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 Player 태그를 가진 경우
        if (other.CompareTag("Player"))
        {
            Debug.Log("떨어짐");
            playerState.playerDeath = true;
        }
        if (other.CompareTag("Object") && gameManger.point == 3)
        {
            Debug.Log("떨어짐");
            fallObj.Play();
            gameManger.point += 1;
            Destroy(checkPointObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        // 충돌한 오브젝트가 Player 태그를 가진 경우
        if (other.CompareTag("Player"))
        {
            playerState.playerDeath = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // 충돌한 오브젝트가 Player 태그를 가진 경우
        if (other.CompareTag("Player"))
        {
            playerState.playerDeath = false;
        }
    }
}
