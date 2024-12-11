using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientGameManager
{
    const string MENUSCENENAME = "Menu";

    //Async allows you to run the method asynchronous of the code running (Runs in background. Returns when it is done)
    public async Task<bool> InitAsync()
    {
        await UnityServices.InitializeAsync();
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
}
