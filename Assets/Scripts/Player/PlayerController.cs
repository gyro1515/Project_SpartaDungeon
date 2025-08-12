using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerController : BaseController, IJumpable
{
    [Header("카메라 세팅")]
    [SerializeField] Transform cameraContainerFirst;
    //[SerializeField] Transform cameraContainerThird;
    [SerializeField] float minXLook;  // 최소 시야각
    [SerializeField] float maxXLook;  // 최대 시야각
    [SerializeField] float lookSensitivity; // 카메라 민감도

    float camCurXRot; // 현재 카메라 x축(피치) 회전 값
    private Vector3 curMovementInput;  // 현재 입력 값 -> 게임 축과 똑같게 Vector3로 바꿈
    private Vector2 mouseDelta;  // 마우스 변화
    bool canLook = true; // 인벤토리 온/오프 시 카메라 회전 용도
    Camera cam;

    Player player;
    PlayerStateController stateController;
    CapsuleCollider col;
    Action inventory;
    ObjectInteraction objectInteraction;
    public ObjectInteraction ObjectInteraction { get { return objectInteraction; } }
    PerspectiveShift perspectiveShift;

    // 인풋 -> 인스펙터 창보다에서 연결하는게 생각보다 더 귀찮아서...
    private PlayerInput playerInput;  
    private InputActionMap mainActionMap; 
    private InputAction moveAction; 
    private InputAction LookAction; 
    private InputAction jumpAction;
    private InputAction inventoryAction;
    private InputAction interactionAction;
    private InputAction dashAction;
    private InputAction PerspectiveShiftAction;

    // IJumpable 상속
    public float JumpPower { get { return player?.JumpPower ?? 0f; } set {  if(player) player.JumpPower = value; } }
    public bool IsJump { get; set; } = false;
    public int JumpCount { get; set; }
    public int CurJumpCount { get; set; } // 현재 점프 가능 횟수

    // 점프 채크용 위치
    Vector3 jumpCheckPos = Vector3.zero;
    private float jumpJudgeTime = 0.1f; // 점프하자마자 collisionStay호출되는 이슈때문에 점프 판정 시간 두기
    private float lastJumpTime = -999f;
    // 대시시 캐릭터 색 변경용
    MeshRenderer meshRenderer;
    protected override void Awake()
    {
        base.Awake();
        Cursor.lockState = CursorLockMode.Locked; // 마우스 가운데로 잠그고 안보이게 하기
        
        player = GetComponent<Player>();
        stateController = GetComponent<PlayerStateController>();
        col = GetComponent<CapsuleCollider>();
        // 점프 체크용 포지션, 캡슐 콜라이더는 아래가 둥글기 때문에 경사가 있는 길도 올라 갈수 있다.
        // 따라서 점프하고 착지한 바닥이 플레이어 위치 바로 아래가 아닐 가능성이 있기때문에 jumpCheckPos로 보정을 했다.
        // -> 캡슐 콜라이더 아래 둥근 부분에 Terrain에 해당하는 무언가가 충돌한다면 착지로 전환 -> IsJump = false;
        jumpCheckPos = Vector3.down * (col.height / 2 - col.radius );

        playerInput = GetComponent<PlayerInput>();
        mainActionMap = playerInput.actions.FindActionMap("Player");
        moveAction = mainActionMap.FindAction("Move");
        moveAction.performed += OnMove; // 해당 키가 작동 중이라면
        //moveAction.canceled += OnMoveStop; // 이건 필요 없음
        LookAction = mainActionMap.FindAction("Look");
        LookAction.performed += OnLook;
        LookAction.canceled += OnLookStop;
        jumpAction = mainActionMap.FindAction("Jump");
        jumpAction.started += OnJump; // 키가 눌렸을 때
        inventoryAction = mainActionMap.FindAction("Inventory");
        inventoryAction.started += OnInventory;
        interactionAction = mainActionMap.FindAction("Interaction");
        interactionAction.started += OnInteraction;
        dashAction = mainActionMap.FindAction("Dash");
        dashAction.started += OnDash;
        dashAction.canceled += OnFinishDash;
        PerspectiveShiftAction = mainActionMap.FindAction("PerspectiveShift");
        PerspectiveShiftAction.started += OnPerspectiveShift;

        objectInteraction = GetComponent<ObjectInteraction>();

        JumpCount = 1; // 기본 점프 횟수 1회
        CurJumpCount = JumpCount;

        meshRenderer = GetComponentInChildren<MeshRenderer>();
        cam = Camera.main; // 카메라 컴포넌트 가져오기
        perspectiveShift = GetComponent<PerspectiveShift>();
    }
    private void Start()
    {
        // Action 호출 시 필요한 함수 등록
        inventory += UIManager.Instance.InventoryToggle; // inventory 키 입력 시
    }
    /*private void Update()
    {
        
    }*/
    private void FixedUpdate()
    {
        Move();
    }
    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        CheckLanding(collision);
    }
    private void OnCollisionStay(Collision collision)
    {
        CheckLanding(collision);
    }
    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainerFirst.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }
    void OnMove(InputAction.CallbackContext context)
    {
        curMovementInput = context.ReadValue<Vector3>();
    }
    /*void OnMoveStop(InputAction.CallbackContext context)
    {
        Debug.Log("OnMoveStop");
        curMovementInput = Vector3.zero;
    }*/
    void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }
    void OnLookStop(InputAction.CallbackContext context)
    {
        mouseDelta = Vector2.zero;
    }
    void OnJump(InputAction.CallbackContext context)
    {
        StartJump();
    }
    void OnInventory(InputAction.CallbackContext context)
    {
        inventory?.Invoke(); // 인벤토리 열기/닫기
        ToggleCursor(); // 인벤토리 열면 카메라 회전 안되게
    }
    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
    void OnInteraction(InputAction.CallbackContext context)
    {
        if (objectInteraction.SelectedItem == null) return; // 선택된 아이템이 없다면 리턴
        objectInteraction.SelectedItem.OnInteract(); // 선택된 아이템의 OnInteract 호출
    }
    void OnDash(InputAction.CallbackContext context)
    {
        if (player.CurStemina <= 0) return; // 스테미나가 없다면 리턴
        //Debug.Log("대쉬 시작");
        player.IsDashing = true; // 대쉬 시작
        meshRenderer.material.color = Color.red; // 대쉬 중일 때 색상 변경

    }
    void OnFinishDash(InputAction.CallbackContext context)
    {
        //Debug.Log("대쉬 끝");
        player.IsDashing = false;
        meshRenderer.material.color = Color.white; // 대쉬 중일 때 색상 변경

    }
    void OnPerspectiveShift(InputAction.CallbackContext context)
    {
        perspectiveShift.ChangePerspective();
    }
    void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.z + transform.right * curMovementInput.x; // 실제 축처럼 z가 앞을 향하도록
        dir *= player.IsDashing? player.RunSpeed : player.WalkSpeed;  
        dir.y = _rigidbody.velocity.y;  // y값은 velocity(변화량)의 y 값을 넣어준다.

        _rigidbody.velocity = dir;
    }

    public void StartJump() // IJumpable 상속
    {
        if (CurJumpCount <= 0) return; // 점프 가능 횟수가 없다면 리턴
        if(player.CurStemina <= 0) return; // 스테미나가 없다면 리턴
        IsJump = true;
        CurJumpCount--; // 점프 가능 횟수 감소
        _rigidbody.AddForce(Vector2.up * JumpPower, ForceMode.Impulse);
        lastJumpTime = Time.time;
        
        stateController.AddStemina(-player.JumpStemina); // 점프 시 스테미나 감소
    }

    public void EndJump() // IJumpable 상속
    {
        IsJump = false;
        CurJumpCount = JumpCount; // 점프 가능 횟수 초기화
    }

    void CheckLanding(Collision collision)
    {
        if(!IsJump) return; // 점프시에만 체크
        if (Time.time - lastJumpTime < jumpJudgeTime) return; // 점프 판정 시간이 안 지났다면 리턴

        if ((1 << collision.gameObject.layer & LayerMask.GetMask("Terrain")) == 0) return; // 터레인이 아니면 리턴

        // 점프 종료 체크
        ContactPoint[] contacts = new ContactPoint[collision.contactCount];
        collision.GetContacts(contacts);

        foreach (var contact in contacts)
        {
            // 충돌한 위치 - 플레이어 점프 체크 위치
            Vector3 chekV = contact.point - (gameObject.transform.position + jumpCheckPos);
            // chekV와 내적하여 값 0 초과 체크 -> 충돌한 위치가 transform.position + jumpCheckPos보다 아래에 있는가
            if (Vector3.Dot(chekV, Vector3.down) > 0f)
            {
                //Debug.Log($"{Vector3.Dot(chekV, Vector3.down)} / {Vector3.Distance(contact.point, transform.position + jumpCheckPos)}");
                //Debug.DrawLine(gameObject.transform.position + jumpCheckPos, contact.point, Color.red, 1.0f);
                EndJump();
                return;
            }
        }
    }
}
