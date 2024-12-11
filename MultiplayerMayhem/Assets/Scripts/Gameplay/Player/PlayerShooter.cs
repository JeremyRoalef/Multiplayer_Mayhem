using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerShooter : NetworkBehaviour
{
    [Header("References")]

    [SerializeField]
    InputReaderSO inputReaderSO;

    [SerializeField]
    Transform projectileSpawnpoint;

    [SerializeField]
    GameObject serverProjectilePrefab;

    [SerializeField]
    GameObject clientProjectilePrefab;


    [Header("Settings")]

    [SerializeField]
    float projectileSpeed;


    bool isFiring;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) { return; }

        //Subscribe to the input
        inputReaderSO.PrimaryFireEvent += HandlePrimaryFire;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) { return; }

        //Unsubscribe form the input
        inputReaderSO.PrimaryFireEvent -= HandlePrimaryFire;
    }

    void Update()
    {
        if (!IsOwner) { return; }
        if (!isFiring) { return; }

        PrimaryFireServerRpc(projectileSpawnpoint.position, projectileSpawnpoint.up);
        SpawnProjectile(projectileSpawnpoint.position, projectileSpawnpoint.up);

    }

    //This will instantiate projectiles on YOUR screen, but not others' screens
    //To see the projectile on others' screens, need client & server Rpc (see below)
    void SpawnProjectile(Vector3 spawnPos, Vector3 dirToMove)
    {
        GameObject projectile =  Instantiate(clientProjectilePrefab, spawnPos, Quaternion.identity);
        projectile.transform.up = dirToMove;
    }

    void HandlePrimaryFire(bool isFiring)
    {
        this.isFiring = isFiring;
    }

    //This will show the projectile on the server's screen
    [ServerRpc]
    void PrimaryFireServerRpc(Vector3 spawnPos, Vector3 dirToMove)
    {
        GameObject projectile = Instantiate(serverProjectilePrefab, spawnPos, Quaternion.identity);
        projectile.transform.up = dirToMove;

        PrimaryFireClientRpc(spawnPos, dirToMove);
    }

    //This will show the projectil on the client's screen
    [ClientRpc]
    void PrimaryFireClientRpc(Vector3 spawnPos, Vector3 dirToMove)
    {
        //owner does not need to see this
        if (IsOwner) { return; }

        GameObject projectile = Instantiate(clientProjectilePrefab, spawnPos, Quaternion.identity);
        projectile.transform.up = dirToMove;
    }
}
