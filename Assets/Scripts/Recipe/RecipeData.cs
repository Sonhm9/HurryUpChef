using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "burger", menuName = "Scriptable Objects/Recipe")]
public class RecipeData : ScriptableObject
{
    public string RecipeName; // ���� �̸�
    public List<string> Ingredients; // ��� ����Ʈ
    public Sprite RecipeImage; // ���� �̹���
    public int RecipePoint; // ���� ����
}