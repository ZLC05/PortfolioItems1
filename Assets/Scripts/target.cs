using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class target : MonoBehaviour
{
    MeshRenderer mr;
    Collider col;
    bool disabled = false;


    private void Start()
    {
        disabled = false;
        mr = GetComponent<MeshRenderer>();
        col = GetComponent<Collider>();
    }
    IEnumerator respawning()
    {
        mr.enabled = false;
        col.enabled = false;
        yield return new WaitForSeconds(6);
        mr.enabled = true;
        col.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit");
        StartCoroutine(respawning());
    }
}
