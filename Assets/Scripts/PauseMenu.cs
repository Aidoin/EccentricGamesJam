using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private ButtonControls buttonControls;

    [SerializeField] private GameObject pausePanel;

    private bool isPaused; public bool IsPaused => isPaused;
    public bool isEnd = false;


    private void Start() {
        buttonControls = ButtonControls.Instance;
    }

    void Update() {
        if (Input.GetKeyDown(buttonControls.Escape) && isEnd == false) {
            if (isPaused) {
                GameManager.Instance.UnPause();
                Unpause();
            } else {
                GameManager.Instance.Pause();
                Pause();
            }
        }
    }

    public void Pause() {
        isPaused = true;
        pausePanel.SetActive(true);
    }
    public void Unpause() {
        isPaused = false;
        pausePanel.SetActive(false);
    }

    public void Restart() {
        isPaused = false;
        pausePanel.SetActive(false);
    }
    public void Exit() {
        Application.Quit();
    }
}
