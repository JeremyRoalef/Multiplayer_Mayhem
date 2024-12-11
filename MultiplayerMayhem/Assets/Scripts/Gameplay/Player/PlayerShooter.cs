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

    [SerializeField]
    GameObject muzzleFlash;

    [SerializeField]
    Collider2D playerCollider;


    [Header("Settings")]

    [SerializeField]
    float projectileSpeed;

    [SerializeField]
    float fireRate = 1f;

    [SerializeField]
    float muzzleFlashDuration = 0.5f;

    bool isFiring;
    float previousFireTime;
    float muzzleFlashTimer;

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
        if (muzzleFlashTimer > 0)
        {
            muzzleFlashTimer -= Time.deltaTime;
        }
        if (muzzleFlashTimer <= 0)
        {
            muzzleFlash.SetActive(false);
        }



        if (!IsOwner) { return; }
        if (!isFiring) { return; }
        if (Time.time < (1 / fireRate) + previousFireTime) { return; }

        PrimaryFireServerRpc(projectileSpawnpoint.position, projectileSpawnpoint.up);
        SpawnProjectile(projectileSpawnpoint.position, projectileSpawnpoint.up);

        previousFireTime = Time.time;
    }

    //This will instantiate projectiles on YOUR screen, but not others' screens
    //To see the projectile on others' screens, need client & server Rpc (see below)
    void SpawnProjectile(Vector3 spawnPos, Vector3 dirToMove)
    {
        muzzleFlash.SetActive(true);
        muzzleFlashTimer = muzzleFlashDuration;

        GameObject projectile =  Instantiate(clientProjectilePrefab, spawnPos, Quaternion.identity);
        projectile.transform.up = dirToMove;


        Physics2D.IgnoreCollision(playerCollider, projectile.GetComponent<Collider2D>());

        if (projectile.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = rb.transform.up * projectileSpeed;
        }
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

        Physics2D.IgnoreCollision(playerCollider, projectile.GetComponent<Collider2D>());

        //Set the owner id of the projectile to this game object's id
        if (projectile.TryGetComponent<DamageOnContact>(out DamageOnContact dealDamage))
        {
            dealDamage.SetOwner(OwnerClientId);
        }

        if (projectile.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = rb.transform.up * projectileSpeed;
        }
    }

    //This will show the projectil on the client's screen
    [ClientRpc]
    void PrimaryFireClientRpc(Vector3 spawnPos, Vector3 dirToMove)
    {
        //owner does not need to see this
        if (IsOwner) { return; }

        SpawnProjectile(spawnPos, dirToMove);
    }
}
