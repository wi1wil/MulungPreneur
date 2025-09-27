using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TabControllerScript : MonoBehaviour
{
    private AudioManagerScript audioScript;
    public Image[] tabImages;
    public GameObject[] pages;

    private void Awake()
    {
        // if (!audioScript)
        //     audioScript = GameObject.Find("Audio Manager").GetComponent<AudioManagerScript>();
    }
    void Start()
    {
        ActivateTab(0);
    }

    public void ActivateTab(int tabIndex)
    {
        // audioScript.PlaySfx(audioScript.uiClick);
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
            tabImages[i].color = Color.gray;
        }
        pages[tabIndex].SetActive(true);
        tabImages[tabIndex].color = Color.white;
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Start Menu");
    }
}
