using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sortinglayertest : MonoBehaviour {

    //GameObject[] g;
    SpriteRenderer[] s;
    float testditance;//太卡了就改小一点
    Vector3 a;
    Vector3 b;
    //int i;
    // Use this for initialization
    void Start () {
        //s=GetComponent<SpriteRenderer>();

       s= GameObject.FindObjectsOfType<SpriteRenderer>();
        foreach (var i in s)
        {
            foreach (var j in s)
            {
                if (i == j)
                    continue;

                if (SortingLayer.GetLayerValueFromID(i.sortingLayerID) == SortingLayer.GetLayerValueFromID(j.sortingLayerID))

                {
                    if (i.sortingOrder == j.sortingOrder)
                    {
                        a = i.transform.position;
                        b = j.transform.position;

                        if (a.z == b.z)
                        {
                            if (Vector3.Distance(a, b) < 10)
                            { 
                            testditance = Mathf.Sqrt((i.size.x * 0.5f) * (i.size.x * 0.5f) + (i.size.y * 0.5f) * (i.size.y * 0.5f))
                               + Mathf.Sqrt((j.size.x * 0.5f) * (j.size.x * 0.5f) + (j.size.y * 0.5f) * (j.size.y * 0.5f));
                            if (Vector3.Distance(a, b) < testditance)
                                Debug.LogError(i.gameObject.name + "," + j.gameObject.name);
                            }
                        }
                    }

                }








            }




        }





	}
	
	// Update is called once per frame
	//void Update () {
	//s.sortingOrder
 //         i= SortingLayer.GetLayerValueFromID(s.sortingLayerID);
 //       Debug.Log(i);
	//}
}
