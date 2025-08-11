using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDUI : MonoBehaviour
{
    [Header("HUD 세팅")]
    [SerializeField] Image hpBar;
    [SerializeField] Image hungerBar;
    [SerializeField] Image steminaBar;
    [SerializeField] GameObject GainItemPanel;

    /*private void Awake()
    {
        
        // 굳이 이렇게 찾아야 할까...?
        //hpBar = transform.Find("HpBar")?.GetComponent<Image>();
    }
    private void Start()
    {
        UIManager.Instance.HUDUI = this;
    }*/
    public void SetHpBar(float value)
    {
        if (hpBar) hpBar.fillAmount = value;
    }
    public void SetHungerBar(float value)
    {
        if (hungerBar) hpBar.fillAmount = value;
    }
    public void SetSteminaBar(float value)
    {
        if (steminaBar) hpBar.fillAmount = value;
    }
    public void SetGainItemPanelActive(bool active)
    {
        GainItemPanel?.SetActive(active);
    }
}
