using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MyCour());
    }

    IEnumerator MyCour()
    {
        Debug.Log("Before yield");
        yield return null;        
        Debug.Log("After yield");
    }
}
