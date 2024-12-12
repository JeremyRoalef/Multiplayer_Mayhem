using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NameSelector : MonoBehaviour
{
    [SerializeField]
    TMP_InputField nameField;

    [SerializeField]
    Button connectButton;

    [SerializeField]
    int minNameLength = 1;

    [SerializeField]
    int maxNameLength = 25;

    const string PLAYERNAMEKEY = "PlayerName";


    void Start()
    {
        //Headless server
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            return; 
        }

        nameField.text = PlayerPrefs.GetString(PLAYERNAMEKEY, string.Empty);

        HandleNameChanged();
    }

    public void HandleNameChanged()
    {
        connectButton.interactable = 
            nameField.text.Length >= minNameLength && nameField.text.Length <= maxNameLength;
    }

    public void Connect()
    {
        PlayerPrefs.SetString(PLAYERNAMEKEY, nameField.text);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
