using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    [Header("오브젝트 설정")]
    [SerializeField] protected ObjectData objectData;

    protected ObjectUI objectUI = null;
    public ObjectUI ObjectUI { get { return objectUI; } }


    private void Start()
    {
        objectUI = UIManager.Instance.AddObjectUI(objectData, this);
    }
    public void SetActive(bool active)
    {
        if (objectUI == null) return;
        objectUI.SetActive(active);
    }
}
