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
        if (colorIndex < 0) colorIndex = colors.Count -1;
        UpdateColor();
        RpcChangeColor(colorIndex);
    }

    [Command]
    public void CmdNextColor()
    {
        colorIndex++;
        if (colorIndex >= colors.Count) colorIndex = 0;
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
        for (int i = 0; i < rends.Length; i++)
        {
            rends[i].material.SetColor("_Color", CurrentColor);
        }
    }
}
