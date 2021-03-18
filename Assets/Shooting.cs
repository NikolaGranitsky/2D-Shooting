using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private GameObject bulletPrefab;
    [SerializeField]public Transform FirePoint;
    void Start()
    {
        
    }

    public void Shoot()
    {
        Instantiate(bulletPrefab, FirePoint.position, FirePoint.rotation);;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
