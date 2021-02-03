using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;

    public int enemyScore;

    public float speed;
    public int health;
    public Sprite[] sprites;

    public GameObject bulletA;
    public GameObject bulletB;
    public GameObject player;

    public GameObject boom;
    public GameObject Power;
    public GameObject coin;
    public ObjectManager objectManager;

    public float maxDelay;
    public float curDelay;

    SpriteRenderer spriteRenderer;  //피격된 비행물체가 잠시 다른 sprite로 변하는 것을
                                    //표현하기 위해 사용
    Rigidbody2D rigid;
   
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    void OnEnable()
    {
        switch (enemyName)
        {
            case "L":
                health = 10;
                break;
            case "M":
                health = 5;
                break;
            case "S":
                health = 3;
                break;

        }    
    }

    void Update()
    {
        Fire();
        Reload();
    }

    void Fire()
    {
        if (curDelay < maxDelay)
            return ;

        if(enemyName == "S")
        {
            GameObject bullet = objectManager.MakeObj("EnemyBulletA");
            bullet.transform.position = transform.position;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector3 drivVec = player.transform.position - transform.position;
            rigid.AddForce(drivVec.normalized * 3, ForceMode2D.Impulse);   //총알 쏘는거
        }
        else if(enemyName == "L"){
            GameObject bulletR = objectManager.MakeObj("EnemyBulletB");
            bulletR.transform.position = transform.position+Vector3.right*0.3f;
            Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
            Vector3 drivVecR = player.transform.position - (transform.position+Vector3.right*0.3f);
            rigidR.AddForce(drivVecR.normalized * 5, ForceMode2D.Impulse);   //총알 쏘는거

            GameObject bulletL = objectManager.MakeObj("EnemyBulletB");
            bulletL.transform.position = transform.position + Vector3.left * 0.3f;
            Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
            Vector3 drivVecL = player.transform.position - (transform.position+Vector3.left*0.3f);
            rigidL.AddForce(drivVecL.normalized * 5, ForceMode2D.Impulse);   //총알 쏘는거
        }

        curDelay = 0;
    }

    void Reload()
    {
        curDelay += Time.deltaTime;
    }

    public void OnHit(int dmg)
    {
        if (health <= 0)
            return;

        health -= dmg;
        spriteRenderer.sprite = sprites[1]; //피격 될 때 색 바뀜
        Invoke("ReturnSprite", 0.1f);   //시간 차 함수 적용, 뒤에 인자로 오는 함수명은
                                        // " "로 묶어서 표시해줘야함.

        if(health <= 0)
        {
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += enemyScore;
            gameObject.SetActive(false);
            int ran = Random.Range(0, 10);
            if (ran < 3){
                Debug.Log("Not item");
            }
            else if(ran < 6){
                GameObject coin = objectManager.MakeObj("ItemCoin");
                coin.transform.position = transform.position;
            }
            else if (ran < 8){
                GameObject power = objectManager.MakeObj("ItemPower");
                power.transform.position = transform.position;
            }
            else if (ran < 10)            {
                GameObject boom = objectManager.MakeObj("ItemBoom");
                boom.transform.position = transform.position;
            }

            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
        }
    }

    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0]; //비행 물체 색깔이 다시 원래대로 돌아오는 과정
    }

    void OnTriggerEnter2D(Collider2D collision) //총알이 적 비행체에 충돌 시 
    {
        if (collision.gameObject.tag == "BorderBullet")
        {
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
        }
           
        else if (collision.gameObject.tag == "Bullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.dmg);
            collision.gameObject.SetActive(false);  //총알도 충돌 시 삭제
        }
    }
}
