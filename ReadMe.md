<div align="center">

# RKode Utility
### `com.rkode.utils` · v0.1.0-alpha · Unity 2021.3+

**The foundation of the RKode ecosystem.**  
Runtime and editor utilities built to be copied zero times — import once, use everywhere.

[![Unity](https://img.shields.io/badge/Unity-2021.3+-0a0c08?style=flat-square&logo=unity&logoColor=c9933a)](https://unity.com)
[![License](https://img.shields.io/badge/License-MIT-0a0c08?style=flat-square&logoColor=c9933a)](LICENSE)
[![Version](https://img.shields.io/badge/Version-0.1.0--alpha-0a0c08?style=flat-square&logoColor=c9933a)]()

</div>

---

## What's Inside

### Runtime

| Class | What it does |
|---|---|
| `Singleton<T>` | Thread-safe, quit-safe, test-resettable generic singleton base. Override `OnAwake()` instead of `Awake()`. |
| `StateMachine<T>` | Generic state machine. Blocks same-state transitions by default. `ChangeState()`, `Tick()`. |
| `ObjectPool<T>` | Generic object pool with preloading, `IPoolable` notification, and active/pooled tracking. |
| `SaveDataHelper` | JSON persistence with custom `.rkode` extension. `SaveData<T>()` / `LoadData<T>()`. |
| `SceneLoader` | Async scene loading with progress callback, additive support, loading scene injection. |
| `SceneLifeCycleBase` | Abstract base for per-scene lifecycle management. |
| `UIPanelBase` | Abstract base class for UI panels. |
| `GameStartupHelper` | Bootstrap scene redirect — ensures correct start scene in Editor and builds. |
| `BezierCurve` | Cubic Bezier math utility. Used by Placement Toolkit for path placement. |
| `ColorUtility` | Color math helpers shared across editor packages. |
| `Loggger` | Editor-only extension method logging with per-project on/off toggle. |
| `IDataParser` / `UniversalJsonParser` | Swappable serialization interface. Drop-in JSON backend. |
| `IPoolable` | Interface for pool-aware objects. `OnSpawn()` / `OnDespawn()`. |
| `IState` | Interface for state machine states. `OnEnter()` / `OnTick()` / `OnExit()`. |
| `FakeLoader` | Simulated async loading for UI testing without actual scene loads. |
| `QuitApplication` | Platform-safe application quit. |
| `PlayerNameUtils` | Shared player name utilities. |
| `ProjectConfig` | ScriptableObject for project-wide config (loading scene name, etc). |

### Editor

| Class | What it does |
|---|---|
| `RoundedPanelLayout` | IMGUI layout helper for rounded panel containers — used by VERO and Level Editor. |
| `RoundedPanelStyle` | GUIStyle definitions for rounded panels. |
| `PaletteButtonStyle` | Shared styled button renderer for editor windows. |
| `SceneUtility` | Editor-side scene helpers. |
| `SettingsProvider` | Base for registering entries in Unity Project Settings. |
| `ISettingsSection` | Interface for modular settings sections. |
| `StartupMenu` | Editor menu items for GameStartup configuration. |

---

## Installation

### Via Git URL (recommended)

Add to your project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.rkode.utils": "https://github.com/rk00717/com.rkode.utils.git",
    "com.unity.nuget.newtonsoft-json": "3.0.2"
  }
}
```

> **Requires** `com.unity.nuget.newtonsoft-json` 3.0.2 — add it alongside.

### Manual

Clone this repo into your project's `Packages/` folder:

```bash
cd YourProject/Packages
git clone https://github.com/rk00717/com.rkode.utils.git com.rkode.utils
```

---

## Quick Start

```csharp
// Singleton
public class GameManager : Singleton<GameManager> {
    protected override void OnAwake() {
        // Your init here
    }
}

// State machine
var fsm = new StateMachine<IGameState>();
fsm.ChangeState(new MenuState());
fsm.Tick(); // call in Update()

// Object pool
var pool = new ObjectPool<EnemyView>(prefab, container, preloadCount: 10);
var enemy = pool.Get();
pool.ReturnToPool(enemy);

// Save / load
SaveDataHelper.SaveData(playerData, "player");
var loaded = SaveDataHelper.LoadData<PlayerData>("player"); // → player.rkode

// Scene loading
SceneLoader.Instance.LoadScene("GameScene", onLoadCallback: OnSceneReady);
```

---

## Dependencies

| Package | Version |
|---|---|
| `com.unity.nuget.newtonsoft-json` | 3.0.2 |

---

<div align="center">
<sub>Built by <a href="https://ronik.dev">RKode Studio</a> · <a href="https://github.com/rk00717">rk00717</a></sub>
</div>
