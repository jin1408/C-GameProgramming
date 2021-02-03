using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public float maxDelay;
    public float curDelay;
    public ObjectManager objectManager;


    //Player를 따라가기 위한 로직을 구성하는 변수들
    public Vector3 followPos;
    public int followDelay;
    public Transform parent;
    public Queue<Vector3> parentPos;

    void Awake()
    {
        parentPos = new Queue<Vector3>();    
    }

    void Update()
    {
        Watch();
        Follow();
        Fire();
        Reload();
    }

    void Watch()    //따라갈 위치를 계속 갱신해주는 함수
    {
        if(!parentPos.Contains(parent.position))
            parentPos.Enqueue(parent.position); //부모의 위치를 큐에 추가하는 과정

        if (parentPos.Count > followDelay)
            followPos = parentPos.Dequeue();
        else if (parentPos.Count < followDelay)
            followPos = parent.position;
    }

    void Follow()
    {
        transform.position = followPos;
    }

    void Fire()
    {
        // 매개변수 오브젝트를 생성하는 함수
        if (!(Input.GetButton("Fire1")))
            return;
        if (curDelay < maxDelay)
            return;

        GameObject bullet = objectManager.MakeObj("BulletFollower");
        bullet.transform.position = transform.position;

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);   //총알 쏘는거

        curDelay = 0;
    }

    void Reload()
    {
        curDelay += Time.deltaTime;
    }


}
