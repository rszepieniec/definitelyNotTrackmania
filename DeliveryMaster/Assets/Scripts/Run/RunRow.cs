using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RunRow : MonoBehaviour
{
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI carText;
    public TextMeshProUGUI deliveriesText;
    public TextMeshProUGUI avgText;
    public TextMeshProUGUI maxText;
    public TextMeshProUGUI dateText;
    public Image background;

    public void Bind(int rank, RunRecord r, Color bg)
    {
        if (rankText != null) rankText.text = "#" + rank;
        if (nameText != null) nameText.text = r.playerName;
        if (coinsText != null) coinsText.text = r.coinsEarned.ToString();
        if (carText != null) carText.text = r.carName;
        if (deliveriesText != null) deliveriesText.text = r.deliveryCount.ToString();
        if (avgText != null) avgText.text = r.avgReward.ToString();
        if (maxText != null) maxText.text = r.maxReward.ToString();
        if (dateText != null) dateText.text = r.GetDate().ToString("yyyy-MM-dd HH:mm");
        if (background != null) background.color = bg;
    }
}
