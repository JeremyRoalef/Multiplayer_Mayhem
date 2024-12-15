using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkClient
{
    const string MAINMENUNAME = "Menu";

    NetworkManager networkManager;

    public NetworkClient(NetworkManager networkManager)
    {
        this.networkManager = networkManager;

        //Triggered when someone connects to the server & gives info about the connection
        networkManager.OnClientDisconnectCallback += OnClientDisconnect;
    }

    private void OnClientDisconnect(ulong clientId)
    {
        //Dont run if server
        if (clientId != 0 && clientId != networkManager.LocalClientId) {return;}

        if (SceneManager.GetActiveScene().name != MAINMENUNAME)
        {
            SceneManager.LoadScene(MAINMENUNAME);
        }

        if (networkManager.IsConnectedClient)
        {
            networkManager.Shutdown();
        }
    }
}
