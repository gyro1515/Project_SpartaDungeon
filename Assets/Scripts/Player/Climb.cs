using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climb : MonoBehaviour
{
    [Header("클라이밍 설정")]
    [SerializeField] float climbSpeed = 2f; // 클라이밍 속도
    [SerializeField] Transform baseCameraContainer;
    [SerializeField] float climbJumpPower = 100f;
    [SerializeField] ForceMode foreMode = ForceMode.Impulse;
    public float ClimbSpeed { get { return climbSpeed; } }
    Player player;
    CapsuleCollider col;
    Rigidbody playerRb;
    LayerMask climbableLayer; // 클라이밍 가능한 레이어
    Transform playerTransform;
    private void Awake()
    {
        player = GetComponent<Player>();
        col = GetComponent<CapsuleCollider>();
        playerRb = player.GetComponent<Rigidbody>();
        climbableLayer = LayerMask.GetMask("Wall");
        playerTransform = player.gameObject.transform; // 플레이어 트랜스폼 캐싱

    }
    private void Update()
    {
        if (!player.IsClimbing) return; // 벽타기 상태가 아닐 경우 업데이트 중지

        // 벽타기 중 플레이어 앞에 벽이 있는지 확인
        if (RayToWall(playerTransform.position, out RaycastHit hit))
        {
            // 플레이어 위쪽에 벽이 있으면 벽타기 계속
            if (RayToWall(playerTransform.position + playerTransform.up * col.height / 2, out RaycastHit hit2)) return;
            Debug.DrawLine(playerTransform.position + playerTransform.up * col.height / 2, 
                playerTransform.position + playerTransform.up * col.height / 2 + playerTransform.forward * (col.radius + 0.1f), 
                Color.green, 1.0f);
            //Debug.Log("벽 위로 점프");

            FinishClimb();

            // 플레이어 위치에는 벽이 있고 위쪽에 벽이 없다면 점프하여 벽 위로 올라가기
            playerRb.AddForce((playerTransform.up + playerTransform.forward) * col.height * climbJumpPower, foreMode);
        }
        else
        {
            //Debug.Log("벽이 없습니다. 벽타기 중지.");
            // 벽이 없으면 벽타기 중지
            FinishClimb();
        }

    }
    public bool CheckWallAndClimb()
    {
        // 이미 벽타기 상태라면 벽타기 중지
        if(player.IsClimbing)
        {
            FinishClimb();
            return true; // 벽타기 중지 후 점프 안하도록
        }

        // 플레이어 위치(캡슐 중간위치)에서 레이캐스트로 벽타기 가능한지 확인
        if (!RayToWall(playerTransform.position, out RaycastHit hit)) return false; // 불가능하면 false 반환

        playerTransform.position = hit.point + hit.normal * (col.radius); // 플레이어 위치를 벽에 맞춰 조정
        baseCameraContainer.rotation = Quaternion.Euler(baseCameraContainer.eulerAngles.x, playerTransform.eulerAngles.y, 0f); // 카메라는 원래 바라보는 방향 유지

        playerTransform.rotation = Quaternion.LookRotation(-hit.normal); // 플레이어가 벽을 바라보도록 회전
        player.IsClimbing = true; // 벽타기 상태로 변경
        playerRb.useGravity = false; // 중력 비활성화
        playerRb.velocity = Vector3.zero; // 벽타기 시작 시 속도 초기화
        //Debug.Log("벽타기 시작");
        return true;
    }
    void FinishClimb()
    {
        player.IsClimbing = false; // 벽타기 상태 해제
        playerRb.useGravity = true; // 중력 활성화
        playerRb.velocity = Vector3.zero;
        // 바라보는 방향으로 플레이어 회전
        playerTransform.rotation = Quaternion.Euler(0, baseCameraContainer.eulerAngles.y, 0f);
        // 카메라 회전 초기화
        baseCameraContainer.localEulerAngles = new Vector3(baseCameraContainer.localEulerAngles.x, 0f, 0f);
    }
    bool RayToWall(Vector3 startRayPos, out RaycastHit hit)
    {
        // 플레이어 앞쪽에 Ray를 쏴서 벽 충돌 여부 확인
        // 충돌한 게 없으면 false
        if (!Physics.Raycast(startRayPos, playerTransform.forward, out hit, col.radius + 0.1f)) return false;
        // 충돌한 게 있지만 레이어가 벽이 아니라면 false
        if ((1 << hit.collider.gameObject.layer & climbableLayer) == 0) return false;

        return true;
    }
}
