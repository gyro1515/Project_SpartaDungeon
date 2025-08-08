using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectUI : MonoBehaviour
{
    [Header("오브젝트UI")]
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] Vector3 uIPos = Vector3.zero;
    [SerializeField] const float activeTime = 3.0f; // 활성화 시간
    [SerializeField] RectTransform rectTransform;
    bool isActive = false;
    float activeTimer = 0f;
    

    private void Awake()
    {
        //rectTransform = GetComponent<RectTransform>();
    }
    private void Update()
    {
        if (!isActive) return;
        //Debug.Log(activeTimer);
        activeTimer += Time.deltaTime;
        if(activeTimer >= activeTime)
        {
            // 시간 다되면 자동으로 꺼지기
            SetActive(false);
            activeTimer = 0.0f;
        }
    }
    public void SetNameText(string text)
    {
        nameText.text = text;
    }
    public void SetdescriptionText(string text)
    {
        descriptionText.text = text;
    }
    public void SetActive(bool active)
    {
        if (isActive == active)
        {
            activeTimer = 0f;
            return;
        }
        gameObject.SetActive(active);
        isActive = active;
        
    }
}
