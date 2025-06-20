using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{


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
            Debug.Log("Player hit");
        }
    }
}
