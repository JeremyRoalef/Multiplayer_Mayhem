using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ConnectionButtons : MonoBehaviour
{
    public void StartHosting()
    {
        //Create the client as a host
        NetworkManager.Singleton.StartHost();
    }

    public void OnButtonJoinAsClientClick()
    {
        //Create the client in the game
        NetworkManager.Singleton.StartClient();
    }
}
