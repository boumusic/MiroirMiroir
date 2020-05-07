using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum PauseTabType
{
    Home,
    Controls,
    Options
}

[System.Serializable]
public class PauseTab
{
    public PauseTabType type;
    public GameObject go;
}

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [HideInInspector]
    public Player player;

    [Header("Chat")]
    public TMP_InputField chatWindow;
    public bool IsTyping => chatWindow.gameObject.activeInHierarchy;
    public bool CanOpenChat { get; private set; }

    [Header("Pause")]
    public GameObject pause;
    public PauseTab[] tabs;

    private PauseTabType currentTab;

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
        pause.SetActive(false);
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
        bool active = !pause.activeInHierarchy;
        pause.SetActive(active);
        if(active)
        {
            SwitchPauseTab(PauseTabType.Home);
        }
    }

    public void SwitchPauseTab(PauseTabType type)
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].go.SetActive(type == tabs[i].type);
        }
    }

    private IEnumerator ResetOpenChat()
    {
        yield return new WaitForSeconds(0.1f);
        CanOpenChat = true;
    }
}
