using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Crafting/Recipe")]
public class RecipesSO : ScriptableObject
{
    public string recipeName;
    public Sprite icon;
    public List<RecipeRequirements> inputs = new List<RecipeRequirements>();
    public GameObject outputPrefab;
    public int outputAmount = 1;
    public float craftTime;
    public int cost;
}
