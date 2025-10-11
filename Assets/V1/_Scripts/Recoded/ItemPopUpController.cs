using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;

public class ItemPopUpController : MonoBehaviour
{
    public static ItemPopUpController Instance { get; private set; }

    public GameObject ItemPopUpPrefab;
    public int maxPopUps = 5;
    public float popUpDuration = 2f;
    private readonly Queue<GameObject> _activePopUp = new();

    void Awake()
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

    public void ShowItemPickUp(string itemName, Sprite ItemIcon)
    {
        GameObject newPopup = Instantiate(ItemPopUpPrefab, transform);
        newPopup.GetComponentInChildren<TMP_Text>().text = itemName;

        Image itemImage = newPopup.transform.Find("ItemIcon")?.GetComponent<Image>();
        if (itemImage)
        {
            itemImage.sprite = ItemIcon;
        }

        _activePopUp.Enqueue(newPopup);
        if (_activePopUp.Count > maxPopUps)
        {
            Destroy(_activePopUp.Dequeue());
        }

        StartCoroutine(FadeOutAndDestroy(newPopup));
    }

    private IEnumerator FadeOutAndDestroy(GameObject popUp)
    {
        yield return new WaitForSeconds(popUpDuration);
        if (popUp == null) yield break;
        CanvasGroup canvasGroup = popUp.GetComponent<CanvasGroup>();
        for (float timePassed = 0; timePassed < 1f; timePassed += Time.deltaTime)
        {
            if (popUp == null) yield break;
            canvasGroup.alpha = 1f - timePassed;
            yield return null;
        }

        Destroy(popUp);
    }
}
