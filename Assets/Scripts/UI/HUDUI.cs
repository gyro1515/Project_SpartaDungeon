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
    [SerializeField] GameObject InterationObjectPanel;
    InteractionObjectUI interactionObjectUI;
    private void Awake()
    {
        interactionObjectUI = InterationObjectPanel.GetComponent<InteractionObjectUI>();

        // 굳이 이렇게 찾아야 할까...?
        //hpBar = transform.Find("HpBar")?.GetComponent<Image>();
    }
    //private void Start()
    //{
    //    UIManager.Instance.HUDUI = this;
    //}
    public void SetHpBar(float value)
    {
        if (hpBar) hpBar.fillAmount = value;
    }
    public void SetHungerBar(float value)
    {
        if (hungerBar) hungerBar.fillAmount = value;
    }
    public void SetSteminaBar(float value)
    {
        if (steminaBar) steminaBar.fillAmount = value;
    }
    public void SetInterationObjectPanelActive(bool active)
    {
        InterationObjectPanel?.SetActive(active);
    }
    public void SetInteractionObjectText(string value)
    {
        //Debug.Log("HudUI");
        interactionObjectUI?.SetText(value);
    }
}
