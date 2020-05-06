using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    public Character character;
    [SyncVar(hook = nameof(UpdateName))]
    public string username = "";
       
    private void Start()
    {
        if (hasAuthority && isClient)
        {
            UIManager.instance.player = this;
            character.Initialize(this);

            PlayerInfo info = FindObjectOfType<PlayerInfo>();
            if(info) CmdUpdateName(info.username);
        }
    }

    private void Update()
    {
        if (!hasAuthority) return;
        Inputs();
    }

    [Command]
    void CmdUpdateName(string name)
    {
        username = name;
    }

    public void UpdateName(string oldValue, string newValue)
    {
        character.UpdateTextName(newValue);
    }

    private void Inputs()
    {
        if (UIManager.instance.IsTyping) return;
        character.InputHorizontal(Input.GetAxisRaw("Horizontal"));
        
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            character.color.CmdNextColor();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            character.color.CmdPreviousColor();
        }

        if (Input.GetKeyDown(KeyCode.Return) && UIManager.instance.CanOpenChat)
        {
            UIManager.instance.OpenChatWindow();
            character.InputHorizontal(0);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.instance.TogglePause();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            character.StartJump();
        }

        character.UpdateDown(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.S));
        character.InputSight(Input.GetKey(KeyCode.Mouse1));
    }
}
