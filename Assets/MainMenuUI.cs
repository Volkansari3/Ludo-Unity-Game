using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject howToPlayPanel;
    public GameObject settingsPanel;
    public GameObject creditsPanel;

    // ---- PLAY (AYNEN KALDI) ----
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    // ---- OTHER BUTTONS ----
    public void OpenHowToPlay()
    {
        CloseAll();
        howToPlayPanel.SetActive(true);
    }

    public void OpenSettings()
    {
        CloseAll();
        settingsPanel.SetActive(true);
    }

    public void OpenCredits()
    {
        CloseAll();
        creditsPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        CloseAll();
        mainMenuPanel.SetActive(true);
    }

    // ---- HELPER ----
    void CloseAll()
    {
        if (mainMenuPanel) mainMenuPanel.SetActive(true);
        if (howToPlayPanel) howToPlayPanel.SetActive(false);
        if (settingsPanel) settingsPanel.SetActive(false);
        if (creditsPanel) creditsPanel.SetActive(false);
    }

    public void SetMusicVolume(float value)
    {
        AudioListener.volume = value;
    }

}