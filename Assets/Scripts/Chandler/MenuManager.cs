using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Referances")]
    public GameObject buttons;
    public GameObject howToPlayScreen;
    public GameObject themesScreen;
    public GameObject titleScreen;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(howToPlayScreen.activeSelf || themesScreen.activeSelf)
            {
                themesScreen.SetActive(false);
                howToPlayScreen.SetActive(false);
                titleScreen.SetActive(true);

                buttons.SetActive(true);
            }
        }
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void ThemesButton()
    {
        themesScreen.SetActive(true);
        howToPlayScreen.SetActive(false);
        titleScreen.SetActive(false);

        buttons.SetActive(false);
    }

    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Application Quits...");
    }

    public void HowToPlayButton()
    {
        themesScreen.SetActive(false);
        howToPlayScreen.SetActive(true);
        titleScreen.SetActive(false);

        buttons.SetActive(false);
    }
}
