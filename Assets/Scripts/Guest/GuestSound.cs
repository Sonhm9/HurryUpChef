using System.Collections;
using UnityEngine;

public class GuestSound : MonoBehaviour
{
    // 성별 선택
    public enum Gender
    {
        Male,
        Female
    }
    public Gender gender;

    Coroutine throatRoutine;

    // 불만족할때 나는 소리 메서드
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

    // 만족할때 나는 소리 메서드
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

    // 기다릴때 내는 소리 메서드
    public void Throat()
    {
        throatRoutine = StartCoroutine(ThroatRoutine());
    }

    // 헛기침 루틴을 그만하는 메서드
    public void StopThroat()
    {
        if(throatRoutine != null)
        {
            StopCoroutine(throatRoutine);
            throatRoutine = null;
        }
    }

    // 헛기침 루틴 메서드
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
