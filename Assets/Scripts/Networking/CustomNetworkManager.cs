using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CustomNetworkManager : NetworkManager
{

    public override void Start()
    {
        base.Start();

        string name = "";
        if (Info) name = Info.username;

        if (name == "server")
            StartServer();
        else if (name == "host")
            StartHost();
        else
        {
            if (name.Contains("debug")) GetComponent<NetworkManagerHUD>().showGUI = true;
            else StartClient();
        }


#if UNITY_EDITOR
        GetComponent<NetworkManagerHUD>().showGUI = true;
#endif
    }

    public override GameObject OnServerAddPlayer(NetworkConnection conn)
    {
        GameObject player = base.OnServerAddPlayer(conn);

        if (Info) player.GetComponent<Player>().username = Info.username;

        return player;
    }

    private PlayerInfo Info => FindObjectOfType<PlayerInfo>();
}
