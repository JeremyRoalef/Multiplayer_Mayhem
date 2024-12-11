using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [field: SerializeField] //field: SerializedField makes the property available in inspector while property is private set
    public int MaxHealth { get; private set; } = 100; //Can publicly get the variable, only privately set the variable


    //Creating a network variable -- This variable is only modifiable on the server's end (server ownership)
    public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>();
    bool isDead = false;

    public Action<Health> OnDie;
    public override void OnNetworkSpawn()
    {
        //No need to do anything if you're not the server
        if (!IsServer) { return; }

        CurrentHealth.Value = MaxHealth;
    }

    public void TakeDamage(int damageToTake)
    {
        ModifyHealth(-damageToTake);
    }

    public void RestoreHealth(int healthAmount)
    {
        ModifyHealth(healthAmount);
    }

    void ModifyHealth(int value)
    {
        if (isDead) { return; }

        int newHealth = CurrentHealth.Value + value;
        CurrentHealth.Value = Math.Clamp(newHealth, 0, MaxHealth);

        if (CurrentHealth.Value == 0)
        {
            OnDie?.Invoke(this);
            isDead = true;
        }
    }
}
