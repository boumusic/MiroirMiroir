using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterColor : NetworkBehaviour
{
    [Header("Components")]
    public Renderer[] rends;

    [Header("Settings")]
    public List<Color> colors = new List<Color>();
    private int colorIndex;

    public Color CurrentColor => colors[colorIndex];
    
    [Command]
    public void CmdPreviousColor()
    {
        colorIndex--;
        UpdateColor();
        RpcChangeColor(colorIndex);
    }

    [Command]
    public void CmdNextColor()
    {
        colorIndex++;
        UpdateColor();
        RpcChangeColor(colorIndex);
    }

    [ClientRpc]
    private void RpcChangeColor(int index)
    {
        colorIndex = index;
        UpdateColor();
    }

    private void UpdateColor()
    {
        colorIndex = colorIndex % colors.Count;
        for (int i = 0; i < rends.Length; i++)
        {
            rends[i].material.SetColor("_Color", CurrentColor);
        }
    }
}
