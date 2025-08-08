using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour
{
    [Header("스탯 설정")]
    [SerializeField] protected int maxHp = 0;
    [SerializeField] protected int atkPower = 0;
    [SerializeField] protected int defPower = 0;
    [SerializeField] protected int walkSpeed = 0;
    [SerializeField] protected int runSpeed = 0;
    protected int curHp = 0;
    public int CurHp { get { return curHp; } set { curHp = value; } }
    public int MaxHp { get { return maxHp; } set { maxHp = value; } }
    public int AtkPower { get { return atkPower; } set { atkPower = value; } }
    public int DefPower { get { return defPower; } set { defPower = value; } }
    public int WalkSpeed { get { return walkSpeed; } set { walkSpeed = value; } }
    public int RunSpeed { get { return runSpeed; } set { runSpeed = value; } }
    

    // 컨트롤러도 여기에?

    protected virtual void Awake()
    {
        curHp = maxHp;
    }


}
