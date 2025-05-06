using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private int mainMenuSceneIndex;

    [Header("Scene Transtion")]
    [SerializeField] private RawImage ftbImage;
    [SerializeField] private float fadeToBlackTime;


    [Header("Volume Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("SoundFXs")]
    public AudioClip optionsSaveSound;
    public AudioClip homePressedSound;
    public AudioClip buttonPressSound;

    private void Start()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);

        StartCoroutine(FTWTransition(() => { }));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionsMenu.activeSelf)
                OnOptionsBackClick();
            else if (pauseMenu.activeSelf)
                OnResumeClick();
            else
            {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
            }
        }
    }

    public void OnOptionsBackClick()
    {
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(true);
        SoundManager.instance.PlayOneshotAudio(buttonPressSound, Camera.main.transform.position, SoundManager.SoundType.sfx);
    }

    public void OnOptionsSaveClick()
    {
        float masterVol = masterSlider.value;
        float musicVol = musicSlider.value;
        float sfxVol = sfxSlider.value;

        SoundManager.instance.UpdateVolumePrefs(masterVol, musicVol, sfxVol);
        SoundManager.instance.PlayOneshotAudio(optionsSaveSound, Camera.main.transform.position, SoundManager.SoundType.sfx);
    }

    public void OnOptionsClick()
    {
        optionsMenu.SetActive(true);
        pauseMenu.SetActive(false);
        SoundManager.instance.PlayOneshotAudio(buttonPressSound, Camera.main.transform.position, SoundManager.SoundType.sfx);

        float masterVolume = PlayerPrefs.GetFloat("masterVolume");
        float musicVolume = PlayerPrefs.GetFloat("musicVolume");
        float sfxVolume = PlayerPrefs.GetFloat("sfxVolume");

        masterSlider.value = masterVolume;
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;
    }

    public void OnResumeClick()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        SoundManager.instance.PlayOneshotAudio(buttonPressSound, Camera.main.transform.position, SoundManager.SoundType.sfx);
    }

    public void OnHomeClick()
    {
        Debug.Log("fric");
        SoundManager.instance.PlayOneshotAudio(homePressedSound, Camera.main.transform.position, SoundManager.SoundType.sfx);
        AsyncOperation sceneLoadOp = SceneManager.LoadSceneAsync(mainMenuSceneIndex, LoadSceneMode.Single);
        sceneLoadOp.allowSceneActivation = false;

        ftbImage.raycastTarget = true;

        StartCoroutine(FTBTransition(() =>
        {
            Time.timeScale = 1;
            sceneLoadOp.allowSceneActivation = true;
        }));
    }

    public IEnumerator FTBTransition(System.Action finishCallback)
    {
        float currentTime = 0;
        while (currentTime <= fadeToBlackTime)
        {
            ftbImage.color = new Color(0, 0, 0, currentTime / fadeToBlackTime);
            yield return new WaitForEndOfFrame();
            currentTime += Time.unscaledDeltaTime;
        }

        ftbImage.color = new(0, 0, 0, 1);
        yield return new WaitForEndOfFrame();

        finishCallback.Invoke();
    }

    public IEnumerator FTWTransition(System.Action finishCallback)
    {
        float currentTime = 0;
        while (currentTime <= fadeToBlackTime)
        {
            ftbImage.color = new Color(0, 0, 0, 1 - currentTime / fadeToBlackTime);
            yield return new WaitForEndOfFrame();
            currentTime += Time.unscaledDeltaTime;
        }

        ftbImage.color = new(0, 0, 0, 0);
        yield return new WaitForEndOfFrame();

        finishCallback.Invoke();
    }

}
