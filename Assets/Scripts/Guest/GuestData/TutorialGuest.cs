using UnityEngine;
[CreateAssetMenu(fileName = "TutorialGuest", menuName = "Scriptable Objects/TutorialGuest")]
public class TutorialGuest : Guest
{
    public override void PerformAction()
    {
        // �⺻ �մ��̹Ƿ�, Ư�� �ൿ ����
        Debug.Log("Tutorial");
    }

    public override bool ShouldPerformAction(string currentState)
    {
        return currentState == "OrderState";
    }
}
