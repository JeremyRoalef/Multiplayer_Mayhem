using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class JoinServer : MonoBehaviour
{
    public void OnButtonJoinAsClientClick()
    {
        //Create the client in the game
        NetworkManager.Singleton.StartClient(); 
    }
}
