using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPoint : MonoBehaviour
{
    public bool CamMoveStart;
    public bool PlayerIn;
    public Vector3 startPos;
    public Vector3 backPos = new Vector3 (-35.5f, -51.2f, -60f);
    public GameObject wall;
    void Start()
    {
        wall.SetActive(false);
        startPos = this.gameObject.transform.position;
    }

    void Update()
    {
        if(PlayerIn)
        {
            this.gameObject.transform.position = backPos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 "Player" 태그를 가지고 있는지 확인
        if (other.CompareTag("Player"))
        {
            CamMoveStart = true;
            PlayerIn = true;
            wall.SetActive(true);
        }
    }
}
