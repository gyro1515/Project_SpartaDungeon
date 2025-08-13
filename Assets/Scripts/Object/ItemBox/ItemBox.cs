using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class ItemBox : Object
{
    private static readonly int OpenBox = Animator.StringToHash("Open");
    private static readonly int CloseBox = Animator.StringToHash("Close");
    Animator animator;
    bool isOpen = false;
    public bool IsOpen { get { return isOpen; } }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        
    }
    public void InteractBox()
    {
        if (isOpen)
        {
            animator.SetTrigger(CloseBox);
        }
        else
        {
            animator.SetTrigger(OpenBox);
        }
        isOpen = !isOpen;
    }
}
