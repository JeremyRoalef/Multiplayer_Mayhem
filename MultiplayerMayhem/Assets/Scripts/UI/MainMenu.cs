using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public async void StartHost()
    {
        HostSingleton.Instance.GameManager.StartHostAsync();
    }
}
