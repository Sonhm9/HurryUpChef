using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GuestSpawnManager : MonoBehaviour
{
    public Guest[] guestTypes; // �մ� Ÿ�� �迭
    public float[] ratios; // �մ� Ÿ�Կ� ���� Ȯ�� �迭
    public RecipeData[] recipeDatas; // ���� ������ �迭
    public float[] recipeRatios; // ���� �����Ϳ� ���� Ȯ�� �迭

    public Vector3 spawnPosition;

    [SerializeField] float span = 15f; // ���� ��

    float delta; // ���� �ð�

    Coroutine guestSpawnRoutine;

    void Start()
    {
        guestSpawnRoutine = StartCoroutine(GuestSpawnRoutine());
    }

    // �մ� ���� ��ƾ
    IEnumerator GuestSpawnRoutine()
    {
        while (true)
        {
            // �����ð�(��)�� ������ �մ� ����
            yield return new WaitForSeconds(span);

            // �������� �մ� ����
            InstantiateGuest(GetRandomGuest(guestTypes, ratios));
        }
    }

    // �մ� ������ �ߴ��ϴ� �޼���
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
        // Ȯ���� ������ ���
        float totalRatio = 0;
        for (int i = 0; i < ratios.Length; i++)
        {
            totalRatio += ratios[i];
        }

        // ������ ����
        float randomValue = Random.Range(1, totalRatio + 1);

        // Ȯ���� ������ ���� �ش� �մ� ��ȯ
        float sum = 0;
        for (int i = 0; i < ratios.Length; i++)
        {
            sum += ratios[i];
            if(randomValue < sum)
            {
                return guests[i];
            }
        }

        // ������ġ�� �մ� ��ȯ
        return guests[guests.Length - 1];
    }

    public RecipeData GetRandomRecipeData(RecipeData[] recipeDatas, float[] recipeRatios)
    {
        // Ȯ���� ������ ���
        float totalRatio = 0;
        for (int i = 0; i < recipeRatios.Length; i++)
        {
            totalRatio += recipeRatios[i];
        }

        // ������ ����
        float randomValue = Random.Range(1, totalRatio + 1);

        // Ȯ���� ������ ���� �ش� ������ ��ȯ
        float sum = 0;
        for (int i = 0; i < recipeRatios.Length; i++)
        {
            sum += recipeRatios[i];
            if (randomValue < sum)
            {
                return recipeDatas[i];
            }
        }

        // ������ġ�� ������ ��ȯ
        return recipeDatas[recipeDatas.Length - 1];
    }

    // �մ��� �����ϴ� �޼���
    public void InstantiateGuest(Guest guestType)
    {
        //int recipeDice = Random.Range(0, recipeDatas.Length);

        GameObject obj = Instantiate(GetGuestPrefab(guestType), spawnPosition, Quaternion.LookRotation(Vector3.right));
        GuestController guest = obj.GetComponent<GuestController>();

        //guest.recipeData = recipeDatas[recipeDice];
        guest.recipeData = GetRandomRecipeData(recipeDatas, recipeRatios);
    }

    // �ش��ϴ� �մ� ������ �մ��� �������� ��ȯ�ϴ� �޼���
    public GameObject GetGuestPrefab(Guest guest)
    {
        int dice = Random.Range(0, guest.prefabs.Length);

        return guest.prefabs[dice];
    }

    // ���̵��� ���� �մ� ���� �󵵸� �����ϴ� �޼���
    public void SetParameter(float span)
    {
        this.span = span;
    }

}
