using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "burger", menuName = "Scriptable Objects/Recipe")]
public class RecipeData : ScriptableObject
{
    public string RecipeName; // 음식 이름
    public List<string> Ingredients; // 재료 리스트
    public Sprite RecipeImage; // 음식 이미지
    public int RecipePoint; // 음식 점수
}