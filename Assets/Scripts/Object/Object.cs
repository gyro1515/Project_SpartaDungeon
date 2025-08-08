using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    [Header("오브젝트 설정")]
    [SerializeField] ObjectData objectData;
    ObjectUI onjectUI;

    private void Awake()
    {
        onjectUI = GetComponentInChildren<ObjectUI>(true); // 활성화 안된 것도 가져오기
        SetText();
    }

    void SetText()
    {
        onjectUI.SetdescriptionText(objectData.description);
        onjectUI.SetNameText(objectData.name);
    }
    public void SetActive(bool active)
    {
        onjectUI.SetActive(active);
    }
}
