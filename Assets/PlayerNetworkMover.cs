using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnitySampleAssets.Characters.FirstPerson;

public class PlayerNetworkMover : Photon.MonoBehaviour
{
    [SerializeField] Camera sceneCamera;
    [SerializeField] Text winText;
    [SerializeField] Text healthText;

    public FirstPersonController firstPersonController;

    public delegate void Respawn(float time);
    public event Respawn RespawnMe;

    public GameObject button;
    public GameObject life;

    Vector3 position;
    Quaternion rotation;
    float smoothing = 10f;
    public float health = 100f;

    void Start()
    {

        if (photonView.isMine)
        {
            //rigidbody.useGravity = true;
            ///自分のキャラ以外はコンポーネントをオフのままにします。
            GetComponent<FirstPersonController>().enabled = true;
            GetComponent<CharacterController>().enabled = true;
            GetComponent<AudioSource>().enabled = true;
            GetComponentInChildren<GunShooting>().enabled = true;
            //foreach (SimpleMouseRotator rot in GetComponentsInChildren<SimpleMouseRotator>())
            //    rot.enabled = true;
            foreach (Camera cam in GetComponentsInChildren<Camera>())
                cam.enabled = true;


            ///勝った時のテキストはデフォルトでは表示しません。
            //winText = GetComponent<Text>(); // <---- これは結局なぜか使えませんでした。
            winText = GameObject.Find("winText").GetComponent<Text>();
            if (photonView.isMine)
            {
                winText.text = " ";
            }


            ///自分のhealth（ライフ）のテキストはデフォルトでは100＋pointと出るようにします。
            //healthText = GetComponent<Text>(); // <---- これは結局なぜか使えませんでした。
            healthText = GameObject.Find("healthText").GetComponent<Text>();
            if (photonView.isMine)
            {
                healthText.text = health.ToString() + "points";
            }

            // winText.text = " "; <---- これは結局なぜか使えませんでした。
            // healthText.text = health + "point"; <---- これは結局なぜか使えませんでした。
            //transform.Find("Head Joint/First Person Camera/GunCamera/Candy-Cane").gameObject.layer = 11; //<---- これは何か解りません。
        }

        else
        {
            StartCoroutine("UpdateData");//コルーチンを始めます。
        }

    }

    ///場所の同期を取った時に間を補完する処理（スムーズに動く）。
    IEnumerator UpdateData()
    {
        while (true)
        {
            transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * smoothing);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * smoothing);
            yield return null;
        }
    }


    ///場所の同期を取る処理。
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(health);
        }
        else
        {
            position = (Vector3)stream.ReceiveNext();
            rotation = (Quaternion)stream.ReceiveNext();
            health = (float)stream.ReceiveNext();
        }
    }

    ///FPSPlayerのtagに被弾時に呼ばれる関数

    [PunRPC]
    public void GetShot(float damage)
    {
        health -= damage;　//ライフからダメージ分の差を取る
        StartCoroutine("startPush");　//コルーチン開始
        life.transform.localScale = new Vector3(0.01f * health, 0.24f, 0.01f * health); //頭の上のライフゲージのスケール


        ///FPSPlayerのtagに被弾時に色が変わるスクリプト(上手く動いていない)
        if (photonView.isMine)
        {
            GameObject.Find("Capsule").GetComponent<Renderer>().material.color = new Color(255 - health * 25.5f, 255, 255);
        }

        //if 文はどこ？　何故動いてる？
        {
            healthText.text = health.ToString() + "points"; //自分のhealth（ライフ）のテキストpointと出るようにします。public TextにTEXTをD&D必須。
        }

        //this.healthText = this.GetComponent<Text>(); // <---- これは結局なぜか使えませんでした。
        //healthText.text = health + "point";// <---- これは結局なぜか使えませんでした。

        if (health <= 0 && photonView.isMine)
        {
            if (RespawnMe != null)　 //<---- これは何か解りません。
                RespawnMe(3f);  //<---- これは何か解りません。

            GameObject.Find("Canvas").transform.Find("Panel").transform.Find("Button").gameObject.SetActive(true);//親オブジェクトからSetActiveでチェックボックスオン。
            //button.SetActive(true);　//<---- こういう風に書いてもダメでした。
            sceneCamera.enabled = true;　//シーンカメラオン。

            if (photonView.isMine)
            {
                GameObject.Find("Panel").GetComponent<UnityEngine.UI.Image>().enabled = true;//赤パネルオン。
            }

            if (photonView.isMine)
            {
                winText.text = "YOU LOSE";//負けテキスト表示。
            }

            PhotonNetwork.Destroy(gameObject);　//自分のキャラをデストロイ（後にここに大爆発エフェクトを追加予定）！！
        }
    }


    /// 撃たれたときの赤い画面のエフェクト
    IEnumerator startPush()
    {
        if (photonView.isMine)
        {
            GameObject.Find("Panel").GetComponent<UnityEngine.UI.Image>().enabled = true;//panelオン
            yield return new WaitForSeconds(0.5f);　//五秒待つ
            GameObject.Find("Panel").GetComponent<UnityEngine.UI.Image>().enabled = false;//panelオフ
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ItemFast"))
        {
            PhotonNetwork.Destroy(other.gameObject); //ぶつかった相手をディアクティベート（消える
            if (photonView.isMine)
            {
                FirstPersonController firstPersonController = GetComponent<FirstPersonController>();
                firstPersonController.walkSpeed += 2f;
            }

        }
        else if (other.gameObject.CompareTag("ItemLife"))
        {
            PhotonNetwork.Destroy(other.gameObject); //ぶつかった相手をディアクティベート（消える
            health += 30f;
            healthText.text = health.ToString() + "points";

        } else if (other.gameObject.CompareTag("ItemPower"))
        {
            PhotonNetwork.Destroy(other.gameObject); //ぶつかった相手をディアクティベート（消える

            if (photonView.isMine)
            {
                GunShooting gunShooting = GameObject.Find("Cylinder").GetComponent<GunShooting>();
                gunShooting.damage += 10f;
            }
         }
    }
}
