using UnityEngine;

public class Ingredient : MonoBehaviour
{
    [SerializeField]
    private IngredientData ingredientData;
    public IngredientData IngredientData 
    {
        get { return ingredientData; }
        set { IngredientData = value; } 
    }
}
