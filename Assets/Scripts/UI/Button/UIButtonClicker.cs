using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonClicker : MonoBehaviour
{
    public void Click()
    {
        GetComponent<UIButton>().Click();
    }
}
