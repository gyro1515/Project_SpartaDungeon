using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDUI : MonoBehaviour
{
    [Header("HUD 세팅")]
    [SerializeField]Image hpBar;

    private void Awake()
    {
        UIManager.Instance.HUDUI = this;
        // 굳이 이렇게 찾아야 할까...?
        //hpBar = transform.Find("HpBar")?.GetComponent<Image>();
    }
    public void SetHpBar(float value)
    {
        if (hpBar) hpBar.fillAmount = value;
    }
}
