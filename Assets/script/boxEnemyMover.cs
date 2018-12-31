using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class boxEnemyMover : Photon.MonoBehaviour
{

    GameController gameController;

    public delegate void Respawn(float time);
    public event Respawn RespawnMe;

    public GameObject life;

    public float health = 100f;

 
  
                
    ///FPSPlayerのtagに被弾時に呼ばれる関数

    [PunRPC]
    public void GetShot(float damage)
    {
        health -= damage;　//ライフからダメージ分の差を取る
        life.transform.localScale = new Vector3(0.01f * health, 0.24f, 0.01f * health); //頭の上のライフゲージのスケール


        if (health <= 0 && photonView.isMine)
        {
            if (RespawnMe != null)　 //<---- これは何か解りません。
                RespawnMe(3f);  //<---- これは何か解りません。

            PhotonNetwork.Destroy(gameObject); //自分のキャラをデストロイ（後にここに大爆発エフェクトを追加予定）！！
            gameController = GameObject.Find("GameController").GetComponent<GameController>();
            

        }
    }

}
