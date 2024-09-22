using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform Target;    
    public float SmoothSpeed = 0.125f;  // �������� � ������� ������ ����� ��������� �� �����
    public Vector3 Offset;  // �������� ������ ������������ ����
    public float MaxOrthographicSize = 50f;
    public float ScrollSpeed = 1.0f;

    void LateUpdate()
    {
        if (Target == null) return;

        Vector3 desiredPosition = Target.position + Offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, SmoothSpeed);
        smoothedPosition.z = transform.position.z;
        transform.position = smoothedPosition;

        transform.LookAt(Target); 
    }
}
