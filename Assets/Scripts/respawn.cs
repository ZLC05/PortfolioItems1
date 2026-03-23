using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class respawn : MonoBehaviour
{
    public Vector3 spawnPos;
    // Start is called before the first frame update
    void Start()
    {
        spawnPos = GameObject.Find("Main Camera").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //x
        if(transform.position.x >= 500f || transform.position.x <= -500f)
        {
            transform.position = spawnPos;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        //z
        if (transform.position.z >= 500f || transform.position.z <= -500f)
        {
            transform.position = spawnPos;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        //y
        if (transform.position.y >= 250f || transform.position.y <= -250f)
        {
            transform.position = spawnPos;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

}
