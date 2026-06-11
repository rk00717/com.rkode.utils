# Changelog
All notable changes to `com.rkode.utils` will be documented here.

Format follows [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).  
Versioning follows [SemVer](https://semver.org/spec/v2.0.0.html).

---

## [0.4.0] — 2026-06-11

### Added
- `ProjectScopedKey` — scopes `EditorPrefs` keys per project using `PlayerSettings.productGUID`. Prevents settings leaking across projects on the same machine.

### Changed
- `StartupConstants` keys are now project-scoped via `ProjectScopedKey`
- `StartupSettingsSection` toggle no longer fires every frame while settings window is open
- `Bootstrap` scene warnings now correctly check Build Settings instead of null references
- `ProjectConfig` no longer marks assets dirty on every script reload

### Fixed
- `Bootstrap` runtime build path was passing a `SceneAsset` reference instead of a scene name string

---

## [0.3.0] — 2026-06-05

### Changed
- Added Conditional Log inside Logger
- Exposed Logger.IsLoggingEnabled to be able to control Logger from outside of Logger.

---

## [0.2.0] — 2026-06-05

### Changed
- `QuitApplication`: extracted quit logic into public static `Execute()` method
- Instance method `Quit()` now delegates to `Execute()` — Inspector wiring unaffected
- Other classes can now call `QuitApplication.Execute()` without a MonoBehaviour reference

---

## [0.1.0-alpha] — 2026-06-03

### Added

**Runtime**
- `Singleton<T>` — thread-safe, quit-safe, test-resettable generic MonoBehaviour singleton base
- `StateMachine<T>` — generic state machine with same-state block and forced transition support
- `ObjectPool<T>` — generic object pool with preloading, active/pooled tracking, and `IPoolable` notification
- `IPoolable` — interface for pool-aware objects (`OnSpawn` / `OnDespawn`)
- `IState` — interface for state machine states (`OnEnter` / `OnTick` / `OnExit`)
- `SaveDataHelper` — JSON persistence with custom `.rkode` file extension
- `SceneLoader` — async scene loading with progress callback, additive support, loading scene injection
- `SceneLifeCycleBase` — abstract base for per-scene lifecycle management
- `UIPanelBase` — abstract base class for UI panels
- `GameStartupHelper` / `Bootstrap` — bootstrap scene redirect for correct start scene in Editor and builds
- `BezierCurve` — cubic Bezier math utility
- `ColorUtility` — color math helpers shared across editor packages
- `Loggger` — editor-only extension method logging
- `IDataParser` / `UniversalJsonParser` — swappable JSON serialization interface
- `FakeLoader` — simulated async loading for UI testing
- `QuitApplication` — platform-safe application quit
- `PlayerNameUtils` — shared player name utilities
- `ProjectConfig` — ScriptableObject for project-wide configuration
- `StartupConstants` — shared startup configuration constants

**Editor**
- `RoundedPanelLayout` — IMGUI rounded panel container layout helper
- `RoundedPanelStyle` — GUIStyle definitions for rounded panels
- `PaletteButtonStyle` — shared styled button renderer for editor windows
- `SceneUtility` — editor-side scene helpers
- `SettingsProvider` / `ISettingsSection` / `SettingsRegister` — modular Project Settings registration system
- `StartupMenu` — editor menu items for GameStartup configuration
- `SceneLoaderSettingsSection` / `StartupSettingsSection` — built-in settings sections

**Tests**
- `SingletonTests` — Play Mode tests for singleton lifecycle and reset behaviour

**Docs**
- `README.md` — full API reference, installation instructions, quick start, ecosystem table
- `CHANGELOG.md` — this file

**Chore**
- `.gitignore` — Unity package standard ignore rules

### Known Issues
- None identified at this stage

### Compatibility
- Unity 2021.3 LTS and above
- Requires `com.unity.nuget.newtonsoft-json` 3.0.2
