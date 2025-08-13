using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBoxButton : MonoBehaviour, IInteractable
{
    [Header("아이템 박스 버튼 세팅")]
    [SerializeField] ItemBox itemBox;
    float activeTime = 0.8f;
    float timer = 0f;
    bool isActive = false;
    private void Update()
    {
        if (!isActive) return;
        timer += Time.deltaTime;
        if (timer >= activeTime)
        {
            isActive = false;
            timer = 0f;
        }
    }
    public void OnInteract()
    {
        if(isActive) return; 
        isActive = true;
        itemBox.InteractBox();
    }

    public void SetInteractionText()
    {
        if (isActive) return;
        string s = itemBox.IsOpen ? "닫기" : "열기";
        UIManager.Instance.SetInteractionObjectText($"보물 상자 {s}");
    }
}
