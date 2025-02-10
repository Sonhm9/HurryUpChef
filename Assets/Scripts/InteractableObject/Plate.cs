using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    private float height = 0;
    private bool isFirst = true;

    public float getHeight()
    {
        return height;
    }

    public void setHeight(float num)
    {
        height = num;
    }

    public bool IsFirstBunOnPlate()
    {
        return isFirst;
    }

    public void SetIsFirstBoolean(bool b)
    {
        isFirst = b;
    }

    // ���� ������Ʈ�� �̸��� ����Ʈ�� �����ϴ� �Լ�
    public List<string> GetChildNames()
    {
        List<string> childNames = new List<string>();

        foreach (Transform child in transform)
        {
            string childName;

            // ���� ������Ʈ�� �̸��� ����Ʈ�� �߰� (Scriptable Object����)
            if (child.GetComponent<Ingredient>().IngredientData.HasMultipleStates)
            {
                int state = child.GetComponent<Patty>().GetCurrentState();
                childName = child.GetComponent<Ingredient>().IngredientData.GetNameByState(state);
            }
            else
            {
                childName = child.GetComponent<Ingredient>().IngredientData.IngredientName;
            }

            childNames.Add(childName);
        }

        return childNames;
    }

    // �����ǿ� ����Ʈ ��
    public bool CompareWithRecipe(RecipeData recipe)
    {
        List<string> childNames = GetChildNames();

        // �������� ��� ���� �ٸ��� �ٷ� false ��ȯ
        if (recipe.Ingredients.Count != childNames.Count)
        {
            return false;
        }

        // �� ����Ʈ�� ������ ����Ͽ� ���� ��
        for (int i = 0; i < recipe.Ingredients.Count; i++)
        {
            if (recipe.Ingredients[i] != childNames[i])
            {
                return false;
            }
        }

        return true;
    }
}