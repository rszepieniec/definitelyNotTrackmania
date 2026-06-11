# Delivery Master

Mała gra zręcznościowa w Unity — wcielasz się w kuriera w niskopolygonowym mieście. Łapiesz misje, dostarczasz paczki w wyznaczonym czasie, zbierasz monety, kupujesz lepsze auta. Inspiracja: Crazy Taxi spotkany z Trackmanią.

## Główne mechaniki

- **Tryb Run (3 min)** — od kliknięcia *Start Run* lecisz przeciw zegarowi i zarabiasz ile się da w wyznaczonym czasie. Po runie podsumowanie i wpis do **Top Runs**.
- **Proceduralne misje** — checkpointy startu i celu pojawiają się losowo na siatce dróg. Kolor (niebieski → czerwony) i odliczanie czasu odzwierciedlają trudność. Im krótszy czas vs odległość, tym wyższy mnożnik nagrody.
- **NPC drogowe** — auta sterowane AI jeżdżą po waypoint-grafie, losują kierunki na skrzyżowaniach, hamują przed przeszkodami, potrafią się cofnąć gdy się zaklinują.
- **Warsztat** — kupowanie aut (sedan / SUV / truck) i kolorów. Wybrane auto wpływa na bonus do nagrody (truck × 1.6, SUV × 1.3, sedan × 1.0).
- **Kary za kolizje** — wjechanie z prędkością w przeszkodę = utrata kilku monet.
- **Persystencja** — postęp i ranking lecą do plików JSON w `Application.persistentDataPath`.

## Sterowanie

- **WSAD / strzałki** — jazda
- **Spacja** — hamulec ręczny
- **H** — klakson
- **Prawy przycisk myszy + ruch** — obrót kamery
- **Esc** — pauza

## Struktura projektu

```
DeliveryMaster/Assets/
├── Scenes/         MainMenu, MainScene, RunSummary
├── Scripts/
│   ├── Car/        CarHandler, InputHandler
│   ├── Run/        RunManager, RunRecord, RunTimerUI, RunSummaryUI, TopRunsUI, RunRow
│   ├── Workshop/   ShopDataManager, CarSpawner, CarData, AccountDisplay, WorkshopUI
│   ├── Editor/     WaypointTools — narzędzie do generowania waypointów
│   └── (root)      MissionManager, CheckpointTrigger, CoinManager, CrashPenalty, ...
├── Prefab/         StartBeacon, EndBeacon, RunRow, ...
├── Resources/      cars.json + grafiki aut
└── Materials/      ...
```

## Zapisywane dane

| Plik | Co tam jest |
|---|---|
| `run_records.json` | Historia Run-ów: nick, data, wynik, auto, liczba dostaw, śr./maks nagroda |
| `user_profile.json` | Konto: saldo monet, posiadane auta i kolory, wybrany pojazd |

Lokalizacja (`Application.persistentDataPath`):
- macOS: `~/Library/Application Support/DefaultCompany/DeliveryMaster/`
- Windows: `%appdata%\..\LocalLow\DefaultCompany\DeliveryMaster\`

## Wymagania

- Unity **6000.0.x** (URP)
- TextMeshPro (część standard Unity)
- Modele 3D miasta z pakietów w `Assets/ithappy/` i `Assets/SimplePoly_City/`

## Uruchomienie
Dwa możliwe podejścia:
---

1. Sklonuj repo, otwórz `DeliveryMaster/` w Unity Hub.
2. Załaduj scenę `MainMenu`.
3. **▶ Play** → klik **Start Run** → dostarczasz przez 3 minuty → ranking.

---

1. Odpalić (tylko na Windows) plik .exe
