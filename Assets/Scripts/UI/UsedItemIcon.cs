using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsedItemIcon : MonoBehaviour
{
    [Header("사용한 아이템 아이콘 설정")]
    [SerializeField] private Image iconImg; // 아이콘 이미지
    [SerializeField] private Image tmpIconImg; // 투명 아이콘용 뒷 배경

    ItemData itemData;
    float duration = -1.0f;
    float timer = 0.0f; // 타이머 초기화

    void Update()
    {
        if (duration == -1.0f) return; // 지속 시간이 설정되지 않은 경우 업데이트 중지

        timer += Time.deltaTime; // 지속 시간 감소
        SetValue(1 - timer / duration); // 아이콘의 채우기 비율 업데이트

        if (timer >= duration)
        {
            UIManager.Instance.UsedItemUI.RemoveIcon(itemData); // 아이콘 제거
            Destroy(gameObject); // 아이템 아이콘 제거
        }
    }
    public void Init(ItemData _itemData)
    {
        itemData = _itemData;
        iconImg.sprite = itemData.icon; // 아이콘 이미지 설정
        duration = itemData.consumables[0].duration; // 아이템의 지속 시간 설정
        timer = 0.0f; // 타이머 초기화
        SetValue(1.0f);
    }
    void SetValue(float value)
    {
        // 당근 이미지 수정 안돼서 배경만 세팅하기
        //iconImg.fillAmount = value;
        tmpIconImg.fillAmount = value;
    }
    public float GetRemainingTime()
    {
        return duration - timer; // 남은 시간 계산
    }
}
