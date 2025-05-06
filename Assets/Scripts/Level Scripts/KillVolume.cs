using UnityEngine;
using UnityEngine.SceneManagement;

public class KillVolume : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerUIManager ui = FindFirstObjectByType<PlayerUIManager>();

        AsyncOperation reloadScene = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        reloadScene.allowSceneActivation = false;
        StartCoroutine(ui.FTBTransition(() =>
        {
            reloadScene.allowSceneActivation = true;
        }));
    }
}
