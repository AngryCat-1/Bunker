using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectGenerator : Editor
{

    GameObject objs;
    GameObject NeedToSpawnObj;


    [MenuItem("Window/MeshSpawn")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ObjectGenerator));
    }

    void OnGUI()
    {
        objs = EditorGUILayout.ObjectField(objs, typeof(GameObject), true) as GameObject;
        NeedToSpawnObj = EditorGUILayout.ObjectField(NeedToSpawnObj, typeof(GameObject), true) as GameObject;

        if (GUILayout.Button("Spawn"))
        {
            Spawn();
        }


    }
    private Vector3 GetRandomPositionOnGameObjectSurface()
    {
        var chunk = objs;
       
        Vector3[] vertices = chunk.GetComponent<Mesh>().vertices;
        int[] triangles = chunk.GetComponent<Mesh>().triangles;

        if (triangles.Length == 0)
        {
            Debug.LogWarning("The chunk mesh does not have any triangles.");
            return Vector3.zero;
        }

        int randomTriangleIndex = UnityEngine.Random.Range(0, triangles.Length / 3);
        int triangleIndex = randomTriangleIndex * 3;

        if (triangleIndex + 2 >= vertices.Length)
        {
            Debug.LogWarning("Invalid triangle index.");
            return Vector3.zero;
        }

        Vector3 v0 = vertices[triangles[triangleIndex]];
        Vector3 v1 = vertices[triangles[triangleIndex + 1]];
        Vector3 v2 = vertices[triangles[triangleIndex + 2]];

        float u = UnityEngine.Random.Range(0f, 1f);
        float v = UnityEngine.Random.Range(0f, 1f);
        if (u + v > 1f)
        {
            u = 1f - u;
            v = 1f - v;
        }

        Vector3 randomPointOnTriangle = v0 + u * (v1 - v0) + v * (v2 - v0);

        Vector3 worldPosition = chunk.transform.TransformPoint(randomPointOnTriangle);



        return worldPosition;



    }

    void Spawn()
    {
        Instantiate(NeedToSpawnObj, GetRandomPositionOnGameObjectSurface(), Quaternion.identity);
    }


   
}
