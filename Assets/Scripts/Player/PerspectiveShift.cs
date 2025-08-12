using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveShift : MonoBehaviour
{
    enum ChangeState
    {
        FirstPerson, // 1인칭
        ThirdPerson,  // 3인칭
        FirstToThird, // 1인칭에서 3인칭으로
        ThirdToFirst  // 3인칭에서 1인칭으로
    }
    ChangeState state = ChangeState.FirstPerson; // 초기 상태는 1인칭

    [Header("시점 트랜스폼 설정")]
    [SerializeField] Transform firstPersonTransform; // 1인칭 시점
    [SerializeField] Transform thirdPersonTransform; // 3인칭 시점
    [SerializeField] float chageTime = 0.3f; // 시점 전환 시간
    Camera cam;
    Player player;
    ObjectInteraction objectInteraction;
    float changeTimer = 0f; // 시점 전환 타이머

    private void Awake()
    {
        cam = Camera.main; //  Start()로 바꿔야 할 수도..?
        player = GetComponent<Player>();
        objectInteraction = GetComponent<ObjectInteraction>();
    }
    private void Update()
    {
        switch (state)
        {
            case ChangeState.FirstToThird:
                LerpCamPos(firstPersonTransform, thirdPersonTransform);
                break;
            case ChangeState.ThirdToFirst:
                LerpCamPos(thirdPersonTransform, firstPersonTransform);
                break;
        }
    }
    public void ChangePerspective()
    {
        player.IsFirstPerson = !player.IsFirstPerson; // 1인칭/3인칭 전환
        if (player.IsFirstPerson)
        {
            cam.cullingMask ^= LayerMask.GetMask("PlayerOnly"); // 플레이어 레이어를 카메라의 컬링 마스크에서 제거하여 플레이어가 보이지 않도록 함
            objectInteraction.maxCheckDistance -= 3.5f; // 1인칭 모드에서는 상호작용 거리 감소
        }
        else
        {
            cam.cullingMask |= LayerMask.GetMask("PlayerOnly"); // 플레이어 레이어를 카메라의 컬링 마스크에 추가하여 플레이어가 보이도록 함
            objectInteraction.maxCheckDistance += 3.5f; // 3인칭 모드에서는 상호작용 거리 증가
        }
        state = player.IsFirstPerson ? ChangeState.ThirdToFirst : ChangeState.FirstToThird; // 상태 업데이트

    }
    void LerpCamPos(Transform from, Transform to)
    {
        changeTimer += Time.deltaTime;
        cam.transform.position = Vector3.Lerp(from.position, to.position, changeTimer / chageTime);
        if (changeTimer >= chageTime)
        {
            changeTimer = 0f; // 타이머 초기화
            cam.transform.SetParent(to);
            cam.transform.localPosition = Vector3.zero; // 카메라 위치 초기화
            state = player.IsFirstPerson ? ChangeState.FirstPerson : ChangeState.ThirdPerson; // 상태 업데이트
        }
    }
}
