using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class CrafterUIManager : MonoBehaviour
{
    public static CrafterUIManager Instance;

    [Header("Recipe List UI")]
    public RectTransform content; 
    public GameObject craftItemPrefab;
    public List<RecipesSO> recipes;

    [Header("Crafting UI")]
    public GameObject recipesPanel;      
    public GameObject craftingPanel;     
    public TMP_Text craftingNameText;    
    public Image progressFill;           

    private List<CrafterItemPrefabs> spawned = new List<CrafterItemPrefabs>();

    void Awake() => Instance = this;

    void Start() => Populate();

    void Populate()
    {
        foreach (var r in recipes)
        {
            var go = Instantiate(craftItemPrefab, content);
            var ui = go.GetComponent<CrafterItemPrefabs>();
            ui.Setup(r);
            spawned.Add(ui);
        }
    }

    public void SetAllButtonsInteractable(bool v)
    {
        foreach (var ui in spawned) ui.SetInteractable(v);
    }

    public void OnItemCraftClicked(RecipesSO recipe)
    {
        // bool started = CrafterManager.Instance.TryCraft(recipe);
        // if (started)
        // {
        //     recipesPanel.SetActive(false);
        //     craftingPanel.SetActive(true);

        //     craftingNameText.text = recipe.recipeName;
        //     progressFill.fillAmount = 0f;
        // }
        Debug.Log("Crafting: " + recipe.recipeName);
    }

    public void UpdateCraftingProgress(float t) =>
        progressFill.fillAmount = t;

    public void OnCraftingFinished()
    {
        craftingPanel.SetActive(false);
        recipesPanel.SetActive(true);
    }
}
