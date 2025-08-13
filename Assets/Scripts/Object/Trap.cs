using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

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
    private void FixedUpdate()
    {
        checkRateTimer += Time.fixedDeltaTime;
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
        //Debug.Log($"{triggerTimer} -> 트리거");
        triggerTimer = 0f;

        // 플레이어 무브 뜯어 고쳐서, 아래 방식이면 충분함
        Vector3 dir = Vector3.right + Vector3.up;
        hit.rigidbody?.AddForce(dir * 10f, ForceMode.VelocityChange);

        /*Vector3 starTrigerPos = hit.transform.position;
        starTrigerPos.y += 0.1f; // 살짝 띄워서 출발, 바닥에 붙어 있으면 제대로 AddForce 안됨
        hit.transform.position = starTrigerPos;
        hit.rigidbody?.AddForce(hit.transform.forward * 15f + Vector3.up * 10f, ForceMode.VelocityChange);*/
        //StartCoroutine(AddForceNextFrame(hit));

    }
    IEnumerator AddForceNextFrame(RaycastHit hit)
    {
        /*Vector3 starTrigerPos = hit.transform.position;
        starTrigerPos.y += 0.1f; // 살짝 띄워서 출발, 바닥에 붙어 있으면 제대로 AddForce 안됨*/
        hit.rigidbody.useGravity = false;
        hit.rigidbody.velocity = Vector3.up / Time.fixedDeltaTime;
        //hit.transform.position = starTrigerPos;

        yield return new WaitForFixedUpdate();
        //yield return new WaitForSeconds(0.1f);
        hit.rigidbody.useGravity = true;
        hit.rigidbody.velocity = Vector3.zero;

        Vector3 dir = Vector3.right + Vector3.up;
        hit.rigidbody?.AddForce(dir * 10f, ForceMode.VelocityChange);

        //hit.rigidbody?.AddForce(hit.transform.forward * 15f + Vector3.up * 10f, ForceMode.VelocityChange);
        //hit.rigidbody?.AddForce(Vector3.forward * 15f + Vector3.up * 10f, ForceMode.VelocityChange);
        //Debug.Log(hit.transform.forward);
        //Vector3 dir = hit.transform.forward + Vector3.up;

    }
}
