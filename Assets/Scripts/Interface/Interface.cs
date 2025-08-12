using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void TakeDamage(float damage); // 데미지 주기
}

public interface IJumpable
{
    public float JumpPower { get; set; }
    public bool IsJump { get; set; }
    public int JumpCount { get; set; } // 점프 가능 횟수
    public int CurJumpCount { get { return JumpCount; } set { JumpCount = value; } } // 현재 점프 가능 횟수
    void StartJump(); // 점프 시작
    void EndJump(); // 점프 앤드
}
public interface IAttackable
{
    public void Attack(); // 공격 정의하기
    public void Hit(); // 공격하고 언제 히트 판정을 할 것인지 정의하기
}

public interface IInteractable
{
    public void OnInteract();
}
