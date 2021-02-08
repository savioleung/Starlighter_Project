using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public List<GameObject> bitList;
    public Text bitText,maxBitText;
    //銃オブジェクト
    [SerializeField]
    private GameObject gunBit, handGunBit, camSpot, steper, floorEff, shieldEff;
    [SerializeField]
    private GameObject[] multiBit = new GameObject[4];
    public int skill = 1;

    public GameObject Beamlaser;
    float Radius = 0.5f, gunLaserSpeed = 30.0f;
    Vector3 MousePos1, ScreenMouse, MouseOffset;
    //rigidbody
    Rigidbody2D rb;

    //ビットの使用可能数、最大数
    int bitMaxCount = 20, bitCount;
    //ビットのスポーン位置
    public Transform bitSpwan;

    GameObject[] cloneBit2 = new GameObject[8];
    //run speed
    float runSpeed = 8.0f, flySpeed = 10.0f, jumpSpeed = 400.0f;
    Vector2 force = new Vector2(0, 0);

    //地面チェック
    bool onGround = false,canDash=false,isFlying=false;
    //死と勝ちの判定
    public bool isDeath = false, isWin = false;

    [SerializeField]
    private Image[] skillSlot;
    // Start is called before the first frame update
    void Start()
    {
        bitCount = bitMaxCount;
        rb = GetComponent<Rigidbody2D>();
        steper.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //ハンドガン処理
        handGun();
        //移動処理
        movement();
        #region スロット変更
        var key = Input.inputString;
        switch (key)
        {
            case "1":
                skill = 1;
                break;
            case "2":
                skill = 2;
                break;
            case "3":
                skill = 3;

                for (int i = 0; i < 4; i++)
                {
                    multiBit[i].transform.localPosition = new Vector3(-0.9f + 0.6f * i, -0.1f, 0);
                }
                break;
            case "4":
                multiBit[0].transform.localPosition = new Vector3(-0.6f, 1f);
                multiBit[1].transform.localPosition = new Vector3(-0.6f, -1f);
                multiBit[2].transform.localPosition = new Vector3(0.6f, -1f);
                multiBit[3].transform.localPosition = new Vector3(0.6f, 1f);
                skill = 4;
                break;
            default:
                break;
        }
        skillSlotSelect(skill);
        #endregion

        //銃
        if (Input.GetMouseButtonDown(0) && skill == 1)
        {
            GameObject laser = Instantiate(Beamlaser, handGunBit.transform.position, Quaternion.identity);
            // クリックした座標の取得（スクリーン座標からワールド座標に変換）
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 向きの生成（Z成分の除去と正規化）
            Vector3 shotForward = Vector3.Scale((mouseWorldPos - transform.position), new Vector3(1, 1, 0)).normalized;

            // 弾に速度を与える
            laser.GetComponent<Rigidbody2D>().velocity = shotForward * gunLaserSpeed;

            Destroy(laser, 1);
        }
        //ビット発射
        if (Input.GetMouseButtonDown(1) && bitCount > 0 && skill == 2)
        {
            GameObject cloneBit = Instantiate(gunBit, bitSpwan.position, Quaternion.identity);
            bitCount--;
        }
        //足場/シールド
        if (skill == 3 || skill == 4)
        {
            skill34(skill);
        }
        else { steper.SetActive(false); }

        bitText.text = bitCount+"";
        maxBitText.text=bitMaxCount+"";
    }
    //移動処理
    void movement()
    {
        float x = Input.GetAxis("Horizontal");
        force.x = x * runSpeed;
        rb.AddForce(force, ForceMode2D.Force);
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)) )
        {
            //ジャンプ処理
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (onGround)
                {
                    onGround = false;
                    rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Force);
                }
                else
                {
                    if (!isFlying)
                    {
                        rb.velocity = new Vector2(0, 1);
                    }
                    isFlying = true;
                }
            }
            if (isFlying)
            {
                rb.AddForce(Vector2.up * flySpeed, ForceMode2D.Force);
                flySpeed *= 0.99f;
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    isFlying = false;
                }
            }
        }
        //ダッシュ
        if (Input.GetKey(KeyCode.LeftShift)&&canDash)
        {
            canDash = false;
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(Input.GetKey(KeyCode.D)?1:-1, 0.8f) * jumpSpeed, ForceMode2D.Force);
        }

        
//バックパックの位置をマウスの位置によって左右移動によって方向転換を表現
if (ScreenMouse.x > transform.position.x)
        {
            transform.localScale = new Vector3(1, 1.6f, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1.6f, 1);
        }

    }
    //ノックバック処理
    void knockBack(GameObject g)
    {
        Vector3 hitPos = g.transform.position;

        // 向きの生成
        Vector3 hitForward = Vector3.Scale((hitPos - transform.position), new Vector3(1, 1, 0)).normalized;
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(hitForward.x, hitForward.y > 0 ? hitForward.y * -1 : hitForward.y) * 200 * -1, ForceMode2D.Force);
    }
    //ハンドガン処理
    void handGun()
    {
        MousePos1 = Input.mousePosition;
        ScreenMouse = Camera.main.ScreenToWorldPoint(new Vector3(MousePos1.x, MousePos1.y, handGunBit.transform.position.z - Camera.main.transform.position.z));
        MouseOffset = ScreenMouse - transform.position;

        var norm = MouseOffset.normalized;
        handGunBit.transform.position = new Vector3(norm.x * Radius + transform.position.x, norm.y * Radius + transform.position.y, handGunBit.transform.position.z);
  }

    /// <summary>
    /// 他のプログラムでこのプログラムの関数を何秒後にを実行したい場合、この関数を経由する
    /// </summary>
    /// <param name="func">実行したい関数</param>
    /// <param name="s">何秒後に実行</param>
    public void invokeFunc(string func, int s)
    {
        Invoke(func, s);
    }
    //ビットをチャージする、
    private void chargeBit()
    {
        Debug.Log("bit charging");
        bitCount++;
    }
    //スキル3と４同一関数
    private void skill34(int s)
    {
        Vector2 aimSpot = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        steper.SetActive(true);
        steper.transform.position = aimSpot;

        if (Input.GetMouseButtonDown(0) && bitCount >= 4)
        {
            //ビットー4
            bitCount -= 4;
            //ビットをすべてリストから排除する
            bitList.Clear();
            for (int i = 0; i < 4; i++)
            {
                cloneBit2[i] = Instantiate(gunBit, bitSpwan.position, Quaternion.identity);
                cloneBit2[i].GetComponent<BitController>().vec = multiBit[i].transform.position;
                cloneBit2[i].GetComponent<BitController>().canShoot = false;
                //一段ビットをリストに入れる
                bitList.Add(cloneBit2[i]);
            }
            //狙い先の場所でオブジェを生成
            GameObject targetObj = Instantiate(floorEff, aimSpot, Quaternion.identity) as GameObject;

        }
    }

    //スキル選択の色変更
    public void skillSlotSelect(int skill)
    {
        for (int i = 0; i < skillSlot.Length; i++)
        {
            skillSlot[i].color = new Color(0.15f, 0.15f, 0.15f);
        }
        skillSlot[skill - 1].color = new Color(0, 1.0f, 0);
    }
    //ダメージ処理
    public void getDamage()
    {
        //ビットが自機にある場合、ビットを消費
        if (bitCount > 0 && bitMaxCount > 0)
        {
            bitMaxCount--;
            bitCount--;
        }
        else
        {
            isDeath = true;
            Debug.Log("death");
        }
    }

    //地面チェック
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "step")
        {
            onGround = true;
            canDash = true;
            isFlying = false;
            flySpeed = 10.0f;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "step")
        {
            onGround = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyLaser")
        {
            knockBack(collision.gameObject);
            Destroy(collision.gameObject);
            getDamage();

        }
        if (collision.gameObject.tag == "Goal")
        {
            isWin = true;
        }
    }

}
