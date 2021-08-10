using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Audio;
using TMPro;
using System.Linq;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] TMP_Dropdown resolutionsDropdown;
    [SerializeField] TMP_Dropdown qualityDropdown;
    [SerializeField] Slider volumeSlider;
    [SerializeField] Toggle soundsToggle;

    Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();
        resolutionsDropdown.ClearOptions();

        int currResIndex = 0;
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            //if (resolutions[i].Equals(Screen.currentResolution))
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currResIndex = i;
            }
        }

        resolutionsDropdown.AddOptions(options);

        resolutionsDropdown.value = PlayerPrefs.GetInt("resolution", currResIndex);
        resolutionsDropdown.RefreshShownValue();

        if (qualityDropdown)
        {
            qualityDropdown.value = PlayerPrefs.GetInt("quality", 3);
            qualityDropdown.RefreshShownValue();
        }

        if (volumeSlider)
            volumeSlider.value = PlayerPrefs.GetFloat("mvol", -5);
        soundsToggle.isOn = PlayerPrefs.GetInt("fullscreen", 1) == 1;

        LoadSavedSettings();

        this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        PlayerPrefs.Save();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        //Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("resolution", resolutionIndex);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
        PlayerPrefs.SetFloat("mvol", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("quality", qualityIndex);
    }

    public void ToggleSounds(bool toggle)
    {

        if (toggle)
            PlayerPrefs.SetInt("sfx", 1);
        else
            PlayerPrefs.SetInt("sfx", 0);
    }

    void LoadSavedSettings()
    {
        //SetResolution(resolutionsDropdown.value);
        //SetQuality(qualityDropdown.value);
        ToggleSounds(PlayerPrefs.GetInt("fullscreen", 1) == 1);
        //SetVolume(PlayerPrefs.GetFloat("mvol", -5));
    }
}
