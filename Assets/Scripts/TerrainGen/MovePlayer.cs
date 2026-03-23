using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovePlayer : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed;
    Rigidbody rb;



    public bool sliding;

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        MoverPlayer();
    }


    private void MoverPlayer()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            rb.AddForce(transform.forward * speed);
        }
        else
        {
            rb.AddForce(transform.forward * 0);
        }

    }
}
