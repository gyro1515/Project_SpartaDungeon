using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EquipTool : Equip
{
    public float attackRate;
    private bool attacking;
    public float attackDistance;
    public float useStamina;

    [Header("Resource Gathering")]
    public bool doesGatherResources;

    [Header("Combat")]
    public bool doesDealDamage;
    public int damage;

    private Animator animator;
    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
        animator = GetComponent<Animator>();
    }
    public override void OnAttackInput()
    {
        //if (attacking || !GameManager.Instance.Player.condition.UseStamina(useStamina)) return;
        if (attacking) return;

        attacking = true;
        animator.SetTrigger("Attack");
        Invoke("OnCanAttack", attackRate);
    }

    void OnCanAttack()
    {
        attacking = false;
    }
    public void OnHit()
    {

        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        /*Debug.DrawLine(cam.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0)), cam.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0)) +
            ray.direction * attackDistance, Color.red, 1.0f);*/
        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            if (doesGatherResources && hit.collider.TryGetComponent(out Resource resource))
            {
                resource.Gather(hit.point, hit.normal);
                //Debug.DrawLine(cam.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0)), hit.point, Color.blue, 1.0f);
            }
            else if(doesDealDamage && hit.collider.TryGetComponent(out IDamagable damagable))
            {
                //Debug.Log("SwordHit");

                damagable.TakeDamage(damage);
            }
        }
    }

}
