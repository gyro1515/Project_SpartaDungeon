using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ItemObject : Object, IInteractable
{
    /*[Header("아이템 오브젝트 설정")]
    [SerializeField] ItemData data;*/

    public void OnInteract()
    {
        GameManager.Instance.Player.AddItem?.Invoke((ItemData)objectData); // 형 변환하여 넘겨주기
        objectUI.SetActive(false); // 아이템 오브젝트 UI 비활성화
        Destroy(objectUI.gameObject); // 아이템 오브젝트 UI 삭제
        Destroy(gameObject);
    }
    public void SetInteractionText()
    {
        UIManager.Instance.SetInteractionObjectText("아이템 획득하기");
    }
}