using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunManager : MonoBehaviour
{
    public static RunManager Instance;

    [Header("Konfiguracja")]
    [Tooltip("Długość rundy w sekundach (180 = 3 minuty).")]
    public float runDuration = 180f;
    public string gameplaySceneName = "MainScene";
    public string summarySceneName = "RunSummary";
    public string mainMenuSceneName = "MainMenu";

    public bool IsRunActive { get; private set; }
    public float TimeLeft { get; private set; }
    public int StartCoins { get; private set; }
    public int DeliveryCount { get; private set; }
    public int MaxReward { get; private set; }
    public int CoinsEarned { get; private set; }

    public static RunRecord LastSavedRecord;

    private int totalRewardSum;
    private string carIdSnapshot;

    private int CurrentCoins => CoinManager.Instance != null ? CoinManager.Instance.coins : 0;

    public static void StartRun()
    {
        if (Instance == null)
        {
            var go = new GameObject("RunManager");
            DontDestroyOnLoad(go);
            Instance = go.AddComponent<RunManager>();
        }
        Instance.BeginRun();
    }

    private void BeginRun()
    {
        IsRunActive = true;
        TimeLeft = runDuration;
        DeliveryCount = 0;
        MaxReward = 0;
        totalRewardSum = 0;

        carIdSnapshot = "unknown";
        if (ShopDataManager.Instance != null)
        {
            carIdSnapshot = ShopDataManager.Instance.UserProfile.selectedCarId ?? "unknown";
        }

        SceneManager.sceneLoaded += OnSceneLoadedCaptureCoins;
        SceneManager.LoadScene(gameplaySceneName);
    }

    private void OnSceneLoadedCaptureCoins(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != gameplaySceneName) return;
        SceneManager.sceneLoaded -= OnSceneLoadedCaptureCoins;
        StartCoins = CurrentCoins;
    }

    private void Update()
    {
        if (!IsRunActive) return;

        TimeLeft -= Time.deltaTime;
        if (TimeLeft <= 0f)
        {
            TimeLeft = 0f;
            FinishRun();
        }
    }

    public void OnDeliveryCompleted(int reward)
    {
        if (!IsRunActive) return;
        DeliveryCount++;
        totalRewardSum += reward;
        if (reward > MaxReward) MaxReward = reward;
    }

    private void FinishRun()
    {
        IsRunActive = false;
        CoinsEarned = Mathf.Max(0, CurrentCoins - StartCoins);
        if (CoinsEarned > 0)
            AudioManager.Instance?.PlaySFXThenSFX(AudioManager.Instance.sfxRunComplete, AudioManager.Instance.sfxCoins);
        else
            AudioManager.Instance?.PlaySFX(AudioManager.Instance.sfxRunFail);
        SceneManager.LoadScene(summarySceneName);
    }

    public RunRecord BuildPendingRecord(string playerName)
    {
        return new RunRecord
        {
            id = DateTime.UtcNow.Ticks.ToString(),
            playerName = string.IsNullOrWhiteSpace(playerName) ? "Anonymous" : playerName.Trim(),
            dateIso = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),
            coinsEarned = CoinsEarned,
            carName = ResolveCarDisplayName(carIdSnapshot),
            deliveryCount = DeliveryCount,
            avgReward = DeliveryCount > 0 ? Mathf.RoundToInt((float)totalRewardSum / DeliveryCount) : 0,
            maxReward = MaxReward
        };
    }

    public void SaveAndReturnToMenu(string playerName)
    {
        var record = BuildPendingRecord(playerName);
        RunRecordsStorage.Append(record);
        LastSavedRecord = record;

        if (ShopDataManager.Instance != null && record.coinsEarned > 0)
        {
            ShopDataManager.Instance.AddToAccount(record.coinsEarned);
        }

        SceneManager.LoadScene(mainMenuSceneName);
        Destroy(gameObject);
        Instance = null;
    }

    public void DiscardAndReturnToMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
        Destroy(gameObject);
        Instance = null;
    }

    private static string ResolveCarDisplayName(string carId)
    {
        if (string.IsNullOrEmpty(carId)) return "unknown";
        if (ShopDataManager.Instance == null) return carId;
        var car = ShopDataManager.Instance.GetCar(carId);
        return car != null && !string.IsNullOrEmpty(car.name) ? car.name : carId;
    }
}
