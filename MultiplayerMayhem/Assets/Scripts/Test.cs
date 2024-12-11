using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] InputReaderSO inputReaderSO;

    // Start is called before the first frame update
    void Start()
    {
        inputReaderSO.MoveEvent += HandleMove;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        inputReaderSO.MoveEvent -= HandleMove;
    }

    void HandleMove(Vector2 movement)
    {
        Debug.Log(movement);
    }
}
