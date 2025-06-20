using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManger : MonoBehaviour
{
    public GameManger gameManger;
    public AudioSource mainBGM; // 메인 BGM AudioSource
    public AudioSource subBGM;  // 서브 BGM AudioSource

    private bool isMainPlaying = false; // 메인 BGM 상태
    private bool isSubPlaying = false;  // 서브 BGM 상태

    void Start()
    {
        // 초기화 (Main BGM 시작)
        if (!mainBGM.isPlaying)
        {
            mainBGM.Play();
            isMainPlaying = true;
        }
    }

    void Update()
    {
        //VolumeTest();

        // 특정 조건 확인 (예: 특정 키 입력)
        if ((gameManger.point == 1 || gameManger.point >= 3) && !isSubPlaying)
        {
            if (mainBGM.isPlaying)
            {
                mainBGM.Stop();
                isMainPlaying = false;
            }

            subBGM.Play();
            isSubPlaying = true;

            Debug.Log("사운드 플레이 탐색");
        }
        else if ((gameManger.point == 0 || gameManger.point == 2) && !isMainPlaying)
        {
            if (subBGM.isPlaying)
            {
                subBGM.Stop();
                isSubPlaying = false;
            }

            mainBGM.Play();
            isMainPlaying = true;

            Debug.Log("사운드 플레이 평소");
        }
    }
    private void VolumeTest()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            subBGM.Stop();
            mainBGM.Play();
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            mainBGM.Stop();
            subBGM.Play();
        }
    }
}