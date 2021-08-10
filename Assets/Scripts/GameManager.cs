using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField] Animator transition;
    [SerializeField] GameObject drawerUI;
    [SerializeField] GameObject buttonsUI;
    [SerializeField] ParticleSystem place_effect;
    [SerializeField] SliderMenu menu;
    [SerializeField] SliderMenu drawer;
    AudioSource placeSfx;

    AvatarController avatarController;
    TouchManager touchManager;
    public Vector2 mapBounds;

    public enum gameStates { Placing, Exploring };
    gameStates gameState;
    public static GameManager gm;
    float transitionTime = 0.4f;
    public bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        if (gm == null)
        {
            gm = this;
        }

        avatarController = FindObjectOfType<AvatarController>();
        touchManager = FindObjectOfType<TouchManager>();
        placeSfx = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case gameStates.Placing:
                touchManager.enabled = true;
                drawerUI.SetActive(true);
                buttonsUI.SetActive(false);
                break;
            case gameStates.Exploring:
                touchManager.enabled = false;
                drawerUI.SetActive(false);
                if (paused)
                    buttonsUI.SetActive(false);
                else
                    buttonsUI.SetActive(true);
                break;
        }
    }

    public string GetState()
    {
        return gameState.ToString();
    }

    public void ExploreWorld()
    {
        gameState = gameStates.Exploring;
        SetTPPCamera(true);
    }

    public void EditWorld()
    {
        gameState = gameStates.Placing;
        SetTPPCamera(false);
    }

    public void ClearWorld()
    {
        EditBuilding[] buildings = FindObjectsOfType<EditBuilding>();
        foreach (EditBuilding b in buildings)
        {
            Destroy(b.gameObject);
        }
    }

    void SetTPPCamera(bool toggle)
    {
        avatarController.EnableThirdPersonCam(toggle);
    }

    public void PlayPlaceEffect(Vector3 pos)
    {
        placeSfx.Play();
        place_effect.transform.position = pos;
        place_effect.Play();
    }

    public void HideMenu()
    {
        menu.HideMenu();
        drawer.HideMenu();
        paused = false;
    }

    public void PauseGame()
    {
        paused = !paused;
    }

    public void LoadScene(string levelToLoad)
    {
        StartCoroutine(LoadLevelTransition(levelToLoad));
    }

    IEnumerator LoadLevelTransition(string levelToLoad)
    {
        transition.SetTrigger("fade");
        Time.timeScale = 1;
        yield return new WaitForSeconds(transitionTime);

        // load the specified level
        SceneManager.LoadScene(levelToLoad);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
