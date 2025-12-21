using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{
    [Header("Cinemachine Camera")]
    public CinemachineCamera cinCam;

    [Header("Movement Settings")]
    public float moveSpeed;
    public float zoomSpeed;
    public float rotateSpeed;
    public float minZoom;
    public float maxZoom;

    [Header("Limits")]
    public bool enableLimits = false;
    public Vector2 minLimit;
    public Vector2 maxLimit;

    void Update() //60fps 면 1초에 60번 호출
    {
        HandleMovement();
        HandleZoom();
    }

    void HandleMovement()
    {
        Vector3 moveDir = Vector3.zero; //매 프레임마다 초기화
        Vector3 moveRotate = Vector3.zero;

        // New Input System 사용
        if (Keyboard.current != null)
        {
            if(Keyboard.current.qKey.isPressed) moveRotate.y += 1f;
            if(Keyboard.current.eKey.isPressed) moveRotate.y -= 1f;

            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveDir.z += 1f;
            //z축 앞뒤 이동을 얘기한다.
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveDir.z -= 1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveDir.x -= 1f;
            //x축 좌우 이동을 얘기한다.
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveDir.x += 1f;
        } // 줌인 아웃으로 y축 이동 조절

        // 대각선 이동 시 속도 증가 방지
        moveDir.Normalize();
        
        // Space.Self는 Inspector 창의 초기값을 기준으로 회전/이동함.
        // 월드 좌표 기준으로 이동 (카메라가 회전해 있어도 x, z 축으로 이동)
        transform.Rotate(moveRotate * rotateSpeed * Time.deltaTime, Space.World);

        // 카메라의 Y축 회전(바라보는 방향)만 반영하고, X축(기울기)은 무시하여 평면 이동 구현
        Vector3 forward = transform.forward; //실시간으로 변하는 앞뒤방향 벡터
        Vector3 right = transform.right;     //실시간으로 변하는 좌우방향 벡터

        forward.y = 0f; //위아래 방향 제거
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 targetDir = (forward * moveDir.z) + (right * moveDir.x);
        transform.Translate(targetDir * moveSpeed * Time.deltaTime, Space.World);

        if (enableLimits)
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.Clamp(pos.x, minLimit.x, maxLimit.x);
            pos.z = Mathf.Clamp(pos.z, minLimit.y, maxLimit.y);
            transform.position = pos;
        }
        
    }

    void HandleZoom()
    {
        float scroll = 0f; // 지역 변수 초기화 해야됨.


        // New Input System 사용
        if (Mouse.current != null)
        {
            // 마우스 휠 값 읽기 (보통 120 단위이므로 0.01을 곱해 기존 감도와 비슷하게 맞춤)
            scroll = Mouse.current.scroll.ReadValue().y * 0.01f;
        }

        if (scroll != 0)
        {
            //Debug.Log("Scroll Value: " + scroll);
            float zoom = cinCam.Lens.OrthographicSize;
            // Y축 높이를 조절하여 줌인/줌아웃 구현 (Orthographic 카메라 기준)
            zoom -= scroll * zoomSpeed * 100f * Time.deltaTime; 
            zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
            cinCam.Lens.OrthographicSize = zoom;
        }
    }
}
