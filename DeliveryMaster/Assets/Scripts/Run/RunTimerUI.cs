using TMPro;
using UnityEngine;

public class RunTimerUI : MonoBehaviour
{
    [Tooltip("Tekst na który zostanie wpisany czas run-a. Najczęściej top-left.")]
    public TextMeshProUGUI timerText;
    [Tooltip("Duży tekst countdownu 3-2-1-GO! Najczęściej środek ekranu. Opcjonalne — jeśli puste, countdown ląduje w timerText.")]
    public TextMeshProUGUI countdownText;
    [Tooltip("Sekundy przy których timer zaczyna mrugać/zmieniać kolor.")]
    public float warningTime = 15f;
    public Color normalColor = Color.white;
    public Color warningColor = new Color(1f, 0.3f, 0.2f);
    public Color goColor = new Color(0.3f, 1f, 0.4f);
    public float blinkSpeed = 4f;

    [Tooltip("Co pokazać gdy nie ma aktywnego run-a (np. zwykła gra bez timera).")]
    public string idleText = "";

    private void Update()
    {
        if (RunManager.Instance == null)
        {
            SetTimer(idleText, normalColor);
            SetCountdown("", normalColor, false);
            return;
        }

        if (RunManager.Instance.IsCountdown)
        {
            float c = RunManager.Instance.CountdownTimeLeft;
            string text;
            Color col;
            if (c > 0f)
            {
                text = Mathf.CeilToInt(c).ToString();
                col = normalColor;
            }
            else
            {
                text = "GO!";
                col = goColor;
            }

            if (countdownText != null)
            {
                SetCountdown(text, col, true);
                SetTimer(idleText, normalColor);
            }
            else
            {
                SetTimer(text, col);
            }
            return;
        }

        SetCountdown("", normalColor, false);

        if (!RunManager.Instance.IsRunActive)
        {
            SetTimer(idleText, normalColor);
            return;
        }

        float t = RunManager.Instance.TimeLeft;
        int totalSeconds = Mathf.Max(0, Mathf.CeilToInt(t));
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        string timerStr = string.Format("{0:00}:{1:00}", minutes, seconds);

        Color timerCol;
        if (t <= warningTime)
        {
            float blink = Mathf.PingPong(Time.time * blinkSpeed, 1f);
            timerCol = Color.Lerp(normalColor, warningColor, blink);
        }
        else
        {
            timerCol = normalColor;
        }
        SetTimer(timerStr, timerCol);
    }

    private void SetTimer(string text, Color color)
    {
        if (timerText == null) return;
        timerText.text = text;
        timerText.color = color;
    }

    private void SetCountdown(string text, Color color, bool visible)
    {
        if (countdownText == null) return;
        countdownText.gameObject.SetActive(visible);
        if (!visible) return;
        countdownText.text = text;
        countdownText.color = color;
    }
}
