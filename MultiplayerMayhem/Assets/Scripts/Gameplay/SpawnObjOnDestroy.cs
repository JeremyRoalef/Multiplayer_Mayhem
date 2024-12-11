using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjOnDestroy : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;

    private void OnDestroy()
    {
        Instantiate(prefab, transform.position, Quaternion.identity);
    }
}
