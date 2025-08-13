# 🕹️ Unity 프로젝트 - SpartaDungeon

> **다양한 플레이어 조작, 상호작용, 환경 요소를 구현한 3D 액션 어드벤처 스타일 프로젝트**  
> Unity 엔진과 C#을 활용하여 기본 이동, 시점 전환, 다양한 오브젝트와의 상호작용, 아이템 시스템 등을 구현했습니다.

---

## 📅 개발 기간
- **2025.08.08 ~ 2025.08.13** (주말 제외 **4일**)

---

## 🎮 조작법

| 동작             | 키/마우스 |
|------------------|-----------|
| 이동             | **W, A, S, D** |
| 대쉬             | **왼쪽 Shift** |
| 점프 / 벽타기    | **Spacebar** |
| 인벤토리 열기    | **Tab** |
| 상호작용         | **E** |
| 시점 전환        | **1** |
| 카메라 회전      | **마우스 이동** |

---

## ✨ 구현 기능

### 1. 기본 이동 및 점프
- WASD 이동, Spacebar 점프 구현
- 대쉬 기능(Shift)과 연계된 가속 이동

### 2. 플레이어 상태바 UI
- 체력, 스태미나, 버프 지속시간 등 실시간 UI 표시

### 3. 동적 환경 조사 - 상호작용 가능한 오브젝트 표시
- **Raycast**로 플레이어 시선 방향에 있는 오브젝트 탐지
- UI에 오브젝트 이름, 설명 등 표시
- 상호작용 가능한 오브젝트에 마우스를 올리면 해당 오브젝트에 상호작용 가능 UI 표시

### 4. 점프대
- 밟으면 캐릭터가 위로 높게 튀어 오르는 기믹 구현

### 5. 다양한 아이템 구현
- **ScriptableObject**로 아이템 데이터 관리
- 이름, 설명, 속성, 효과 등 개별 정의 가능

### 6. 아이템 사용 시스템
- 특정 아이템 사용 시 효과 지속 (무적, 더블 점프, 스피드 부스트 등)
- 우측 상단에 남은 시간을 아이콘으로 UI에 표시

### 7. 시점 전환
- **1인칭 ↔ 3인칭** 시점 전환 기능 구현

### 8. 움직이는 플랫폼
- 시간에 따라 움직이는 발판
- 플레이어가 자연스럽게 따라가도록 물리 처리

### 9. 벽 타기 및 매달리기
- 벽을 오르거나 매달리는 액션 구현

### 10. 레이저 트랩 (플랫폼 발사기)
- Raycast로 특정 구간 감시
- 플레이어가 일정 시간 닿으면 특정 방향으로 발사

---

## 🛠️ 트러블 슈팅 - 플레이어에 AddForce 적용

### 문제 상황
- 플레이어에 'AddForce' 적용시, **X, Z 축 방향 힘이 적용되지 않는** 문제가 발생했습니다. 
- 플레이어 이동을 `Rigidbody.velocity`로 제어하는 바람에 생긴 문제였습니다.
- 매 프레임 이동 입력 시, velocity를 덮어쓰는 방식 때문에 외부 힘이 무시되는 현상이었습니다.

### 해결 방법
- 기존의 이동 제어 방식을 변경하여, 입력이 있을 때만 `AddForce`로 이동을 처리했습니다.
- 입력이 없으면 `OnMoveStop()`으로 강제로 이동을 멈추도록 구현했습니다.
- 벽에서 자연스럽게 떨어지지 않는 문제를 해결하기 위해 Physic Material의 마찰을 **0**으로 설정했습니다.  
  이로 인해 미끄러짐을 방지하기 위해 강제 정지 로직이 필요했습니다.

```csharp
// FixedUpdate()부분
void Move()
{
    if (curMovementInput == Vector3.zero) return; // 입력 없으면 작동 안하도록

    Vector3 dir = Vector3.zero;
    Vector3 velocityChange = Vector3.zero;
    // 플레이어 상태에 따라 이동 방향과 속도 변경
    if (player.IsClimbing)
    {
        dir = transform.up * curMovementInput.z + transform.right * curMovementInput.x;
        dir = dir.normalized;
        
        float climbRunSpeed = player.WalkSpeed / player.RunSpeed * climb.ClimbSpeed;
        dir *= player.IsDashing ? climbRunSpeed : climb.ClimbSpeed;
        
        velocityChange = dir - _rigidbody.velocity;
    }
    else
    {
        dir = transform.forward * curMovementInput.z + transform.right * curMovementInput.x;
        dir = dir.normalized;
        dir *= player.IsDashing ? player.RunSpeed : player.WalkSpeed;

        velocityChange = dir - new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
    }

    _rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
}

// 인풋액션 부분
void OnMove(InputAction.CallbackContext context)
{
    curMovementInput = context.ReadValue<Vector3>();
    if (curMovementInput.magnitude > 0.002f) return;
    // 입력 없다면 정지
    OnMoveStop();
}
void OnMoveStop()
{
    curMovementInput = Vector3.zero;
    _rigidbody.velocity = new Vector3(0f, _rigidbody.velocity.y, 0f)
}
```

### 결과
- 외부 힘(트랩)에 자연스럽게 반응할 수 있습니다.
- 벽이나 오브젝트에 끼이지 않고 잘 미끄러지도록 구현했습니다.
- 플레이어 조작과 물리 상호작용 모두 자연스러운 상태 유지할 수 있게 되었습니다.

## ⚙️ 기술 스택
- Engine: Unity 2022.3.17f1
- Language: C#
- Version Control:GitHub
