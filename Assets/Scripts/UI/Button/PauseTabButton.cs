using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseTabButton : UIButton
{
    public PauseTabType type;
    public override void Click()
    {
        base.Click();
        UIManager.instance.SwitchPauseTab(type);
    }
}
