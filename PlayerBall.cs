using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBall : MonoBehaviour
{
    public float jump;
    public int itemCount;
    public Manage manager;

    bool isJump;
    
    AudioSource audio;
    Rigidbody rigid;
    void Awake()
    {
        audio = GetComponent<AudioSource>();
        isJump = false;
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && !isJump)
        {
            isJump = true;
            rigid.AddForce(new Vector3(0, jump, 0), ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        rigid.AddForce(new Vector3(h, 0, v), ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
            isJump = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item" )
        {
            itemCount++;
            audio.Play();
            other.gameObject.SetActive(false);
        }
        else if(other.tag == "Finish")
        {
            if(itemCount == manager.TotalItemCount) //아이템 전부 모았을 때
            {
                SceneManager.LoadScene(manager.stage+1);
            }
            else //그렇지 않다면? 재시작
            {
                SceneManager.LoadScene(manager.stage);
            }
        }
    }
}
