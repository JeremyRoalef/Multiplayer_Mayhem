using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Destroy on collision with anything
public class DestroyOnContact : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
