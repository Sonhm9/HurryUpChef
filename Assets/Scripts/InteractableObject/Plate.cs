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

    // 하위 오브젝트의 이름을 리스트로 저장하는 함수
    public List<string> GetChildNames()
    {
        List<string> childNames = new List<string>();

        foreach (Transform child in transform)
        {
            string childName;

            // 하위 오브젝트의 이름을 리스트에 추가 (Scriptable Object에서)
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

    // 레시피와 리스트 비교
    public bool CompareWithRecipe(RecipeData recipe)
    {
        List<string> childNames = GetChildNames();

        // 레시피의 재료 수가 다르면 바로 false 반환
        if (recipe.Ingredients.Count != childNames.Count)
        {
            return false;
        }

        // 두 리스트의 순서를 고려하여 직접 비교
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