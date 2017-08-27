using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIControllerScript : MonoBehaviour {

    public RectTransform menuPanel;
    public RectTransform inPlayPanel;
    public RectTransform settingsPanel;

    public Text startResumeButtonText;
    public Text scoreText;

    public PlayerControllerScript player;
 	// Use this for initialization
	void Start () {
        menuPanel.gameObject.SetActive(true);
        inPlayPanel.gameObject.SetActive(false);
        settingsPanel.gameObject.SetActive(false);
        Time.timeScale = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MainMenu();
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

        scoreText.text = "Score: " + player.GetScore();
    }

    public void StartGame()
    {
        menuPanel.gameObject.SetActive(false);
        inPlayPanel.gameObject.SetActive(true);
        settingsPanel.gameObject.SetActive(false);
        startResumeButtonText.text = "Resume Game";
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SettingsMenu()
    {
        menuPanel.gameObject.SetActive(false);
        inPlayPanel.gameObject.SetActive(false);
        settingsPanel.gameObject.SetActive(true);

    }

    public void MainMenu()
    {
        menuPanel.gameObject.SetActive(true);
        inPlayPanel.gameObject.SetActive(false);
        settingsPanel.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        startResumeButtonText.text = "Start Game";
        Application.Quit();
    }
}
