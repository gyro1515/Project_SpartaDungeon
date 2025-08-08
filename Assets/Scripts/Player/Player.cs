using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseCharacter
{
    [Header("점프력 설정")]
    [SerializeField] float jumpPower = 80f;
    public float JumpPower { get { return jumpPower; } set { jumpPower = value; } }
    PlayerController controller;
    HUDUI hudUI;
    protected override void Awake()
    {
        base.Awake();
        controller = GetComponent<PlayerController>();
        // hudUI -> UIManager에서 가져오기
    }

    public float GetCurHpRatio()
    {
        return curHp / (float)maxHp;
    }
}
