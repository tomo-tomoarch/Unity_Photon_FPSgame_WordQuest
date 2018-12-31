using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public GameObject explode;

    Vector3 force;
    float speed;
    float damage = 10f;
    // Start is called before the first frame update
    public float life_time = 1f;
    float time = 0f;

    void Start()
    {
        speed = 300f;
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        force = this.gameObject.transform.forward * speed;

        // Rigidbodyに力を加えて発射
        this.GetComponent<Rigidbody>().AddForce(force);
        time += Time.deltaTime;
        if (time > life_time)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FPSplayer"))
        {
            other.transform.GetComponent<PhotonView>().RPC("GetShot", PhotonTargets.All, damage);
            explode.GetComponent<ParticleSystem>().Play();
            Debug.Log("YOU HIT!!");


        }
    }
}