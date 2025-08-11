using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectUI : MonoBehaviour
{
    [Header("오브젝트UI")]
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] const float activeTime = 3.0f; // 활성화 시간
    [SerializeField] RectTransform rectTransform;
    bool isActive = false;
    float activeTimer = 0f;
    Object targetObject = null;
    Camera cam = null;
    Player player = null;
    Vector3 uIPos = Vector3.zero;
    Vector3 screenPos = Vector3.zero;
    private void Awake()
    {
        //rectTransform = GetComponent<RectTransform>();
        player = GameManager.Instance.Player;
    }

    private void Update()
    {
        if (!isActive) return;
        //Debug.Log(activeTimer);
        SetUIPos();
        activeTimer += Time.deltaTime;
        if (activeTimer >= activeTime || 
            Vector3.Distance(cam.gameObject.transform.position, targetObject.gameObject.transform.position) > 4f  ||
            Vector3.Dot(player.transform.forward, targetObject.gameObject.transform.position - player.transform.position) < 0f)
        {
            // 시간 다되거나, 플레이어와 거리 멀어거나, 플레이어가 바라보는 쪽이 아니라면 자동으로 꺼지기
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
    public void SetTargetObject(Object _object)
    {
        targetObject = _object;
    }
    public void SetActive(bool active)
    {
        if (isActive == active)
        {
            activeTimer = 0f;
            return;
        }
        if (active)
        {
            if (cam == null) cam = Camera.main;

            SetUIPos();
        }
        gameObject.SetActive(active);
        isActive = active;
    }
    void SetUIPos()
    {
        Vector3 tmpUIPos = cam.WorldToScreenPoint(targetObject.gameObject.transform.position + uIPos);
        rectTransform.position = tmpUIPos + screenPos;
    }
    public void UIPosInit(Vector3 _uIPos, Vector3 _screenPos)
    {
        uIPos = _uIPos;
        screenPos = _screenPos;
    }
}
