using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using Unity.Networking.Transport.Relay;
using Unity.Services.Core;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text;
using Unity.Services.Authentication;

public class ClientGameManager
{


    const string MENUSCENENAME = "Menu";
    JoinAllocation allocation;
    NetworkClient networkClient;

    //Async allows you to run the method asynchronous of the code running (Runs in background. Returns when it is done)
    public async Task<bool> InitAsync()
    {
        await UnityServices.InitializeAsync();

        networkClient = new NetworkClient(NetworkManager.Singleton);

        //Authenticate player
        AuthState authState = await AuthenticationWrapper.DoAuth();
        if (authState == AuthState.Authenticated)
        {
            return true;
        }
        return false;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(MENUSCENENAME);
    }

    public async Task StartClientAsync(string joinCode)
    {
        try
        {
            allocation = await Relay.Instance.JoinAllocationAsync(joinCode);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return;
        }

        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
        transport.SetRelayServerData(relayServerData);

        UserData userData = new UserData()
        {
            userName = PlayerPrefs.GetString(NameSelector.PLAYERNAMEKEY, "John Doe"),
            userAuthId = AuthenticationService.Instance.PlayerId
        };

        //convert user data to json & bytearray
        string payload = JsonUtility.ToJson(userData);
        byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);

        NetworkManager.Singleton.NetworkConfig.ConnectionData = payloadBytes;

        NetworkManager.Singleton.StartClient();
    }
}
