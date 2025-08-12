using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectInteraction : MonoBehaviour
{
    public float checkRate = 0.05f;    // 상호작용 오브젝트 체크 시간
    private float checkTimer;          // 체크 시간용 타이머
    public float maxCheckDistance;     // 최대 체크 거리
    public LayerMask layerMask;        // 충돌 체크할 레이어

    private Camera _camera;

    private IInteractable selectedItem;
    public IInteractable SelectedItem { get { return selectedItem; } }

    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        checkTimer += Time.deltaTime;
        if (checkTimer < checkRate) return;
        checkTimer -= checkRate; // 좀 더 정확한 시간 간격이 되게끔

        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        Debug.DrawRay(ray.origin, ray.direction * maxCheckDistance, Color.red, 0.5f); // 레이캐스트 시각화
        RaycastHit hit;
        // 이렇게 지정도 가능 시작위치, 방향
        //Physics.Raycast(new Vector3(), Vector3.back, out hit, 1000f, layerMask);
        if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
        {
            // 오브젝트 패널 활성화
            hit.collider.GetComponent<Object>()?.SetActive(true);

            // 아이템 얻기 패널 활성화
            selectedItem = hit.collider.GetComponent<IInteractable>();
            if (selectedItem != null) UIManager.Instance.SetGainItemPanelActive(true);
        }
        else
        {
            // 아이템 얻기 패널 비활성화
            selectedItem = null;
            UIManager.Instance.SetGainItemPanelActive(false);
        }
    }

    
}
