using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    public string mainMenuSceneName = "MainMenu";

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Oyun durduysa geri al
        SceneManager.LoadScene(mainMenuSceneName);
    }
}

