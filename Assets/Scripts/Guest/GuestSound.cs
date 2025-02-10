using System.Collections;
using UnityEngine;

public class GuestSound : MonoBehaviour
{
    // ���� ����
    public enum Gender
    {
        Male,
        Female
    }
    public Gender gender;

    Coroutine throatRoutine;

    // �Ҹ����Ҷ� ���� �Ҹ� �޼���
    public void Nod()
    {
        if(gender == Gender.Male)
        {
            SoundManager.Instance.PlaySfx(SFX.MaleNod);
        }
        else
        {
            SoundManager.Instance.PlaySfx(SFX.FemaleNod);
        }
    }

    // �����Ҷ� ���� �Ҹ� �޼���
    public void Satisfacton()
    {
        if (gender == Gender.Male)
        {
            SoundManager.Instance.PlaySfx(SFX.MaleSatisfaction);
        }
        else
        {
            SoundManager.Instance.PlaySfx(SFX.FemaleSatisfaction);
        }
    }

    // ��ٸ��� ���� �Ҹ� �޼���
    public void Throat()
    {
        throatRoutine = StartCoroutine(ThroatRoutine());
    }

    // ���ħ ��ƾ�� �׸��ϴ� �޼���
    public void StopThroat()
    {
        if(throatRoutine != null)
        {
            StopCoroutine(throatRoutine);
            throatRoutine = null;
        }
    }

    // ���ħ ��ƾ �޼���
    IEnumerator ThroatRoutine()
    {
        yield return new WaitForSeconds(15);
        if (gender == Gender.Male)
        {
            SoundManager.Instance.PlaySfx(SFX.MaleThroat);
        }
        else
        {
            SoundManager.Instance.PlaySfx(SFX.FemaleThroat);
        }
    }
}
