using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    Slider bgmVolumeSlider;
    [SerializeField] 
    Slider sfxVolumeSlider;
    [SerializeField]
    private AudioSource mainMenuButtons;
    // Start is called before the first frame update
    void Start()
    {

        bgmVolumeSlider.onValueChanged.AddListener(delegate
        {
            ChangeVolume();
        });
        sfxVolumeSlider.onValueChanged.AddListener(delegate
        {
            ChangeVolume();
        });


        // If there isn't any saved data from the previous game session, set the value to 1
        if (!PlayerPrefs.HasKey("bgmVolume"))
        {
            PlayerPrefs.SetFloat("bgmVolume", 1);
        }
        if (!PlayerPrefs.HasKey("sfxVolume"))
        {
            PlayerPrefs.SetFloat("sfxVolume", 1);
        }
            Load();
    }

    // Sets the volume of the audio to match the value of the slider
    public void ChangeVolume()
    {
        GameManager.instance.bgmAudioSource.volume = bgmVolumeSlider.value;
        GameManager.instance.bgmVolume = bgmVolumeSlider.value;
        GameManager.instance.sfxVolume = sfxVolumeSlider.value;
        mainMenuButtons.volume = GameManager.instance.sfxVolume;
    }

    // This will get the value of the slider, that was set to a float
    private void Load()
    {
        bgmVolumeSlider.value = PlayerPrefs.GetFloat("bgmVolume");
        GameManager.instance.bgmAudioSource.volume = bgmVolumeSlider.value;
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        GameManager.instance.sfxVolume = sfxVolumeSlider.value;
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    private void OnDestroy()
    {
        Save();
    }

    // This will take the value set on the volume sider and save it
    private void Save()
    {
        Debug.Log("Saving Volume Settings");
        PlayerPrefs.SetFloat("bgmVolume", bgmVolumeSlider.value);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolumeSlider.value);
    }
}
