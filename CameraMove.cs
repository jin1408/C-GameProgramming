using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Transform playerTransform;
    Vector3 Offset;
    // Start is called before the first frame update
    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        Offset = transform.position - playerTransform.position; //현재 카메라 위치 - 플레이어 위치
    }

    // Update is called once per frame
    void LateUpdate()   //카메라나 UI에 쓰임
    {
        transform.position = playerTransform.position + Offset;  //이동에 관한 함수? 자료형?

    }
}
