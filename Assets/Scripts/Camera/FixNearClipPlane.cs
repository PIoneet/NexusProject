using UnityEngine;
using Unity.Cinemachine;

[ExecuteInEditMode]
public class FixNearClipPlane : MonoBehaviour
{
    public float targetNearClip = -10f;
    private CinemachineCamera cineCam;

    void OnEnable()
    {
        cineCam = GetComponent<CinemachineCamera>();
    }

    void LateUpdate()
    {
        if (cineCam != null)
        {
            // 렌즈 설정 가져오기
            var lens = cineCam.Lens;

            // 값이 다르면 강제로 고정
            if (Mathf.Abs(lens.NearClipPlane - targetNearClip) > 0.001f)
            {
                lens.NearClipPlane = targetNearClip;
                cineCam.Lens = lens;
            }
        }
    }
}
