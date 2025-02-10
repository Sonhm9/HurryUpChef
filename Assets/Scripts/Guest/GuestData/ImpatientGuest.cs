using UnityEngine;

[CreateAssetMenu(fileName = "ImpatientGuest", menuName = "Scriptable Objects/ImpatientGuest")]
public class ImpatientGuest : Guest
{
    // �ش� �մ��� Ư�� �ൿ
    public override void PerformAction()
    {
        // �ٴ� ��� ����
        Debug.Log("Impatient");
    }

    // Ư�� �ൿ ����ñ�
    public override bool ShouldPerformAction(string currentState)
    {
        return currentState == "EatingState"; // �Դ� ����
    }
}
