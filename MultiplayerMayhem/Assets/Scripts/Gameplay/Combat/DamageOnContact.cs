using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    [SerializeField]
    int damage = 15;


    //ulong is a positive big integer used to store client IDs.
    ulong ownerClientId;

    public void SetOwner(ulong clientId)
    {
        this.ownerClientId = clientId;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody == null) { return; }
        if (other.attachedRigidbody.TryGetComponent<NetworkObject>(out NetworkObject netObj))
        {
            //If this is the owner of the projectile, return.
            if (ownerClientId == netObj.OwnerClientId) //Network object has a OwnerClientId property
            {
                return;
            }
        }

        if (other.gameObject.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage(damage);
        }
    }
}
