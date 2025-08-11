using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : BaseCharacter
{
    [Header("플레이어 설정")]
    [SerializeField] float jumpPower = 80f;
    [SerializeField] float maxHunger = 100f;
    [SerializeField] float hungerPassive = 3f;
    [SerializeField] float maxStemina = 100f;
    [SerializeField] float steminaPassive = 5f;
    [SerializeField] Transform dropPosition;

    float curHunger = 0f;
    public float CurHunger { get { return curHunger; } set { curHunger = Mathf.Clamp(value, 0f, maxHunger); } }
    public float MaxHunger { get { return maxHunger; } }
    public float HungerPassive { get { return hungerPassive; } }
    float curStemina = 0f;
    public float CurStemina { get { return curStemina; } set { curStemina = Mathf.Clamp(value, 0f, maxStemina); } }
    public float MaxStemina { get { return maxStemina; } }
    public float SteminaPassive { get { return steminaPassive; } }
    // 무적
    bool isInvincible = false;
    public bool IsInvincible { get { return isInvincible; } set { isInvincible = value; } }
    // 드롭 위치
    public Transform DropPosition { get { return dropPosition; } }
    public float JumpPower { get { return jumpPower; } set { jumpPower = value; } }
    // 플레이어 컨트롤러
    PlayerController controller;
    public PlayerController Controller { get { return controller; } }
    // 플레이어 상태 컨트롤러
    PlayerStateController stateController;
    public PlayerStateController StateController { get { return stateController; } }
    // 인벤토리 아이템 추가용 델리게이트
    //public delegate void AddItemAction(ItemData item);
    Action<ItemData> addItem;
    public Action<ItemData> AddItem { get { return addItem; } set { addItem = value; } }

    protected override void Awake()
    {
        base.Awake();
        controller = GetComponent<PlayerController>();
        stateController = GetComponent<PlayerStateController>();
        curHunger = maxHunger;
        curStemina = maxStemina;
    }
    public float GetCurHpRatio()
    {
        return curHp / (float)maxHp;
    }
    public float GetCurHungerRatio()
    {
        return curHunger / maxHunger;
    }
    public float GetCurSteminaRatio()
    {
        return curStemina / maxStemina;
    }
}
