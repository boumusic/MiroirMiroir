﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CustomNetworkManager : NetworkManager
{
    public bool debug = false;

    public override void Start()
    {
        base.Start();

        if (!debug) StartClient();
        else
        {
            networkAddress = "localhost";
            if (FindObjectOfType<PlayerInfo>().username == "host")
                StartHost();
            else
                StartClient();
        }
    }

    public override GameObject OnServerAddPlayer(NetworkConnection conn)
    {
        GameObject player = base.OnServerAddPlayer(conn);

        player.GetComponent<Player>().username = FindObjectOfType<PlayerInfo>().username;

        return player;
    }
}
