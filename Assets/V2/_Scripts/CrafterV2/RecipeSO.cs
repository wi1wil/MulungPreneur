using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Crafter/Recipe")]
public class RecipeSO : ScriptableObject
{
    public string recipeName;
    public Sprite icon;
    public List<RecipeReq> inputs = new List<RecipeReq>();
    
    public ItemsSO outputItem;
    public int outputAmount = 1;
    
    public float craftTime;
    public int cost;
}
