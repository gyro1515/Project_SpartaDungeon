using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [Header("움직이는 범위 설정")]
    [SerializeField] Vector3 startPosition; // 시작 위치
    [SerializeField] Vector3 endPosition; // 끝 위치
    [SerializeField] float speed = 1f; // 이동 속도
    float timeToMove; // 속도와 이동 거리를 기반으로 계산된 이동 시간
    float timer = 0f; // 타이머
    //Rigidbody _rigidbody; 
    //HashSet<GameObject> objects = new HashSet<GameObject>();
    bool canMove = true; // 이동 가능 여부
    float moveTimer = 0f;
    Vector3 prePos = Vector3.zero;
    float preTime = 0f; // 이전 시간
    private void Awake()
    {
        //_rigidbody = GetComponent<Rigidbody>();
        float distance = Vector3.Distance(startPosition, endPosition); // 시작 위치와 끝 위치 사이의 거리 계산
        timeToMove = distance / speed; // 이동 시간 계산
    }
    private void FixedUpdate()
    {
        if (!canMove) return;
        
        prePos = transform.position;
        preTime = timer; // 이전 시간 저장

        timer += Time.fixedDeltaTime; // 타이머 업데이트
        transform.position = Vector3.Lerp(startPosition, endPosition, Mathf.PingPong(timer / timeToMove, 1)); // 이건 플레이어 깔림 현상 발생
    }
    private void Update()
    {
        if (canMove) return;

        moveTimer += Time.deltaTime;
        if (moveTimer < 0.1f) return;
        // 0.1초 이내에는 움직이지 않도록 함
        moveTimer = 0f; // 타이머 초기화
        canMove = true; // 이동 가능 상태로 변경
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        if (!collision.gameObject.CompareTag("Player")) return;
        bool isOnObject = false;
        for (int i = 0; i < collision.contactCount; i++)
        {
            if (Vector3.Dot(-collision.contacts[i].normal, Vector3.up) < 0.8f) 
            {
                // 깔림 현상 방지: 충돌 지점의 노멀 벡터가 위쪽을 향하지 않는 경우
                canMove = false;
                moveTimer = 0f;
                transform.position = prePos;
                timer = preTime;
                continue; 
            }

            //Debug.Log($"충돌 지점 노멀 {i}: {collision.contacts[i].normal}");
            isOnObject = true;
        }
        if (!isOnObject) return;
        collision.gameObject.transform.SetParent(gameObject.transform);
    }
    private void OnCollisionExit(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        //Debug.Log("충돌 종료");
        collision.gameObject.transform.SetParent(null);
        //canMove = true;
        /*if (objects.Contains(collision.gameObject))
        {
            objects.Remove(collision.gameObject);
        }*/
    }
}
