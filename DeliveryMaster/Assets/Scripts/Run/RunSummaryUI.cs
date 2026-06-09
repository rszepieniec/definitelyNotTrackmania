using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunSummaryUI : MonoBehaviour
{
    [Header("Statystyki — TextMeshProUGUI")]
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI deliveriesText;
    public TextMeshProUGUI avgRewardText;
    public TextMeshProUGUI maxRewardText;
    public TextMeshProUGUI carText;

    [Header("Input")]
    public TMP_InputField nameInput;

    [Header("Fallback")]
    [Tooltip("Nazwa sceny gdy z jakiegoś powodu nie ma RunManager.Instance.")]
    public string fallbackMenuScene = "MainMenu";

    private void Start()
    {
        if (RunManager.Instance == null)
        {
            Debug.LogWarning("RunSummaryUI: brak RunManager.Instance — pokażę zera.");
            SetText(coinsText, "0");
            SetText(deliveriesText, "0");
            SetText(avgRewardText, "0");
            SetText(maxRewardText, "0");
            SetText(carText, "—");
            return;
        }

        var preview = RunManager.Instance.BuildPendingRecord("");
        SetText(coinsText, preview.coinsEarned.ToString());
        SetText(deliveriesText, preview.deliveryCount.ToString());
        SetText(avgRewardText, preview.avgReward.ToString());
        SetText(maxRewardText, preview.maxReward.ToString());
        SetText(carText, preview.carName);

        if (nameInput != null)
        {
            nameInput.text = "";
            nameInput.Select();
            nameInput.ActivateInputField();
        }
    }

    public void OnSaveAndBackPressed()
    {
        string playerName = nameInput != null ? nameInput.text : "";
        if (RunManager.Instance != null)
        {
            RunManager.Instance.SaveAndReturnToMenu(playerName);
        }
        else
        {
            SceneManager.LoadScene(fallbackMenuScene);
        }
    }

    public void OnDiscardPressed()
    {
        if (RunManager.Instance != null)
        {
            RunManager.Instance.DiscardAndReturnToMenu();
        }
        else
        {
            SceneManager.LoadScene(fallbackMenuScene);
        }
    }

    private static void SetText(TextMeshProUGUI label, string value)
    {
        if (label != null) label.text = value;
    }
}
