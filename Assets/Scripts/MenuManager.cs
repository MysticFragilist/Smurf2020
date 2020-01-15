using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public Image titleScreen;
    public GameObject MainMenu;
    public GameObject OptionsMenu;
    public GameObject TitleScreen;
    public GameObject CreditsMenu;
    public GameObject MultiplayerMenu;
    public GameObject PressSpace;
    //public EventSystem eventSystems;

    public void OpenMultiplayerScreen()
    {
        MainMenu.SetActive(false);
        MultiplayerMenu.SetActive(true);
    }

    private void LoadSampleScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        #if UNITY_EDITOR
             UnityEditor.EditorApplication.isPlaying = false;
        #else
             Application.Quit();
        #endif
    }

    public void OpenOptions()
    {
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(true);
    }

    public void OpenCredits()
    {
        MainMenu.SetActive(false);
        CreditsMenu.SetActive(true);
    }

    public void OpenMainMenu()
    {
        fadeAway();
    }

    public void OpenMainMenuFromOptions()
    {
        OptionsMenu.SetActive(false);
        MainMenu.SetActive(true);
    }
    public void OpenMainMenuFromCredits()
    {
        OptionsMenu.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void fadeAway()
    {
        StartCoroutine(FadeTo(0.0f, 2.0f));
    }

    IEnumerator FadeTo(float aValue, float aTime)
    {
        float alpha = titleScreen.color.a;
        PressSpace.SetActive(false);
        MainMenu.SetActive(true);
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            titleScreen.color = newColor;
            yield return null;
        }
        TitleScreen.SetActive(false);
        MainMenu.SetActive(true);
        //eventSystems.SetSelectedGameObject(NextSelectedBtn);
    }
}
