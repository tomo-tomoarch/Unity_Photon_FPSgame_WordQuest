using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemyMover : Photon.MonoBehaviour
{

    GameController gameController;

    public delegate void Respawn(float time);
    public event Respawn RespawnMe;

    public GameObject life;
    public GameObject[] items;
    public GameObject[] words;
   

    Vector3 position;
    Quaternion rotation;
    float smoothing = 10f;
    public float health = 100f;

    private float speed = 10f;
    private float rotationSmooth = 1f;
    public Text enemyText;

    private Vector3 targetPosition;

    private float changeTargetSqrDistance = 30f;

    private Transform player;

    public GameObject bulletPrefab;
    public Transform muzzle;
    private float attackInterval = 0.2f;
    private float lastAttackTime;
    Vector3 force;

    void Start()
    {
        //倒した敵の数表示
        enemyText = GameObject.Find("enemyText").GetComponent<Text>();
      
            gameController = GameObject.Find("GameController").GetComponent<GameController>();
            enemyText.text = gameController.enemyCount.ToString() + "enemy killed";

        //文字の挿入
        int wordNumber = Random.Range(2, 6);

        for (int i = 0; i < wordNumber; i++)
        {
            int dice = Random.Range(0, 5);

            if(dice == 0)
            {
                //文字の挿入
                var wordPosition = PhotonNetwork.Instantiate("WordA", new Vector3(transform.position.x + 3 * i, transform.position.y + 2f, transform.position.z), Quaternion.identity, 0);
                wordPosition.transform.parent = gameObject.transform;
            }else if (dice == 1)
            {
                //文字の挿入
                var wordPosition = PhotonNetwork.Instantiate("WordI", new Vector3(transform.position.x + 3 * i, transform.position.y + 2f, transform.position.z), Quaternion.identity, 0);
                wordPosition.transform.parent = gameObject.transform;
            }
            else if (dice == 2)
            {
                //文字の挿入
                var wordPosition = PhotonNetwork.Instantiate("WordU", new Vector3(transform.position.x + 3 * i, transform.position.y + 2f, transform.position.z), Quaternion.identity, 0);
                wordPosition.transform.parent = gameObject.transform;
            }
            else if (dice == 3)
            {
                //文字の挿入
                var wordPosition = PhotonNetwork.Instantiate("WordE", new Vector3(transform.position.x + 3 * i, transform.position.y + 2f, transform.position.z), Quaternion.identity, 0);
                wordPosition.transform.parent = gameObject.transform;
            }
            else if (dice == 4)
            {
                //文字の挿入
                var wordPosition = PhotonNetwork.Instantiate("WordO", new Vector3(transform.position.x + 3 * i, transform.position.y + 2f, transform.position.z), Quaternion.identity, 0);
                wordPosition.transform.parent = gameObject.transform;
            }
        }

        


        if (photonView.isMine)
        {
            targetPosition = new Vector3(Random.Range(0, 200), 2, Random.Range(0, 200));
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

    private void Update()
    {
       
        
          /*  player = GameObject.FindWithTag("FPSplayer").transform;

            if (Vector3.SqrMagnitude(player.position - transform.position) <= 50f)
            {
                player = GameObject.FindWithTag("FPSplayer").transform;
                // プレイヤーの方向を向く
                Quaternion targetplayerRotation = Quaternion.LookRotation(player.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetplayerRotation, Time.deltaTime * rotationSmooth);

                if (Time.time > lastAttackTime + attackInterval)
                {
                    Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
                    lastAttackTime = Time.time;
                }
                // 前方に進む
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
            else
            {*/
                // 目標地点との距離が小さければ、次のランダムな目標地点を設定する
                float sqrDistanceToTarget = Vector3.SqrMagnitude(transform.position - targetPosition);
                if (sqrDistanceToTarget < changeTargetSqrDistance)
                {
                    targetPosition = new Vector3(Random.Range(0, 200), 2, Random.Range(0, 200));
                }

                // 目標地点の方向を向く
                Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmooth);

                // 前方に進む
                transform.Translate(Vector3.forward * speed * Time.deltaTime);

                if (Time.time > lastAttackTime + attackInterval)
                {
                    Instantiate(bulletPrefab, muzzle.position, Random.rotation);

                    force = this.gameObject.transform.forward * speed;

                    // Rigidbodyに力を加えて発射
                    bulletPrefab.GetComponent<Rigidbody>().AddForce(force);
                    lastAttackTime = Time.time;
                    GetComponent<AudioSource>().Play();
                }
           // }
       
        
           
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "wall")
        {
            targetPosition = new Vector3(100,1,100);
            Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmooth);

            // 前方に進む
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

  




    ///FPSPlayerのtagに被弾時に呼ばれる関数

    [PunRPC]
    public void GetShot(float damage)
    {
        health -= damage;　//ライフからダメージ分の差を取る
        life.transform.localScale = new Vector3(0.01f * health, 0.24f, 0.01f * health); //頭の上のライフゲージのスケール


        ///FPSPlayerのtagに被弾時に色が変わるスクリプト(上手く動いていない)
        if (photonView.isMine)
        {
            GameObject.Find("Capsule").GetComponent<Renderer>().material.color = new Color(255 - health * 2, 255, 255);
        }

        if (health <= 0 && photonView.isMine)
        {
            if (RespawnMe != null)　 //<---- これは何か解りません。
                RespawnMe(3f);  //<---- これは何か解りません。
            Vector3 currentposition = this.transform.position;
            Quaternion currentquanternion = this.transform.rotation;
            PhotonNetwork.Destroy(gameObject); //自分のキャラをデストロイ（後にここに大爆発エフェクトを追加予定）！！
            gameController = GameObject.Find("GameController").GetComponent<GameController>();
            gameController.StartSpawnProcess(3f);
            gameController.enemyCount += 1;
            enemyText.text = gameController.enemyCount.ToString() + "enemy killed";
            int index = Random.Range(0, items.Length);
            if(index == 0)
            {
                PhotonNetwork.Instantiate("ItemLife",
                currentposition,
                currentquanternion,
                0);
            }else if(index == 1){
                PhotonNetwork.Instantiate("ItemPower",
                currentposition,
                currentquanternion,
                0);
            }
            else if (index == 2)
            {
                PhotonNetwork.Instantiate("ItemFast",
                currentposition,
                currentquanternion,
                0);
            }

        }
    }

}
