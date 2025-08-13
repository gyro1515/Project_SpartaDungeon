using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour, IDamagable
{
    Player player;
    private Dictionary<ConsumableType, Action<ItemDataConsumable>> effectHandlers; // 소비 아이템 처리용
    private void Awake()
    {
        player = GetComponent<Player>();
        effectHandlers = new Dictionary<ConsumableType, Action<ItemDataConsumable>>
        {
            { ConsumableType.Health, AddHealth },
            { ConsumableType.Stemena, AddStemena },
            { ConsumableType.Hunger, AddHunger },
            { ConsumableType.Speed, AddSpeed },
            { ConsumableType.Invincibility, ApplyInvincibility },
            { ConsumableType.DoubleJump, ApplyDoubleJump }
        };
    }
    private void Update()
    {
        ConditionPassive(); // 플레이어 상태 지속적 변화
        Dash(); // 대쉬 처리
        // 테스트 용도
        if (Input.GetKeyDown(KeyCode.F)) TakeDamage(10);
    }
    public void TakeDamage(float damage) // IDamagable 상속
    {
        if(player.IsInvincible) return; // 무적 상태면 데미지 받지 않음
        player.CurHp -= damage;
        UIManager.Instance.SetHpBar(player.GetCurHpRatio());
    }
    void ConditionPassive()
    {
        player.CurHp += player.HpPassive * Time.deltaTime; // 체력 증가
        UIManager.Instance.SetHpBar(player.GetCurHpRatio());
        player.CurHunger -= player.HungerPassive * Time.deltaTime; // 배고픔 감소
        UIManager.Instance.SetHungerBar(player.GetCurHungerRatio());
        player.CurStemina += player.SteminaPassive * Time.deltaTime; // 스테미나 증가
        UIManager.Instance.SetSteminaBar(player.GetCurSteminaRatio());
    }
    void Dash()
    {
        if(!player.IsDashing) return; // 대쉬 중이 아니면 종료
        AddStemina(-player.DashStemina * Time.deltaTime); // 대쉬 중 스테미나 감소
    }
    public void ApplyConsumable(ItemData itemData)
    {
        bool hasDuration = false;
        foreach (var consumable in itemData.consumables)
        {
            if (!effectHandlers.TryGetValue(consumable.type, out var handler)) return;

            handler.Invoke(consumable);
            hasDuration = consumable.duration > 0; // 지속 시간이 있는지 확인
        }
        if (!hasDuration) return; // 지속 시간이 없는 경우는 여기서 종료
        // 아이템 사용 아이콘 생성
        UIManager.Instance.UsedItemUI.AddIcon(itemData); // 아이템 사용 아이콘 추가

        // 아이콘과 실제 작용 동기화 필요, 오차 발생 가능성 있음 -> 갈아 엎어야 할 거 같으니 나중으로 미루기

    }

    private void AddHealth(ItemDataConsumable consumable)
    {
        player.CurHp += consumable.value;
        UIManager.Instance.SetHpBar(player.GetCurHpRatio());
    }
    private void AddHunger(ItemDataConsumable consumable)
    {
        player.CurHunger += consumable.value;
        UIManager.Instance.SetHungerBar(player.GetCurHungerRatio());
    }
    private void AddStemena(ItemDataConsumable consumable)
    {
        player.CurStemina += consumable.value;
        UIManager.Instance.SetSteminaBar(player.GetCurSteminaRatio());

    }
    public void AddStemina(float value) // 스테미나 증가 메소드, 외부에서 호출 가능
    {
        player.CurStemina += value;
        UIManager.Instance.SetSteminaBar(player.GetCurSteminaRatio());
    }
    private void AddSpeed(ItemDataConsumable consumable)
    {
        StartCoroutine(SpeedBuffRoutine(consumable.value, consumable.duration));
    }

    private IEnumerator SpeedBuffRoutine(float value, float duration)
    {
        float originalWalkSpeed = player.WalkSpeed;
        float originalRunSpeed = player.RunSpeed;
        player.WalkSpeed *= value;
        player.RunSpeed *= value;
        yield return new WaitForSeconds(duration);
        player.WalkSpeed = originalWalkSpeed;
        player.RunSpeed = originalRunSpeed;
    }
    void ApplyInvincibility(ItemDataConsumable consumable)
    {
        StartCoroutine(InvincibilityRoutine(consumable.duration));
    }
    private IEnumerator InvincibilityRoutine(float duration)
    {
        player.IsInvincible = true;
        yield return new WaitForSeconds(duration);
        player.IsInvincible = false;
    }
    void ApplyDoubleJump(ItemDataConsumable consumable)
    {
        StartCoroutine(DoubleJumpRoutine(consumable.duration));
    }
    private IEnumerator DoubleJumpRoutine(float duration)
    {
        int originalJumpCount = player.Controller.JumpCount;
        player.Controller.JumpCount = 2;
        player.Controller.CurJumpCount = player.Controller.JumpCount - (originalJumpCount - player.Controller.CurJumpCount); // 현재 점프 가능 횟수 갱신
        yield return new WaitForSeconds(duration);
        player.Controller.JumpCount = originalJumpCount;
    }
}
