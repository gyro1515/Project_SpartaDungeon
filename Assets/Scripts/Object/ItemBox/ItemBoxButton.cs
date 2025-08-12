using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBoxButton : MonoBehaviour, IInteractable
{
    [Header("아이템 박스 버튼 세팅")]
    [SerializeField] ItemBox itemBox;

    public void OnInteract()
    {
        itemBox.InteractBox();
    }
}
