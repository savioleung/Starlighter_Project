using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BitController : MonoBehaviour
{
    //プレイヤー
    Player player;
    //バックパック
    public GameObject bitBag;
    //撃つビーム
    public GameObject Beam;
    //ビットの移動速度、曲がりをつける時間、ビームの速度、回転速度
    float bitSpeed = 5.0f, upMoveTime, laserSpeed = 50.0f, rotPow;
    //目的地に到達したかチェック、撃てるかチェック、撃ったかチェック
    public bool onTarget, canShoot, afterUse;
    //目的地
    public Vector2 vec;
    private void Start()
    {
        //プレイヤーを探す
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        //バックパックを探す       
        bitBag = GameObject.Find("bitBag");
        //プレイヤーがスキル「ビット」を選んでいる場合
        if (player.skill == 2)
        {   //目的地をクリックした座標に、ビームが撃てるように
            vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            this.canShoot = true;
        }
        else
        {//「ビット」以外の場合、打たない
            this.canShoot = false;
            
        }
        //初期化初期化
        this.afterUse = false;
        this.onTarget = false;
        upMoveTime = 2.0f;
        rotPow = 5.0f;

    }
    void Update()
    {
        //ビットを常に回転する
        rotateBit();
        //目的地チェック
        if(this.transform.position.x==vec.x&& this.transform.position.y == vec.y)
        {
            onTarget = true;
        }
        //プレイヤーがビットを選んでいる場合にマウスクリックで射撃
        if (player.skill == 2)
        {
            ShootBeam();
        }
        //使い終わったらバックパックに戻る
        if (afterUse)
        {
            vec = bitBag.transform.position;

        }
        //移動
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(vec.x, vec.y), bitSpeed * Time.deltaTime);

    }
    //射出したビットを回転する動きをつける
     void rotateBit()
    {
        if (upMoveTime > 1)
        {
            upMoveTime *= 0.98f;
            transform.Translate(0, 0.1f, 0);
        }
        transform.Rotate(0, 0, rotPow);
    }
    //射撃処理
    void ShootBeam()
    {
        //マウスクリック
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            GameObject laser = Instantiate(Beam, transform.position, Quaternion.identity);
            // クリックした座標の取得（スクリーン座標からワールド座標に変換）
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 向きの生成（Z成分の除去と正規化）
            Vector3 shotForward = Vector3.Scale((mouseWorldPos - transform.position), new Vector3(1, 1, 0)).normalized;

            // 弾に速度を与える
            laser.GetComponent<Rigidbody2D>().velocity = shotForward * laserSpeed;

            Destroy(laser, 2);
        }//撃ったら戻る処理
        if (Input.GetMouseButtonUp(0) && canShoot)
        {
            canShoot = false;
            upMoveTime = 2.0f;
            afterUse = true;
        }

    }

    void bitControl()
    {
        Vector3 vis = Vector3.zero;
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        //戻っていく時、バックパックに触れて初めてチャージする
        if (collision.gameObject == bitBag&&afterUse)
        {
            //3秒チャージして、使用可能になる
            player.invokeFunc("chargeBit", 3);
            Destroy(gameObject);
        }
    }
}
