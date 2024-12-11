using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : NetworkBehaviour
{
    [Header("References")]

    [SerializeField]
    Health health;

    [SerializeField]
    Image healthBarImage;

    public override void OnNetworkSpawn()
    {
        //This is only for client end. No effect on gameplay
        if (!IsClient) { return; }
        health.CurrentHealth.OnValueChanged += HandleHealthChange;
        HandleHealthChange(0, health.CurrentHealth.Value);
    }
    public override void OnNetworkDespawn()
    {
        if (!IsClient) { return; }
        health.CurrentHealth.OnValueChanged -= HandleHealthChange;
    }

    //Subscription to the OnValueChanged event in the property health.CurrentHealth. Ignore oldHealth. It's necessary for property events
    void HandleHealthChange(int oldHealth, int newHealth)
    {
        healthBarImage.fillAmount = (float)newHealth / health.MaxHealth;
    }
}
