using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    //プレイヤー
    Player player;
    //自分に使うビットのリスト
    public List<GameObject> useingBit;
    //スキルエフェクト
    [SerializeField]
    private GameObject floorEff, shieldEff;
    //スキルエフェクトを一時的保存するオブジェクト
    GameObject effObj,eff;
    //プレイヤーが現在の選んでいるスキル
    int skillNum = 0;
    //挙動制限
    int stage = 0;
    private void Start()
    {   
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        skillNum = player.skill;
        //呼び出された時のプレイヤーのスキルに応じてエフェクト生成
        if (skillNum == 3)
        {
            effObj = floorEff.gameObject ;
           }
        else if (skillNum == 4)
        {
            effObj = shieldEff;
        }
        eff = Instantiate(effObj, this.transform.position, Quaternion.identity) as GameObject;
        //そのエフェクトを子オブジェクトにする
        eff.transform.parent = this.transform;
        //存在を一時的消す
        eff.SetActive(false);
    }
    private void Update()
    {
        //生成時に自分に使うビットをリストに入る
        if (useingBit.Count == 0 && stage == 0)
        {
            foreach (GameObject i in player.bitList)
            {
                useingBit.Add(i);
            }
            stage++;
        }
        //ビット全部リストに入ったらこのステージに入る
        if (stage == 1)
        {
            //ビット全部目的地に到達したらエフェクト始動
            skiller(useingBit);
        }
        //エフェクト始動して、プレイヤーがエフェクトのスキルを選んでいる場合、マウス右クリックでビット回収
        if (Input.GetMouseButtonDown(1)&&stage==2&&player.skill==skillNum)
        {
            bitReturn(useingBit);
            Destroy(this.gameObject);
        }
        
    }
    //スキル使用
    public void skiller(List<GameObject> cloneBit)
    {
        //ビット何個目的地に到達したかチェック
        int pointCheck = 0;
        //ビットがいる場合
        if (cloneBit != null)
        {
            for (int i = 0; i < cloneBit.Count; i++)
            {   //目的地に到達したかチェック
                if (cloneBit[i].GetComponent<BitController>().onTarget)
                {
                    pointCheck++;

                }
            }
        }
        //ビットが全部到達したらエフェクト始動
        if (pointCheck == 4) {
            eff.SetActive(true);
            stage = 2;
        }
    }
    //ビット回収
    void bitReturn(List<GameObject> cloneBit)
    {
        eff.SetActive(false);
        for (int i = 0; i < cloneBit.Count; i++)
        {
            cloneBit[i].GetComponent<BitController>().afterUse = true;

        }
    }

}
