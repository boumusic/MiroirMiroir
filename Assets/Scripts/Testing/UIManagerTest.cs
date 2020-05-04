using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManagerTest : MonoBehaviour
{
    public static UIManagerTest instance;

    public GameObject startMenu;
    public TextMeshProUGUI usernameField;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void ConnectToServer()
    {
        startMenu.SetActive(false);
        //usernameField.interactable = false;
        ClientTest.instance.ConnectToServer();
    }
}