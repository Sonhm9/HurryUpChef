using UnityEngine;

public class TextShaker : MonoBehaviour
{
    private float rotationSpeed = 2f; // ȸ�� �ӵ� ����
    private float rotationAngle = 15f; // �ִ� ȸ�� ����

    private void Update()
    {
        // �ð��� ���� �����ķ� ���� ���
        float rotationZ = Mathf.Sin(Time.time * rotationSpeed) * rotationAngle;

        // Z�� ȸ�� ����
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }
}
