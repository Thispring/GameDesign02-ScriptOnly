using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioObject : MonoBehaviour
{
    public bool radioStart = false; // RadioObject의 상태를 나타내는 변수
    private Rigidbody rb;
    private bool playerCollided = false;
    public Light spotlight;
    public AudioSource audioSource;
    public GameManger gameManger;
    public bool radioOut;
    private Vector3 radioStartPosition;
    public PlayerState playerState;
    public PlayerMove playerMove;
    public float resetPoint;
    public bool destroyRadio;

    void Start()
    {
        resetPoint = 0;
        radioStartPosition = transform.position;
        audioSource.volume = 0;
        audioSource.Play();
        // 일정 시간마다 true/false를 반복
        InvokeRepeating(nameof(ToggleRadioState), 0f, 10f);
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //VolumeTest();

        if (playerState.playerDeath)
        {
            this.gameObject.transform.position = radioStartPosition;
        }

        if (resetPoint == 3)
        {
            this.gameObject.transform.position = radioStartPosition;
            //playerState.playerDeath = true;
            resetPoint = 0;
        }

        if (!radioStart)
        {
            spotlight.intensity = 0f;
        }
        else
        {
            spotlight.intensity = 30f;
        }

        // 특정 조건에서만 PlayRadio를 실행하도록 조건을 설정
        if (gameManger.point == 2)
        {
            if (radioStart)
            {
                audioSource.volume = 1;
            }
            else if (!radioStart)
            {
                audioSource.volume = 0;
            }
        }
        else
        {
            audioSource.volume = 0;
        }
    }
    private void VolumeTest()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            audioSource.volume = 1;
            audioSource.Play();
        }
    }
    private void ToggleRadioState()
    {
        if (!radioStart && gameManger.point == 2)
        {
            StartCoroutine(FadeInSound(5f)); // 5초 동안 볼륨을 서서히 증가
        }
        else
        {
            audioSource.volume = 0;
        }
    }

    private IEnumerator FadeInSound(float duration)
    {
        float startVolume = audioSource.volume;
        float targetVolume = 1f; // 목표 볼륨
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / duration);
            yield return null;
        }

        audioSource.volume = targetVolume; // 최종적으로 목표 볼륨에 도달
        radioStart = true;
        yield return new WaitForSeconds(5f);
        radioStart = false;
        resetPoint += 1;
    }

    // 라디오 제거 기능
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCollided = true;
        }
        if (other.CompareTag("BossEyes"))
        {
            radioOut = true;
            Destroy(this.gameObject);
            playerMove.isColliding = false;
        }
    }
    private void OnDestroy() 
    {
        playerMove.isColliding = false;
        destroyRadio = true;
        playerMove.ResetAnimatorStates();
    }
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
}