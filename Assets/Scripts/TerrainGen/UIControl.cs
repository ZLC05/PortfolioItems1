using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    public Slider seed, scale;
    public GameObject UI;
    bool isActive = true;
    // Start is called before the first frame update
    void Start()
    {
        UI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isActive)
            {
                UI.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                isActive = false;
            }
            else
            {
                UI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                isActive = true;
            }
            
        }
        MapGenerator mapGenerator = GameObject.Find("MapGen").GetComponent<MapGenerator>();
        mapGenerator.seed = ((int)seed.value);
        mapGenerator.noiseScale = scale.value;

    }
    



    public void Quit()
    {
        Application.Quit();
    }
    public void DeleteObjects()
    {
        GameObject[] genration = GameObject.FindGameObjectsWithTag("World");
        foreach (GameObject go in genration)
        {
            Debug.Log("deleted gameobject");
            Destroy(go);
        }
    }
    public void Generate()
    {
        GameObject[] genration = GameObject.FindGameObjectsWithTag("World");
        foreach (GameObject go in genration)
        {
            Debug.Log("deleted gameobject");
            Destroy(go);
        }
        MapGenerator mapGenerator = GameObject.Find("MapGen").GetComponent<MapGenerator>();
        mapGenerator.generateMap();
    }

}
