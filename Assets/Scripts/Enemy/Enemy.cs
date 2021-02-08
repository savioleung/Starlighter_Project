using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected GameObject player;

    Vector2 force = new Vector2(0, 0);

    [SerializeField]
    protected GameObject shootPos, weakPointPos, beamLaser, weakPoint;//射撃位置、弱点位置、レーサー,弱点
    //レーサーの速度
    public float laserSpeed = 20;
    //AIがランダムに動くタイム
    protected float t = 1.5f, reloadTime = 2.5f;
    public float goneTime = 3.0f;
    //移動速度
    protected float jumpSpeed = 400.0f;
    //察知する距離
    protected float r = 22.0f, h = 1;
    //弱点が出るまでのHP
    public int HP = 3;

    public bool breakable = false;
    //始動条件
    protected bool move = false, onGround = false;
    // Start is called before the first frame update
    virtual protected void Start()
    {
        //プレイヤー
        player = player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        //弱点の位置初期化
        weakPoint.transform.position = weakPointPos.transform.position;
        //弱点露出まで弱点を消す
        weakPoint.gameObject.SetActive(false);

    }

    // Update is called once per frame
    virtual protected void Update()
    {
        //プレイヤーとの距離
        var dis = Vector3.Distance(player.transform.position, this.transform.position);
        //プレイヤーが距離内で始動
        if (dis < r && !move)
        {
            move = true;
        }
        if (move)
        {
            t += Time.deltaTime;
            //タイムごとに動く
            if (t >= reloadTime)
            {
                GameObject laser = Instantiate(beamLaser, shootPos.transform.position, Quaternion.identity);
                // プレイヤーの座標
                Vector3 targetPos = player.transform.position;

                // 向きの生成
                Vector3 shotForward = Vector3.Scale((targetPos - transform.position), new Vector3(1, 1, 0)).normalized;

                // 弾に速度を与える
                laser.GetComponent<Rigidbody2D>().velocity = shotForward * laserSpeed;

                turn();
                movement();

                Destroy(laser, goneTime);
                t = 0;

            }
            //弱点露出処理
            if (HP <= 0)
            {
                if (!breakable)
                {
                    weakPoint.gameObject.SetActive(true);
                    if (weakPoint.GetComponent<weakPoint>().isDeath)
                    {
                        Destroy(this.gameObject);
                    }
                }
                else
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }
    void turn()
    {
        if (player.transform.position.x > transform.position.x)
        {
            this.transform.localScale = new Vector3(1.35f, this.transform.localScale.y, this.transform.localScale.z);
            h = 1;
        }
        else
        {
            this.transform.localScale = new Vector3(-1.35f, this.transform.localScale.y, this.transform.localScale.z);
            h = -1;
        }
    }
    void movement()
    {
        //ジャンプ処理
        if (onGround)
        {
            onGround = false;
            rb.AddForce(new Vector2(Random.Range(0.4f, 1.2f) * h, Random.Range(0.4f, 1.2f)) * jumpSpeed, ForceMode2D.Force);
        }
    }

    virtual protected void knockBack(GameObject g)
    {
        Vector3 hitPos = g.transform.position;

        // 向きの生成
        Vector3 hitForward = Vector3.Scale((hitPos - transform.position), new Vector3(1, 1, 0)).normalized;
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(hitForward.x, hitForward.y>0? hitForward.y*-1  : hitForward.y)*400*-1, ForceMode2D.Force);
    }
    virtual protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Laser")
        {
            HP--;
            knockBack(collision.gameObject);
            Destroy(collision.gameObject);
        }


    }
    virtual protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            onGround = true;
        }
    }
    virtual protected void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            onGround = false;
        }
    }
}
