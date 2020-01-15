using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject OptionsMenu;


    public void OpenOptions()
    {
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(true);
    }
    public void ReprendrePartie()
    {
        PauseMenu.SetActive(false);
    }

    public void OpenPauseMenuFromOptions()
    {
        OptionsMenu.SetActive(false);
        PauseMenu.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu.SetActive(true);
        }
    }
}
