using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletTime : MonoBehaviour
{
    [Header("Time Reduction")]
    public float timeReduce;
    private float normalTime;
    [SerializeField] float timeAllowed;
    public float maxTime;
    public float regenTime;
    public bool btActive;
    bool canBulletTime;
    public float regenDelay;
    private float regenRefresh;
    private float normCamX, normCamY;
    PlayerMovement pm;
    PlayerCam cam;
    [SerializeField] Image bulletTimeUI;

    [Header("Keybind")]
    public KeyCode bulletTime = KeyCode.Mouse1;
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerCam>();
        pm = GetComponent<PlayerMovement>();
        regenRefresh = regenDelay;
        normalTime = Time.timeScale;
        timeAllowed = maxTime;
        normCamX = cam.sensX;
        normCamY = cam.sensY;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Mouse1) && canBulletTime == true && pm.grounded == false)
        {
            bulletTimeUI.color = new Color(0.25f, 0.25f, 0.25f, 0.75f);
            cam.sensX = 800;
            cam.sensY = 800;
            if(timeAllowed > 0)
            {

                regenDelay = regenRefresh;
                Debug.Log("Activate Bullet Time");
                timeAllowed -= Time.deltaTime;
                Time.timeScale = timeReduce;
                btActive = true;
            }
            else if(timeAllowed <= 0)
            {
                canBulletTime = false;
            }

        }
        else
        {
            bulletTimeUI.color = new Color(0.25f, 0.25f, 0.25f, 0f);
            cam.sensX = normCamX; cam.sensY = normCamY;
            btActive = false;
            Time.timeScale = normalTime;
            regenDelay -= Time.deltaTime;
            if(regenDelay <= 0)
            {
                timeAllowed = timeAllowed + timeReduce * Time.deltaTime;
            }

            if (timeAllowed > regenTime)
            {
                
                timeAllowed = maxTime;
                canBulletTime = true;
            }
        }
    }
}
