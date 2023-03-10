using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public int minDamage, maxDamage;
    public Camera playerCamera;
    public float range = 10f;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    private EnemyManager enemyManager;

    public float minX, maxX, minY, maxY;
    public Transform Camera;
    Vector3 rot;

    public AudioSource gunSound;

    void Start()
    {


    }


    void Update()
    {

        if (Input.GetButtonDown("Fire1") && Time.timeScale > 0)
        {
            gunSound.Play();
            Fire();
            muzzleFlash.Play();


        }

        rot = Camera.transform.localRotation.eulerAngles;

        if (rot.x != 0f || rot.y != 1.75f)
        {
            Camera.transform.localRotation = Quaternion.Slerp(Camera.transform.localRotation, Quaternion.Euler(0f, 1.75f, 0f), Time.deltaTime * 3);
        }

    }


    void Fire()
    {
        RaycastHit hit;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            enemyManager = hit.transform.GetComponent<EnemyManager>();

            GameObject clone = (GameObject) Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal)); //Ate? etti?imiz yerde mermi izlerinin olu?mas?n? sa?lad?k.
            Destroy(clone, 0.5f);



            if (enemyManager != null) //Bo?lu?a ate? etti?imizde hata almamak i?in yaz?yoruz.
            {
                enemyManager.EnemyTakeDamage(Random.Range(minDamage, maxDamage));
            }

        }

        Recoil();

    }

    private void Recoil()
    {
        float recX = Random.Range(minX, maxX);
        float recY = Random.Range(minY, maxY);
        Camera.transform.localRotation = Quaternion.Euler(rot.x - recY, rot.y + recX, rot.z);

    }

}
