using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public SceneAsset startScene;
    public RawImage ftbImage;
    public GameObject mainMenuButtons;
    public GameObject creditsMenu;
    public float fadeToBlackTime;

    [Header("Option Refs")]
    public GameObject optionsMenu;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    public void Start()
    {
        mainMenuButtons.SetActive(true);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }

    public void OnStartClick()
    {
        AsyncOperation sceneLoadOp = SceneManager.LoadSceneAsync(startScene.name, LoadSceneMode.Single);
        sceneLoadOp.allowSceneActivation = false;

        ftbImage.raycastTarget = true;

        StartCoroutine(FTBTransition(() =>
        {
            sceneLoadOp.allowSceneActivation = true;
        }));
    }

    public void OnCreditsClick()
    {
        optionsMenu.SetActive(false);
        mainMenuButtons.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void OnOptionSaveClick()
    {
        float masterVol = masterSlider.value;
        float musicVol = musicSlider.value;
        float sfxVol = sfxSlider.value;

        SoundManager.instance.UpdateVolumePrefs(masterVol, musicVol, sfxVol);

    }

    public void OnBackToHomeClick()
    {
        optionsMenu.SetActive(false);
        mainMenuButtons.SetActive(true);
        creditsMenu.SetActive(false);
    }

    public void OnOptionsClick()
    {
        mainMenuButtons.SetActive(false);
        optionsMenu.SetActive(true);
        creditsMenu.SetActive(false);

        float masterVolume = PlayerPrefs.GetFloat("masterVolume");
        float musicVolume = PlayerPrefs.GetFloat("musicVolume");
        float sfxVolume = PlayerPrefs.GetFloat("sfxVolume");

        masterSlider.value = masterVolume;
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;
    }

    public IEnumerator FTBTransition(System.Action finishCallback)
    {
        float currentTime = 0;
        while(currentTime <= fadeToBlackTime)
        {
            ftbImage.color = new Color(0, 0, 0, currentTime / fadeToBlackTime);
            yield return new WaitForEndOfFrame();
            currentTime += Time.deltaTime;
        }

        ftbImage.color = new(0, 0, 0, 1);

        finishCallback.Invoke();
    }

    public void OnQuitClick()
    {
        Application.Quit();
        Debug.Log("Application quits");
    }
}
