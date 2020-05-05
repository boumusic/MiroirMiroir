using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Player player;

    [Header("Chat")]
    public TMP_InputField chatWindow;
    public bool IsTyping => chatWindow.gameObject.activeInHierarchy;
    public bool CanOpenChat { get; private set; }

    [Header("Pause")]
    public GameObject pause;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else
        {
            DestroyImmediate(this);
        }

        CanOpenChat = true;
        chatWindow.gameObject.SetActive(false);
    }

    public void OpenChatWindow()
    {
        chatWindow.gameObject.SetActive(true);
        chatWindow.ActivateInputField();
    }

    public void ConfirmMessage()
    {
        string text = chatWindow.text;
        chatWindow.text = "";
        if(text != "")
            player.character.DisplayBubble(text);
        chatWindow.gameObject.SetActive(false);
        CanOpenChat = false;
        StartCoroutine(ResetOpenChat());
    }

    public void TogglePause()
    {
        pause.SetActive(!pause.activeInHierarchy);
    }

    private IEnumerator ResetOpenChat()
    {
        yield return new WaitForSeconds(0.1f);
        CanOpenChat = true;
    }
}
