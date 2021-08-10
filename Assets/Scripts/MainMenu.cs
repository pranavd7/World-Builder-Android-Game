using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Animator transition;

    float transitionTime = 0.4f;

    public void LoadLevel(string levelToLoad)
    {
        StartCoroutine(LoadLevelTransition(levelToLoad));
    }

    IEnumerator LoadLevelTransition(string levelToLoad)
    {
        transition.SetTrigger("fade");
        yield return new WaitForSeconds(transitionTime);

        // load the specified level
        SceneManager.LoadScene(levelToLoad);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
