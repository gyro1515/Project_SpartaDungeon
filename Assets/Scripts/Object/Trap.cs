using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [Header("함정 세팅")]
    [SerializeField] Transform startTransform; 
    [SerializeField] Transform endTransform;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float checkRate = 0.05f; // 함정 체크 시간
    [SerializeField] float triggerTime = 0.5f; // 함정 발동 시간 
    float checkRateTimer = 0f; // 체크 시간용 타이머
    float triggerTimer = 0f; // 체크 시간용 타이머
    float distance;
   

    private void Awake()
    {
        distance = Vector3.Distance(startTransform.position, endTransform.position);
    }
    private void Update()
    {
        checkRateTimer += Time.deltaTime;
        if (checkRateTimer < checkRate) return;
        checkRateTimer -= checkRate; // 좀 더 정확한 시간 간격이 되게끔

        Ray ray = new Ray(startTransform.position, endTransform.position - startTransform.position);
        if (!Physics.Raycast(ray, out RaycastHit hit, distance, layerMask))
        {
            startTransform.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
            endTransform.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;

            return; // 충돌 없으면 리턴
        }
        //if (!hit.collider.CompareTag("Player")) return; // 충돌한 물체가 플레이어가 아니면 리턴
        startTransform.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        endTransform.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;

        // 해당 시간 되면 트리거 발동하도록
        triggerTimer += checkRate;
        if (triggerTimer < triggerTime) return;
        Debug.Log($"{triggerTimer} -> 트리거");
        triggerTimer = 0f;
        hit.rigidbody?.AddForce(Vector3.up * 15f, ForceMode.VelocityChange);
    }
}
