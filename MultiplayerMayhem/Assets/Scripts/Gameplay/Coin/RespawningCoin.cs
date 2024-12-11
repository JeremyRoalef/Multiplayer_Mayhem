using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawningCoin : Coin
{
    public event Action<RespawningCoin> OnCollected;
    Vector3 previousPos;

    public override void OnNetworkSpawn()
    {
        previousPos = transform.position;
    }

    private void Update()
    {
        if (transform.position != previousPos)
        {
            Show(true);
        }
        previousPos = transform.position;
    }

    public override int Collect()
    {
        if (!IsServer)
        {
            Show(false);
            return 0;
        }

        if (alreadyCollected) {return 0;}

        alreadyCollected = true;
        OnCollected?.Invoke(this);

        return coinValue;
    }

    internal void Reset()
    {
        alreadyCollected = false;
    }
}
