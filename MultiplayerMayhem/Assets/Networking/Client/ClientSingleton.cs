using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ClientSingleton : MonoBehaviour
{
    static ClientSingleton instance;
    ClientGameManager gameManager;

    public static ClientSingleton Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            instance = FindObjectOfType<ClientSingleton>();

            if (instance == null)
            {
                Debug.Log("No client singleton in scene");
                return null;
            }

            return instance;
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public async Task CreateClient()
    {
        gameManager = new ClientGameManager();
        //Wait to run code beneath the async method.
        await gameManager.InitAsync();
    }
}