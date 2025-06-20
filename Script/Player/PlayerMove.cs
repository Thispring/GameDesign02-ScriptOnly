using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 z축 이동제한은 타 오브젝트 콜라이더로 제한됨
public class PlayerMove : MonoBehaviour
{
    public Animator animator;
    public new Rigidbody rigidbody;
    public PlayerState playerState;
    public float speed = 5f;
    private Vector3 dir = Vector3.zero;
    [SerializeField]
    private float rotSpeed = 3;
    private CapsuleCollider capsuleCollider;
    private float originalColliderHeight = 1.7f;
    private float crouchColliderHeight = 1.5f;
    public Vector3 originalCenter = new Vector3(0, 0.85f, 0);
    public Vector3 crouchCenter = new Vector3(0, 0.75f, 0);
    public bool isCrouching = false;
    [SerializeField]
    private float initialYPosition; // 초기 Y 위치를 저장할 변수
    private float crouchSmoothness = 0.1f;  // 보간 속도 조절 변수
    public bool isColliding;
    public float isIdleDeath = 0;
    public bool dirControll = true;

    void Start()
    {
        isIdleDeath = 0;
        speed = 5f;
        rigidbody = this.GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        originalCenter = capsuleCollider.center;
        initialYPosition = transform.position.y;    // 게임 시작 시 초기 Y 위치 저장
    }

    void Update()
    {
        if (dirControll)
        {
            dir.x = Input.GetAxis("Horizontal");
            dir.z = Input.GetAxis("Vertical");
        }

        if (dir.magnitude > 1f)
        {
            dir = dir.normalized; // 정규화
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && isCrouching == false)  // 앉기
        {
            //Crouch();
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl) && isCrouching == true)   // 일어서기
        {
            //StandUp();
        }

        //Test();
    }

    private void FixedUpdate()
    {
        if (playerState.playerDeath == true && isIdleDeath == 0 && dir == Vector3.zero)
        {
            isIdleDeath = 1;
            animator.SetTrigger("isIdleD");
            Debug.Log("D애니메이션");
        }

        if (!isCrouching)
        {
            if (dir != Vector3.zero)
            {
                transform.forward = Vector3.Lerp(transform.forward, dir, rotSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime);
            }

            bool isMove = Mathf.Abs(dir.x) > 0 || Mathf.Abs(dir.z) > 0;
            //animator.SetBool("isMove", isMove); // 움직임 애니메이션
            rigidbody.MovePosition(transform.position + dir * speed * Time.deltaTime);
            if (!isColliding)
            {
                animator.SetBool("isMove", isMove); // 움직임 애니메이션
                animator.SetBool("isPush", false);
                if (playerState.playerDeath == true && isIdleDeath == 0 && dir != Vector3.zero)
                {
                    isIdleDeath = 1;
                    speed = 0;
                    animator.SetBool("isMove", false);
                    animator.SetTrigger("isMoveD");
                }
            }
            else if (isColliding)
            {
                animator.SetBool("isMove", false);
                animator.SetBool("isPush", true);
                animator.SetBool("isPushR", true);
                animator.SetBool("isPushR", isMove); // 움직임 애니메이션

                if (playerState.playerDeath == true && isIdleDeath == 0 && dir != Vector3.zero)
                {
                    isIdleDeath = 1;
                    speed = 0;
                    //animator.SetBool("isPush", false);
                    animator.SetTrigger("isPushD");
                }
            }
        }
        else    // 앉으며 이동 시 속도 제한
        {
            if (dir != Vector3.zero)
            {
                transform.forward = Vector3.Lerp(transform.forward, dir, (rotSpeed - 2.5f) * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), (rotSpeed - 2.5f) * Time.deltaTime);
            }

            bool isDownMove = Mathf.Abs(dir.x) > 0 || Mathf.Abs(dir.z) > 0;
            animator.SetBool("isSitMove", isDownMove);
            rigidbody.MovePosition(transform.position + dir * (speed - 2.5f) * Time.deltaTime);
        }
    }

    private void Crouch()
    {
        isCrouching = true;
        animator.SetBool("isMove", false);
        animator.SetBool("isSit", true);
        capsuleCollider.height = crouchColliderHeight;
        capsuleCollider.center = crouchCenter;
        // 초기 Y 위치로 보간 (Rigidbody로 처리)
        //Vector3 targetPosition = new Vector3(transform.position.x, initialYPosition, transform.position.z);
        //rigidbody.MovePosition(Vector3.Lerp(transform.position, targetPosition, crouchSmoothness));
    }

    private void StandUp()
    {
        isCrouching = false;
        animator.SetBool("isSit", false);
        capsuleCollider.height = originalColliderHeight;
        capsuleCollider.center = originalCenter;
        // 다시 서 있을 때 초기 위치로 복귀 (Rigidbody로 처리)
        Vector3 targetPosition = new Vector3(transform.position.x, initialYPosition, transform.position.z);
        rigidbody.MovePosition(Vector3.Lerp(transform.position, targetPosition, crouchSmoothness));
    }

    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.LeftBracket))  // 속도 증가
        {
            speed += 1;
            Debug.Log("스피드업");
        }

        if (Input.GetKeyDown(KeyCode.RightBracket)) // 속도 감소
        {
            speed -= 1;
            Debug.Log("스피드다운");
        }
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Object"))
        {
            isColliding = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Object"))
        {
            isColliding = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Object"))
        {
            isColliding = false;
            animator.SetBool("isPush", false);
            animator.SetBool("isPushR", false);
        }
    }
    */
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Object") || collision.gameObject.CompareTag("Radio") || collision.gameObject.CompareTag("MoveObject"))
        {
            isColliding = true;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Object") || collision.gameObject.CompareTag("Radio") || collision.gameObject.CompareTag("MoveObject"))
        {
            isColliding = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Object") || collision.gameObject.CompareTag("Radio") || collision.gameObject.CompareTag("MoveObject"))
        {
            isColliding = false;
            animator.SetBool("isMove", false);
            animator.SetBool("isPush", false);
            animator.SetBool("isPushR", false);
        }
    }

    public void ResetAnimatorStates()
    {
        animator.SetBool("isMove", false);
        animator.SetBool("isPush", false);
        animator.SetBool("isPushR", false);
        Debug.Log("Animator states reset.");
    }
}
