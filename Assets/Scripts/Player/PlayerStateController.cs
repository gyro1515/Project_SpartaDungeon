using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour, IDamagable
{
    Player player;
    private void Awake()
    {
        player = GetComponent<Player>();
    }
    private void Update()
    {
        ConditionPassive(); // 플레이어 상태 지속적 변화

        // 테스트 용도
        if (Input.GetKeyDown(KeyCode.F)) TakeDamage(10);
    }
    public void TakeDamage(float damage) // IDamagable 상속
    {
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
}
