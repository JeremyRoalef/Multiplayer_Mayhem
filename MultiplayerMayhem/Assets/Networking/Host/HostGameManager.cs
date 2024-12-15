using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Lobbies;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using Unity.Services.Lobbies.Models;
using System.Text;
using Unity.Services.Authentication;

public class HostGameManager
{
    Allocation allocation;
    NetworkServer networkServer;

    string joinCode;
    string lobbyId;
    const int MAXCONNECTIONS = 20;
    const string GAMESCENENAME = "Game";
    public async Task StartHostAsync()
    {
        try
        {
            allocation = await Relay.Instance.CreateAllocationAsync(MAXCONNECTIONS);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return;
        }

        try
        {
            joinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log($"Join code: {joinCode}");
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return;
        }

        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
        transport.SetRelayServerData(relayServerData);

        try
        {
            CreateLobbyOptions lobbyOptions = new CreateLobbyOptions();
            lobbyOptions.IsPrivate = false;
            lobbyOptions.Data = new Dictionary<string, DataObject>()
            {
                {
                    "JoinCode", new DataObject(
                        visibility: DataObject.VisibilityOptions.Member,
                        value: joinCode
                        )
                }
            };
            string playerName = PlayerPrefs.GetString(NameSelector.PLAYERNAMEKEY, "John Doe");
            Lobby lobby = await Lobbies.Instance.CreateLobbyAsync(
                $"{playerName}'s Lobby",
                MAXCONNECTIONS,
                lobbyOptions
                );
            lobbyId = lobby.Id;

            HostSingleton.Instance.StartCoroutine(HeartBeatLobby(15));
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            return;
        }

        networkServer = new NetworkServer(NetworkManager.Singleton);


        UserData userData = new UserData()
        {
            userName = PlayerPrefs.GetString(NameSelector.PLAYERNAMEKEY, "John Doe"),
            userAuthId = AuthenticationService.Instance.PlayerId
        };
        //convert user data to json & bytearray
        string payload = JsonUtility.ToJson(userData);
        byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);

        NetworkManager.Singleton.NetworkConfig.ConnectionData = payloadBytes;

        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene(GAMESCENENAME, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    //Ping the UGS to keep the lobby active
    IEnumerator HeartBeatLobby(float waitTimeSeconds)
    {
        WaitForSecondsRealtime waitForSecondsRealtime = new WaitForSecondsRealtime(waitTimeSeconds);

        while (true)
        {
            Lobbies.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return waitForSecondsRealtime;
        }
    }
}