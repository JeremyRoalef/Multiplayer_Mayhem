using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifetime : MonoBehaviour
{
    [SerializeField]
    float fltLifeTime = 1;

    void Start()
    {
        Invoke("DestroyObj", fltLifeTime);
    }
    void DestroyObj()
    {
        Destroy(gameObject);
    }
}
