using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private GameObject bulletPrefab;
    [SerializeField] private GameObject burstBulletPrefab;
    [SerializeField]public Transform FirePoint;
    int burst = 3;
    int count = 0;


    void Start()
    {
        
    }

    public void Shoot()
    {
        Instantiate(bulletPrefab, FirePoint.position, FirePoint.rotation);
    }

    public void BurstShoot()
    {
        count = 0;
        Shot();
    }


    void Shot()
    {
        if (count < burst)
        {
            Instantiate(burstBulletPrefab, FirePoint.position, FirePoint.rotation);
            Invoke("Shot", 0.1f);
            count++;
        }
    }




    // Update is called once per frame
    void Update()
    {

    }
}
