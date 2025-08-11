using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour
{
    [Header("스탯 설정")]
    [SerializeField] protected float maxHp = 0;
    [SerializeField] protected float hpPassive = 5;
    [SerializeField] protected float atkPower = 0;
    [SerializeField] protected float defPower = 0;
    [SerializeField] protected float walkSpeed = 0;
    [SerializeField] protected float runSpeed = 0;
    protected float curHp = 0;
    public float CurHp { get { return curHp; } set { curHp = Mathf.Clamp(value, 0f, maxHp); } }
    public float MaxHp { get { return maxHp; } set { maxHp = value; } }
    public float HpPassive { get { return hpPassive; } }
    public float AtkPower { get { return atkPower; } set { atkPower = value; } }
    public float DefPower { get { return defPower; } set { defPower = value; } }
    public float WalkSpeed { get { return walkSpeed; } set { walkSpeed = value; } }
    public float RunSpeed { get { return runSpeed; } set { runSpeed = value; } }
    

    // 컨트롤러도 여기에?

    protected virtual void Awake()
    {
        curHp = maxHp;
    }


}
