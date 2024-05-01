using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        Debug.Log("play");
        SceneManager.LoadScene("Island");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
