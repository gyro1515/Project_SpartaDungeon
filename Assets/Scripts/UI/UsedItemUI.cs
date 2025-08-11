using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsedItemUI : MonoBehaviour
{
    [Header("사용한 아이템 UI 설정")]
    [SerializeField] Transform bGSlot;
    [SerializeField] GameObject usedItemIconPrefab;
    Dictionary<ItemData, UsedItemIcon> usedItemDic = new Dictionary<ItemData, UsedItemIcon>(); // 아이템 데이터와 아이콘 매핑
    public Dictionary<ItemData, UsedItemIcon> UsedItemDic { get { return usedItemDic; } }
    // 아이콘 순서 정렬용 리스트
    List<UsedItemIcon> icons = new List<UsedItemIcon>();

    private void Update()
    {
        SortImagesRemainTime(); // 아이콘의 남은 시간에 따라 정렬
    }
    public void AddIcon(ItemData itemData)
    {
        if (usedItemDic.ContainsKey(itemData)) // 이미 있는 경우 시간만 다시 돌아가게 하기
        {
            usedItemDic[itemData].Init(itemData); // 아이콘 초기화
        }
        else
        {
            GameObject icon = Instantiate(usedItemIconPrefab, bGSlot);
            UsedItemIcon usedItemIcon = icon.GetComponent<UsedItemIcon>();
            usedItemIcon.Init(itemData); // 아이콘 초기화
            usedItemDic.Add(itemData, usedItemIcon); // 아이템 데이터 추가
            icons.Add(usedItemIcon);
        }
    }
    void SortImagesRemainTime()
    {
        
        icons.Sort((a, b) => b.GetRemainingTime().CompareTo(a.GetRemainingTime())); // 내림차순 정렬
        for (int i = 0; i < icons.Count; i++)
            icons[i].transform?.SetSiblingIndex(i);
    }
    public void RemoveIcon(ItemData itemData)
    {
        if (usedItemDic.ContainsKey(itemData))
        {
            usedItemDic.Remove(itemData); // 아이템 데이터 제거

            icons.Clear(); // 아이콘 리스트 초기화
            foreach (var icon in UsedItemDic)
            {
                icons.Add(icon.Value);
            }
        }
    }
}
