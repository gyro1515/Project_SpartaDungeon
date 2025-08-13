using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionObjectUI : MonoBehaviour
{
    [Header("상호작용 설정")]
    [SerializeField] TextMeshProUGUI interactionText;
    
    public void SetText(string value)
    {
        interactionText.text = $"[E] {value}";
    }

}
