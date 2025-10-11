using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPopUpUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _itemNameText;
    [SerializeField] private Image _itemIcon;
    [SerializeField] private CanvasGroup _canvasGroup;

    public void Setup(string name, Sprite icon, float duration)
    {
        _itemNameText.text = name;
        _itemIcon.sprite = icon;
        StartCoroutine(FadeOutAndDestroy(duration));
    }
    
    private IEnumerator FadeOutAndDestroy(float duration)
    {
        yield return new WaitForSeconds(duration);
        float fadeTime = 1f;
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            _canvasGroup.alpha = 1f - t / fadeTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
