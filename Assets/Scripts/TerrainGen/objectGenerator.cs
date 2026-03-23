using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectGenerator : MonoBehaviour
{
    public GameObject tree, bush, rock;
    public Vector3 spawnPos;

    public void GenerateTree()
    {
        GameObject go = Instantiate(tree, spawnPos, Quaternion.identity);
        go.transform.localScale = new Vector3(GameObject.Find("Mesh").transform.localScale.x, GameObject.Find("Mesh").transform.localScale.y, GameObject.Find("Mesh").transform.localScale.z);
    }
    public void GenerateRock()
    {
        GameObject go = Instantiate(rock, spawnPos, Quaternion.identity);
        go.transform.localScale = new Vector3(GameObject.Find("Mesh").transform.localScale.x, GameObject.Find("Mesh").transform.localScale.y, GameObject.Find("Mesh").transform.localScale.z);

    }
    public void GenerateBush()
    {
        GameObject go = Instantiate(bush, spawnPos, Quaternion.identity);
        go.transform.localScale = new Vector3(GameObject.Find("Mesh").transform.localScale.x, GameObject.Find("Mesh").transform.localScale.y, GameObject.Find("Mesh").transform.localScale.z);

    }
}
