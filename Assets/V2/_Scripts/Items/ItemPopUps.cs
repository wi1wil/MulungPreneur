using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPopUps : MonoBehaviour
{
    public static ItemPopUps Instance { get; private set; }

    [SerializeField] private GameObject _popUpPrefab;
    [SerializeField] private int _maxPopUps = 5;
    [SerializeField] private float _popUpDuration = 2f;

    private readonly Queue<GameObject> _activePopUps = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DisplayPopUp(string itemName, Sprite icon)
    {
        Debug.Log($"Displaying a item {itemName}");
        GameObject popUpGO = Instantiate(_popUpPrefab, transform);
        popUpGO.GetComponent<ItemPopUpUI>().Setup(itemName, icon, _popUpDuration);
        _activePopUps.Enqueue(popUpGO);
        if(_activePopUps.Count > _maxPopUps)
        {
            Destroy(_activePopUps.Dequeue());
        }
    }
}
