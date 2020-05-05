using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CustomNetworkManager : NetworkManager
{
    public bool debug = false;

    public override void Start()
    {
        base.Start();

        string name = "";
        if(Info) name = Info.username;

#if UNITY_EDITOR
        GetComponent<NetworkManagerHUD>().showGUI = true;
#else

        if (!debug)
        {
            if (name == "server")
                StartServer();
            else
                StartClient();
        }
        else
        {
            networkAddress = "localhost";
            if (name == "host")
                StartHost();
            else
                StartClient();
        }

#endif
    }

    public override GameObject OnServerAddPlayer(NetworkConnection conn)
    {
        GameObject player = base.OnServerAddPlayer(conn);

        if(Info) player.GetComponent<Player>().username = Info.username;

        return player;
    }

    private PlayerInfo Info => FindObjectOfType<PlayerInfo>();
}
