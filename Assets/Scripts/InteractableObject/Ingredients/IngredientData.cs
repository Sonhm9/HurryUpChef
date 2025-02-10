using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient Data", menuName = "Scriptable Objects/Ingredient Data", order = int.MaxValue)]
public class IngredientData : ScriptableObject
{
    [SerializeField]
    private string ingredientName;
    public string IngredientName { get { return ingredientName; } }

    [SerializeField]
    private float offset;
    public float Offset { get { return offset; } }

    [SerializeField]
    private int price;
    public int Price { get { return price; } }

    [SerializeField]
    private bool hasMultipleStates;
    public bool HasMultipleStates { get { return hasMultipleStates; } }

    [SerializeField]
    private string uncookedStateName;
    public string UncookedStateName { get { return uncookedStateName; } }

    [SerializeField]
    private string cookedStateName;
    public string CookedStateName { get { return cookedStateName; } }

    [SerializeField]
    private string trashStateName;
    public string TrashStateName { get { return trashStateName; } }

    public string GetNameByState(int state)
    {
        if (!hasMultipleStates)
        {
            return ingredientName;
        }

        switch (state)
        {
            case 0:
                return uncookedStateName;
            case 1:
                return cookedStateName;
            case 2:
                return trashStateName;
            default:
                return ingredientName;
        };
    }
}
