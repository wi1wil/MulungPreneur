using UnityEngine;
using UnityEngine.UI;

public class MenuTabsManager : MonoBehaviour
{
    [SerializeField] private Image[] _tabImages;
    [SerializeField] private GameObject[] _pagesGO;

    void Start()
    {
        ActivateTab(0);
    }

    public void ActivateTab(int tabIndex)
    {
        AudioManager.instance.PlayUIClick();
        for (int i = 0; i < _pagesGO.Length; i++)
        {
            _pagesGO[i].SetActive(false);
            _tabImages[i].color = Color.gray;
        }
        _pagesGO[tabIndex].SetActive(true);
        _tabImages[tabIndex].color = Color.white;
    }
}
