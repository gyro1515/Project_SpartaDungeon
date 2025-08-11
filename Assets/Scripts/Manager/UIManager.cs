using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonMono<UIManager>
{
    [Header("UI 매니저 설정")]
    [SerializeField] private GameObject objectPanels;
    [SerializeField] private HUDUI hudUI;
    [SerializeField] private UsedItemUI usedItemUI;
    public HUDUI HUDUI { get { return hudUI; } set { hudUI = value; } }
    public UsedItemUI UsedItemUI { get { return usedItemUI; } set { usedItemUI = value; } }
    // 오브젝트 UI
    //private List<ObjectUI> objectUIs = new List<ObjectUI>();
    GameObject objectUIPrefab = null;

    // 인벤토리 UI
    private UIInventory uiInventory = null;
    public UIInventory UIInventory { get { return uiInventory; } set { uiInventory = value; } }
    GameObject inventoryPrefab = null;


    override protected void Awake()
    {
        base.Awake();
        objectUIPrefab = Resources.Load<GameObject>("UI/ObjectPanel");
        inventoryPrefab = Resources.Load<GameObject>("UI/InventoryPanel");
        uiInventory = Instantiate(inventoryPrefab, gameObject.transform).GetComponent<UIInventory>();
    }

    public void SetHpBar(float value)
    {
        hudUI.SetHpBar(value);
    }
    public void SetHungerBar(float value)
    {
        hudUI.SetHungerBar(value);
    }
    public void SetSteminaBar(float value)
    {
        hudUI.SetSteminaBar(value);
    }
    public ObjectUI AddObjectUI(ObjectData objectData, Object _object)
    {
        GameObject objectUIGO = Instantiate(objectUIPrefab, objectPanels.transform);
        ObjectUI objectUI = objectUIGO.GetComponent<ObjectUI>();
        objectUI.SetdescriptionText(objectData.description);
        objectUI.SetNameText(objectData.displayName);
        objectUI.SetTargetObject(_object);
        objectUI.UIPosInit(objectData.uIPos, objectData.screenPos);
        objectUIGO.SetActive(false);
        return objectUI;
    }
    public void InventoryToggle()
    {
        uiInventory.Toggle();
    }
    public void SetGainItemPanelActive(bool active)
    {
        hudUI.SetGainItemPanelActive(active);
    }
}
