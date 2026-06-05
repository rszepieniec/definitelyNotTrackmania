using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopRunsUI : MonoBehaviour
{
    [Header("Lista (ScrollView)")]
    [Tooltip("Content prostokąta wewnątrz ScrollView (z VerticalLayoutGroup + ContentSizeFitter).")]
    public Transform listContent;
    [Tooltip("Prefab pojedynczego wiersza. Musi mieć komponent RunRow na korzeniu.")]
    public GameObject rowPrefab;

    [Header("Wyróżnianie ostatniego runa")]
    public Color normalRowColor = new Color(1f, 1f, 1f, 0.05f);
    public Color highlightRowColor = new Color(1f, 0.85f, 0.2f, 0.35f);

    [Header("Pusta lista")]
    public GameObject emptyMessage;

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (listContent == null || rowPrefab == null)
        {
            Debug.LogWarning("TopRunsUI: brak listContent lub rowPrefab.");
            return;
        }

        for (int i = listContent.childCount - 1; i >= 0; i--)
        {
            Destroy(listContent.GetChild(i).gameObject);
        }

        var db = RunRecordsStorage.Load();
        var runs = new List<RunRecord>(db.runs);
        runs.Sort((a, b) => b.coinsEarned.CompareTo(a.coinsEarned));

        if (emptyMessage != null)
        {
            emptyMessage.SetActive(runs.Count == 0);
        }

        string lastId = RunManager.LastSavedRecord != null ? RunManager.LastSavedRecord.id : null;

        for (int i = 0; i < runs.Count; i++)
        {
            var instance = Instantiate(rowPrefab, listContent);
            var row = instance.GetComponent<RunRow>();
            if (row == null)
            {
                Debug.LogWarning("TopRunsUI: rowPrefab nie ma komponentu RunRow na korzeniu.");
                Destroy(instance);
                continue;
            }
            bool highlight = lastId != null && runs[i].id == lastId;
            row.Bind(i + 1, runs[i], highlight ? highlightRowColor : normalRowColor);
        }
    }
}
