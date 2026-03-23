using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using log4net.Util;

[CustomEditor(typeof(MapGenerator))]

public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator mapGen = (MapGenerator)target;

        if (DrawDefaultInspector())
        {

            if (mapGen.autoUpdate)
            {
                GameObject[] genration = GameObject.FindGameObjectsWithTag("World");
                foreach (GameObject go in genration)
                {
                    Debug.Log("Deleted");
                    DestroyImmediate(go);
                }
                mapGen.generateMap();
            }
        }

        if(GUILayout.Button("Generate"))
        {
            mapGen.generateMap();
            GameObject[] genration = GameObject.FindGameObjectsWithTag("World");
            foreach (GameObject go in genration)
            {
                if(go.transform.position.y <= 0.45 * GameObject.Find("Mesh").transform.localScale.x)
                {
                    Debug.Log("Deleted");
                    DestroyImmediate(go);
                }
                
            }

        }
        if (GUILayout.Button("Delete"))
        {
            GameObject[] genration = GameObject.FindGameObjectsWithTag("World");
            foreach (GameObject go in genration)
            {

                DestroyImmediate(go);
            }
        }
    }
}
