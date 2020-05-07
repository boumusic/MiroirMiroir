using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaveButton : UIButton
{
    public void Leave()
    {
        DestroyImmediate(UIManager.instance.player.gameObject);
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        DestroyImmediate(UIManager.instance.player.gameObject);
        Application.Quit();
    }
}
