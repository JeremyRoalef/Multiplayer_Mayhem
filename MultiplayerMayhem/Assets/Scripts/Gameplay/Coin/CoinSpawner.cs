using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CoinSpawner : NetworkBehaviour
{
    [SerializeField]
    RespawningCoin coinPrefab;

    [SerializeField]
    int maxCoins = 50;

    [SerializeField]
    int coinValue = 1;

    [SerializeField]
    Vector2 xSpawnRange;

    [SerializeField]
    Vector2 ySpawnRange;

    //The layers to check in the engine
    [SerializeField]
    LayerMask layerMask;

    Collider2D[] coinBuffer = new Collider2D[1];

    float coinRadius;
    public override void OnNetworkSpawn()
    {
        if (!IsServer) {return; }
        coinRadius = coinPrefab.GetComponent<CircleCollider2D>().radius;

        for (int i = 0; i < maxCoins; i++)
        {
            SpawnCoin();
        }
    }

    void SpawnCoin()
    {
        RespawningCoin coinInstance = Instantiate(coinPrefab, GetSpawnPos(), Quaternion.identity);
        coinInstance.SetValue(coinValue);
        coinInstance.GetComponent<NetworkObject>().Spawn();

        coinInstance.OnCollected += HandleCoinCollected;
    }

    private void HandleCoinCollected(RespawningCoin coin)
    {
        coin.transform.position = GetSpawnPos();
        coin.Reset();
    }

    Vector2 GetSpawnPos()
    {
        float x = 0;
        float y = 0;

        while (true)
        {
            x = Random.Range(xSpawnRange.x, xSpawnRange.y);
            y = Random.Range(ySpawnRange.x, ySpawnRange.y);

            Vector2 spawnPos = new Vector2(x, y);
            //NonAlloc will not allocate memory
            int numColliders = Physics2D.OverlapCircleNonAlloc(spawnPos, coinRadius, coinBuffer, layerMask);
            if (numColliders == 0)
            {
                return spawnPos;
            }
        }
    }
}
