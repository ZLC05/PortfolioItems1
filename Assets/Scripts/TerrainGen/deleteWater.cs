using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteWater : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y <= 0.45 * GameObject.Find("Mesh").transform.localScale.x)
        {
            Debug.Log("Got Deleted");
            Destroy(gameObject);
        }
    }
}
