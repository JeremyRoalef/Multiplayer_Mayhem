using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkServer
{
    NetworkManager networkManager;

    public NetworkServer(NetworkManager networkManager)
    {
        this.networkManager = networkManager;

        //Triggered when someone connects to the server & gives info about the connection
        networkManager.ConnectionApprovalCallback += ApprovalCheck;
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        //Get the bytearray streamed to us
        string payload = System.Text.Encoding.UTF8.GetString(request.Payload);
        //Convert to a json string & convert that to a readable object
        UserData userData = JsonUtility.FromJson<UserData>(payload);

        Debug.Log(userData.userName);

        response.Approved = true;
        response.CreatePlayerObject = true;
    }
}
