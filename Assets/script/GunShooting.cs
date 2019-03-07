using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShooting : MonoBehaviour {


    public ParticleSystem muzzleFlash;
    public GameObject impactPrefab;
    //追加
    public GameObject explodePrefab;
    //追加
    public ParticleSystem hitExplosion;

    GameObject[] impacts;
    GameObject explode;
    int currentImpact = 0;
    int maxImpacts = 5;
    bool shooting = false;
    public float damage = 10f;


    // Use this for initialization
    void Start()
    {

        impacts = new GameObject[maxImpacts];
        for (int i = 0; i < maxImpacts; i++)
            impacts[i] = (GameObject)Instantiate(impactPrefab);
        //追加
        explode = (GameObject)Instantiate(explodePrefab);
        //追加
    }

    // Update is called once per frame
    void Update()
    {

        //if (Input.GetButtonDown("Fire1") && !Input.GetKey(KeyCode.LeftShift))
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && !Input.GetKey(KeyCode.LeftShift))
        {

            muzzleFlash.Play();
            GetComponent<AudioSource>().Play();
            //AudioSource audio = GetComponent<AudioSource>();
            //audio.Play();
            shooting = true;
        }

    }

    void FixedUpdate()
    {
        if (shooting)
        {
            shooting = false;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.parent.forward, out hit, 100f))
            {
                //print (hit.transform.name);

                if (hit.transform.tag == "FPSplayer")
                {
                    hitExplosion.Play();
                    hit.transform.GetComponent<PhotonView>().RPC("GetShot", PhotonTargets.All, damage);

                    explode.transform.position = hit.point;
                    explode.GetComponent<ParticleSystem>().Play();
                }
                if (hit.transform.tag == "enemy")
                {
                    hitExplosion.Play();
                    hit.transform.GetComponent<PhotonView>().RPC("GetShot", PhotonTargets.All, damage);

                    explode.transform.position = hit.point;
                    explode.GetComponent<ParticleSystem>().Play();
                }
                if (hit.transform.tag == "word")
                {
                    hitExplosion.Play();
                    hit.transform.parent.GetComponent<PhotonView>().RPC("GetShot", PhotonTargets.All, damage);
                }
                    explode.transform.position = hit.point;
                    explode.GetComponent<ParticleSystem>().Play();
                    //cycle impact effects
                    impacts[currentImpact].transform.position = hit.point;
                impacts[currentImpact].GetComponent<ParticleSystem>().Play();

                if (++currentImpact >= maxImpacts)
                    currentImpact = 0;
            }
        }
    }
}
