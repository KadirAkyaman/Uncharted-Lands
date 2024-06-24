using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
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
