using UnityEngine;

public class TextShaker : MonoBehaviour
{
    private float rotationSpeed = 2f; // 회전 속도 조정
    private float rotationAngle = 15f; // 최대 회전 각도

    private void Update()
    {
        // 시간에 따라 사인파로 각도 계산
        float rotationZ = Mathf.Sin(Time.time * rotationSpeed) * rotationAngle;

        // Z축 회전 적용
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }
}
