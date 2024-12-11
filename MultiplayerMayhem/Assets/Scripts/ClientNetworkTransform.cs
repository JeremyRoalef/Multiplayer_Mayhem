using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;


//Unity's script. This give clients authority over their position and rotation as set up in the player script.
public class ClientNetworkTransform : NetworkTransform
{
    //Client owns this object
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }

    protected override void Update()
    {
        CanCommitToTransform = IsOwner;
        base.Update();

        //safety checks
        if (NetworkManager != null)
        {
            //If connected to server as a client
            if (NetworkManager.IsConnectedClient || NetworkManager.IsListening)
            {
                if (CanCommitToTransform)
                {
                    TryCommitTransformToServer(transform, NetworkManager.LocalTime.Time);
                }
            }
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        CanCommitToTransform = IsOwner;
    }
}
