using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public static class AuthenticationWrapper
{
    public static AuthState AuthState { get; private set; } = AuthState.NotAuthenticated;

    public static async Task<AuthState> DoAuth(int maxTries = 5)
    {
        if (AuthState == AuthState.Authenticated)
        {
            return AuthState;
        }

        if (AuthState == AuthState.Authenticating)
        {
            Debug.LogWarning("Already Authenticating!");
            await Authenticating();
            return AuthState;
        }

        await SignInAnonymouslyAsync(maxTries);

        return AuthState;
    }

    private static async Task SignInAnonymouslyAsync(int maxTries)
    {
        int tries = 0;


        AuthState = AuthState.Authenticating;
        while (AuthState == AuthState.Authenticating && tries < maxTries)
        {
            Debug.Log("Authenticating User...");

            try
            {
                Debug.Log("Signing in anonymously");
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                if (AuthenticationService.Instance.IsSignedIn && AuthenticationService.Instance.IsAuthorized)
                {
                    Debug.Log("Sign in successful");
                    AuthState = AuthState.Authenticated;
                    break;
                }
            }
            //Fail to authenticate
            catch (AuthenticationException ex)
            {
                Debug.LogError(ex);
                AuthState = AuthState.Error;
            }
            //Internet/Server failure
            catch (RequestFailedException ex)
            {
                Debug.LogError(ex);
                AuthState = AuthState.Error;
            }

            tries++;
            await Task.Delay(1000);
        }

        //No error, but not successfully authenticated
        if (AuthState != AuthState.Authenticated)
        {
            Debug.LogWarning($"Player was not authorized successfully in {tries} attempts!!!");
            AuthState = AuthState.TimeOut;
        }
    }

    static async Task<AuthState> Authenticating()
    {
        while (AuthState == AuthState.Authenticating || AuthState == AuthState.NotAuthenticated)
        {
            await Task.Delay(200);
        }

        return AuthState;
    }
}

public enum AuthState
{
    NotAuthenticated,
    Authenticating,
    Authenticated,
    Error,
    TimeOut
}