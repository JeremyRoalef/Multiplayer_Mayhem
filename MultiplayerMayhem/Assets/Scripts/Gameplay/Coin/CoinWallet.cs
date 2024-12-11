using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CoinWallet : NetworkBehaviour
{
    public NetworkVariable<int> TotalCoins = new NetworkVariable<int>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log($"Collision with {other.gameObject.name}");
        //Debug.Log($"Is object a coin? {other.gameObject.GetComponent<Coin>() != null}");

        if (other.TryGetComponent<Coin>(out Coin coin))
        {
            //Debug.Log($"Adding coin to wallet");
            int coinValue = coin.Collect();
            if (!IsServer) { return; }
            TotalCoins.Value = coinValue;
        }
    }
}
