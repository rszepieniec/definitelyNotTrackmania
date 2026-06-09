using TMPro;
using UnityEngine;

public class RunTimerUI : MonoBehaviour
{
    [Tooltip("Tekst na który zostanie wpisany czas run-a. Najczęściej top-left.")]
    public TextMeshProUGUI timerText;
    [Tooltip("Sekundy przy których timer zaczyna mrugać/zmieniać kolor.")]
    public float warningTime = 15f;
    public Color normalColor = Color.white;
    public Color warningColor = new Color(1f, 0.3f, 0.2f);
    public float blinkSpeed = 4f;

    [Tooltip("Co pokazać gdy nie ma aktywnego run-a (np. zwykła gra bez timera).")]
    public string idleText = "";

    private void Update()
    {
        if (timerText == null) return;

        if (RunManager.Instance == null || !RunManager.Instance.IsRunActive)
        {
            timerText.text = idleText;
            timerText.color = normalColor;
            return;
        }

        float t = RunManager.Instance.TimeLeft;
        int totalSeconds = Mathf.Max(0, Mathf.CeilToInt(t));
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (t <= warningTime)
        {
            float blink = Mathf.PingPong(Time.time * blinkSpeed, 1f);
            timerText.color = Color.Lerp(normalColor, warningColor, blink);
        }
        else
        {
            timerText.color = normalColor;
        }
    }
}
