using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Jouer()
    {
        Debug.Log("Jouer");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Options()
    {

    }

    public void quitter()
    {
        Debug.Log("Quitter");
        Application.Quit();
    }
}
