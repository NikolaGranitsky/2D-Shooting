using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParralaxEffect : MonoBehaviour
{
    private float length, startpos;
    [SerializeField]private GameObject cam;
    [SerializeField]private float parralaxEffect;
    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float temp = cam.transform.position.x * (1 - parralaxEffect);
        float dist = (cam.transform.position.x * parralaxEffect);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);
        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }
}
