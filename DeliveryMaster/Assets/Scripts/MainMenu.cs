using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Opcjonalne panele")]
    [Tooltip("Panel z listą top runów — pokazywany po kliknięciu Top Runs.")]
    public GameObject topRunsPanel;
    [Tooltip("Główny panel menu — chowany gdy pokazujesz Top Runs.")]
    public GameObject mainPanel;

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("MainScene");
    }

    public void StartRun()
    {
        RunManager.StartRun();
    }

    public void OpenTopRuns()
    {
        if (mainPanel != null) mainPanel.SetActive(false);
        if (topRunsPanel != null) topRunsPanel.SetActive(true);
    }

    public void CloseTopRuns()
    {
        if (topRunsPanel != null) topRunsPanel.SetActive(false);
        if (mainPanel != null) mainPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
