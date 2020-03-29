using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMatrix : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        Vector3 p = transform.position;
        Debug.LogError("世界空间到摄像机空间或者叫观察空间，右手坐标系，z轴刚好相反");
        Debug.Log(Camera.main.worldToCameraMatrix);
        Debug.Log(Camera.main.worldToCameraMatrix * new Vector4(p.x, p.y, p.z, 1));
        Debug.Log(Camera.main.transform.InverseTransformPoint(p));

        Debug.LogError("");
        Debug.Log(Camera.main.projectionMatrix);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
