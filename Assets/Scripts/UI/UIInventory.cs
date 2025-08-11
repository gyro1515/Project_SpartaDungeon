using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    [SerializeField] ItemSlot[] slots;

    [SerializeField] GameObject inventoryWindow;
    [SerializeField] Transform slotPanel;
    Transform dropPosition;      // item 버릴 때 필요한 위치

    [Header("Selected Item")]           // 선택한 슬롯의 아이템 정보 표시 위한 UI
    [SerializeField] TextMeshProUGUI selectedItemName;
    [SerializeField] TextMeshProUGUI selectedItemDescription;
    [SerializeField] TextMeshProUGUI selectedItemStatName;
    [SerializeField] TextMeshProUGUI selectedItemStatValue;
    [SerializeField] GameObject useButton;
    [SerializeField] GameObject equipButton;
    [SerializeField] GameObject unEquipButton;
    [SerializeField] GameObject dropButton;

    private PlayerController controller;
    private PlayerStateController stateController;

    private ItemSlot selectedItem;
    private int selectedItemIndex;

    private int curEquipIndex;

    private void Awake()
    {
        // Inventory UI 초기화 로직들
        slots = new ItemSlot[slotPanel.childCount]; // 자식 개수 가져오기

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
            slots[i].Clear();
        }

        ClearSelectedItemWindow();

        // 버튼 클릭 할당
        useButton.GetComponent<Button>().onClick.AddListener(OnUseButton);
        dropButton.GetComponent<Button>().onClick.AddListener(OnDropButton);
        equipButton.GetComponent<Button>().onClick.AddListener(OnEquipButton);
        unEquipButton.GetComponent<Button>().onClick.AddListener(OnUnEquipButton);
    }
    private void Start()
    {
        controller = GameManager.Instance.Player.Controller;
        stateController = GameManager.Instance.Player.StateController;
        dropPosition = GameManager.Instance.Player.DropPosition;

        GameManager.Instance.Player.AddItem += AddItem;  // 아이템 파밍 시
        Toggle(); // 시작 시 Inventory 창 닫기
    }

    // 선택한 아이템 표시할 정보창 Clear 함수
    void ClearSelectedItemWindow()
    {
        selectedItem = null;
        selectedItemIndex = -1;

        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unEquipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    // Inventory 창 Open/Close 시 호출
    public void Toggle()
    {
        if (IsOpen()) inventoryWindow.SetActive(false);
        else inventoryWindow.SetActive(true);
    }

    public bool IsOpen()
    {
        //return inventoryWindow.activeInHierarchy;
        return inventoryWindow.activeSelf;
    }


    public void AddItem(ItemData data)
    {
        // 여러개 가질 수 있는 아이템이라면
        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;
                UpdateUI();
                return;
            }
        }

        // 빈 슬롯 찾기
        ItemSlot emptySlot = GetEmptySlot();

        // 빈 슬롯이 있다면
        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            return;
        }

        // 빈 슬롯 마저 없을 때
        ThrowItem(data);
    }

    // UI 정보 새로고침
    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            // 슬롯에 아이템 정보가 있다면
            if (slots[i].item != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    // 여러개 가질 수 있는 아이템의 정보 찾아서 return
    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }
        return null;
    }

    // 슬롯의 item 정보가 비어있는 정보 return
    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    // Player 스크립트 먼저 수정
    // 아이템 버리기 (실제론 매개변수로 들어온 데이터에 해당하는 아이템 생성)
    public void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }


    // ItemSlot 스크립트 먼저 수정
    // 선택한 아이템 정보창에 업데이트 해주는 함수
    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index];
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.item.displayName;
        selectedItemDescription.text = selectedItem.item.description;

        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        for (int i = 0; i < selectedItem.item.consumables.Length; i++)
        {
            selectedItemStatName.text += selectedItem.item.consumables[i].type.ToString() + "\n";
            selectedItemStatValue.text += selectedItem.item.consumables[i].value.ToString() + "\n";
        }

        useButton.SetActive(selectedItem.item.type == ItemType.Consumable);
        //equipButton.SetActive(selectedItem.item.type == ItemType.Equipable && !slots[index].equipped);
        //unEquipButton.SetActive(selectedItem.item.type == ItemType.Equipable && slots[index].equipped);
        dropButton.SetActive(!slots[index].equipped); // 장착되면 버리지 못하도록
    }

    public void OnUseButton()
    {
        if (selectedItem.item.type == ItemType.Consumable)
        {
            stateController.ApplyConsumable(selectedItem.item);
            /*for (int i = 0; i < selectedItem.item.consumables.Length; i++)
            {
                // 스위치대신 딕셔너리로 처리해보기
                stateController.ApplyConsumable(selectedItem.item.consumables[i]);

                *//*switch (selectedItem.item.consumables[i].type)
                {
                    case ConsumableType.Health:
                        //condition.Heal(selectedItem.item.consumables[i].value);
                        break;
                    case ConsumableType.Hunger:
                        //condition.Eat(selectedItem.item.consumables[i].value);
                        break;
                }*//*
            }*/
            RemoveSelctedItem();
        }
    }

    public void OnDropButton()
    {
        if (selectedItem.equipped) return; // 장착된 아이템은 버리지 못하도록
        ThrowItem(selectedItem.item);
        RemoveSelctedItem();
    }

    void RemoveSelctedItem()
    {
        selectedItem.quantity--;

        if (selectedItem.quantity <= 0)
        {
            if (slots[selectedItemIndex].equipped)
            {
                //UnEquip(selectedItemIndex);
            }

            selectedItem.item = null;
            ClearSelectedItemWindow();
        }

        UpdateUI();
    }

    
    public void OnEquipButton()
    {
        //if (selectedItemIndex == curEquipIndex) return;

        if (slots[curEquipIndex].equipped)
        {
            UnEquip(curEquipIndex);
        }

        slots[selectedItemIndex].equipped = true;
        curEquipIndex = selectedItemIndex;
        //GameManager.Instance.Player.equip.EquipNew(selectedItem.item);
        UpdateUI();

        SelectItem(selectedItemIndex);
    }

    void UnEquip(int index)
    {
        slots[index].equipped = false;
        //GameManager.Instance.Player.equip.UnEquip();
        UpdateUI();

        if (selectedItemIndex == index)
        {
            SelectItem(selectedItemIndex);
        }
    }

    public void OnUnEquipButton()
    {
        UnEquip(selectedItemIndex);
    }

    
}
