using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem; 
using Unity.Cinemachine;

public class PlayerController : MonoBehaviour
{
    public NavMeshAgent agent;
    public LineRenderer lineRenderer; // [추가] 원 그리는 도구\

    private Quaternion savedTpsRotation;
    
    [Header("Cameras")]
    public CinemachineCamera tpsCam; // 3인칭 카메라
    public CinemachineCamera fpsCam; // 1인칭 카메라
    
    [Header("Settings")]
    public float maxMoveDistance = 3.0f; // [추가] 1턴에 이동 가능한 최대 거리 (5미터)
    public int circleSegments = 50;      // [추가] 원을 얼마나 부드럽게 그릴지 (점 개수)
    private bool isSniperMode = false; // 현재 저격 모드인지 체크

    [Header("Movement AP")]
    public int maxAp = 2;
    public int currentAp;

    void Start()
    {
        lineRenderer.loop = true; 
        DrawMoveCircle();
        currentAp = maxAp; 
    }
    void Update()
    {
        // 1. 스페이스바(Space)를 누르면 모드 전환 (Toggle)
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ToggleCameraMode();
        }

        if(Keyboard.current.enterKey.wasPressedThisFrame)
        {
            EndTurn();
        }

        // 2. 이동 (저격 모드가 아닐 때만 이동 가능하게 하기)
        if (!isSniperMode && Mouse.current.rightButton.wasPressedThisFrame && currentAp > 0) //마우스 클릭을 할떄만 반응하기
        {
            MoveToClickPoint();
        }
    }

    void ToggleCameraMode()
    {
        isSniperMode = !isSniperMode; // 상태 뒤집기

        if (isSniperMode)
        {
            // [추가된 부분] ---------------------------------------
            // 1. 현재 메인 카메라가 바라보는 방향을 가져옵니다.
            Vector3 cameraForward = Camera.main.transform.forward; //월드 공간 방향 벡터
            Debug.Log("Camera Forward: " + cameraForward);
            //로컬 좌표계의 foward값은 항상 (0,0,1)이다. 
            
            // 2. 위/아래(Y축) 기울기는 무시하고 수평 방향만 남깁니다. (사람이 뒤로 눕지 않게)
            cameraForward.y = 0; 

            // 여기서 transform은 Player 오브젝트의 Transform입니다.
            
            transform.rotation = Quaternion.LookRotation(cameraForward);
            //cameraFoward 방향 벡터로 player 몸통이 매 프레임 회전함.
            
            ResetFPSCameraLook();

            // 저격 모드 ON
            fpsCam.Priority = 20;
            tpsCam.Priority = 10;
        }
        else
        {
            ResetFPSCameraLook();   // 저격 모드 OFF
            tpsCam.Priority = 20;
            fpsCam.Priority = 10;
        }
    }

    void UseAp(int amount) //행동력을 한번에 많이 소모하는 경우도 있을 수 있으니까
    {
        currentAp -= amount;
        
        if (currentAp <= 0)
        {
          Debug.Log("AP가 모두 소진되었습니다.");  
        } 
    }
    void EndTurn()
    {
        currentAp = maxAp; // AP 초기화
        Debug.Log("턴 종료! AP가 초기화되었습니다.");
    }


    void DrawMoveCircle() //월드 기준이 아니라 Player 기준으로 원 생성이라 한번만 계산해도됨.
    {
        if (lineRenderer == null) return;

        lineRenderer.positionCount = circleSegments; // 점 개수 설정
        float angle = 0f;

        for (int i = 0; i < circleSegments; i++) //1프레임당 circleSegments만큼 반복
        {
            // 수학 공식: 원의 좌표 (x = r * cos, z = r * sin)
            // 플레이어 몸체 기준(Local)으로 그립니다.
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * maxMoveDistance;
            float z = Mathf.Cos(Mathf.Deg2Rad * angle) * maxMoveDistance;

            lineRenderer.SetPosition(i, new Vector3(x, -1, z));

            angle += (360f / circleSegments); //7의 배수로 각도 증가
        }
    }

    void MoveToClickPoint()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) //광선 발사 마우스 지점까지 충돌체 확인
        {
            // [거리 계산] 클릭한 곳과 내 위치 사이의 거리
            float distance = Vector3.Distance(transform.position, hit.point);

            // [판단] 거리가 최대 이동 거리보다 작을 때만 이동!
            if (distance <= maxMoveDistance)
            {
                agent.SetDestination(hit.point); //NavMeshAgent가 목적지 설정
                UseAp(1); 
            }
            else
            {
                Debug.Log("너무 멀어서 못 가요!"); // (나중에 UI로 경고 띄우면 됨)
            }
        }
    }


    void ResetFPSCameraLook()
    {
        // 1. FPS 카메라에 붙어있는 'CinemachinePanTilt' 컴포넌트를 가져옵니다.
        // (Unity 6 / Cinemachine 3.0에서 'Aim'을 Pan Tilt로 설정하면 이 컴포넌트가 붙어있습니다.)
        var panTilt = fpsCam.GetComponent<CinemachinePanTilt>();

        if (panTilt != null)
        {
            // 2. 가로(X), 세로(Y) 축의 누적된 회전값(Value)을 0으로 초기화합니다.
            panTilt.PanAxis.Value = 0f;
            panTilt.TiltAxis.Value = 0f;
        }
        else
        {
            // 혹시 PanTilt가 아니라 다른 거라면(예: POV), CinemachinePOV로 찾아야 할 수도 있습니다.
            Debug.LogWarning("FPS 카메라에 CinemachinePanTilt 컴포넌트가 없습니다!");
        }
    }
}