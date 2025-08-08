using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonMono<UIManager>
{
    private HUDUI hudUI;
    public HUDUI HUDUI { get { return hudUI; } set { hudUI = value; } }

    public void SetHpBar(float value)
    {
        hudUI.SetHpBar(value);
    }
}
