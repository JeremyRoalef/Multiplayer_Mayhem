using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

//This class will never exist. Will only be inherited
public abstract class Coin : NetworkBehaviour
{
    [SerializeField]
    SpriteRenderer spriteRenderer;

    protected int coinValue;
    protected bool alreadyCollected;

    public abstract int Collect();
    public void SetValue(int value)
    {
        coinValue = value;
    }
    protected void Show(bool show)
    {
        spriteRenderer.enabled = show;
    }
}
