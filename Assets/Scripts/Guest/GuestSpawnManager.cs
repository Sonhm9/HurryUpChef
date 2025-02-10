using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GuestSpawnManager : MonoBehaviour
{
    public Guest[] guestTypes; // 손님 타입 배열
    public float[] ratios; // 손님 타입에 따른 확률 배열
    public RecipeData[] recipeDatas; // 음식 데이터 배열
    public float[] recipeRatios; // 음식 데이터에 따른 확률 배열

    public Vector3 spawnPosition;

    [SerializeField] float span = 15f; // 등장 빈도

    float delta; // 현재 시간

    Coroutine guestSpawnRoutine;

    void Start()
    {
        guestSpawnRoutine = StartCoroutine(GuestSpawnRoutine());
    }

    // 손님 생성 루틴
    IEnumerator GuestSpawnRoutine()
    {
        while (true)
        {
            // 일정시간(빈도)가 지나면 손님 생성
            yield return new WaitForSeconds(span);

            // 랜덤으로 손님 생성
            InstantiateGuest(GetRandomGuest(guestTypes, ratios));
        }
    }

    // 손님 생산을 중단하는 메서드
    public void StopGuestSpawn()
    {
        if (guestSpawnRoutine != null)
        {
            StopCoroutine(guestSpawnRoutine);
            guestSpawnRoutine = null;
        }
    }

    public Guest GetRandomGuest(Guest[] guests, float[] ratios)
    {
        // 확률의 총합을 계산
        float totalRatio = 0;
        for (int i = 0; i < ratios.Length; i++)
        {
            totalRatio += ratios[i];
        }

        // 랜덤값 생성
        float randomValue = Random.Range(1, totalRatio + 1);

        // 확률의 범위에 따라 해당 손님 반환
        float sum = 0;
        for (int i = 0; i < ratios.Length; i++)
        {
            sum += ratios[i];
            if(randomValue < sum)
            {
                return guests[i];
            }
        }

        // 안전장치로 손님 반환
        return guests[guests.Length - 1];
    }

    public RecipeData GetRandomRecipeData(RecipeData[] recipeDatas, float[] recipeRatios)
    {
        // 확률의 총합을 계산
        float totalRatio = 0;
        for (int i = 0; i < recipeRatios.Length; i++)
        {
            totalRatio += recipeRatios[i];
        }

        // 랜덤값 생성
        float randomValue = Random.Range(1, totalRatio + 1);

        // 확률의 범위에 따라 해당 레시피 반환
        float sum = 0;
        for (int i = 0; i < recipeRatios.Length; i++)
        {
            sum += recipeRatios[i];
            if (randomValue < sum)
            {
                return recipeDatas[i];
            }
        }

        // 안전장치로 레시피 반환
        return recipeDatas[recipeDatas.Length - 1];
    }

    // 손님을 생성하는 메서드
    public void InstantiateGuest(Guest guestType)
    {
        //int recipeDice = Random.Range(0, recipeDatas.Length);

        GameObject obj = Instantiate(GetGuestPrefab(guestType), spawnPosition, Quaternion.LookRotation(Vector3.right));
        GuestController guest = obj.GetComponent<GuestController>();

        //guest.recipeData = recipeDatas[recipeDice];
        guest.recipeData = GetRandomRecipeData(recipeDatas, recipeRatios);
    }

    // 해당하는 손님 유형의 손님을 랜덤으로 반환하는 메서드
    public GameObject GetGuestPrefab(Guest guest)
    {
        int dice = Random.Range(0, guest.prefabs.Length);

        return guest.prefabs[dice];
    }

    // 난이도에 따라 손님 등장 빈도를 조절하는 메서드
    public void SetParameter(float span)
    {
        this.span = span;
    }

}
