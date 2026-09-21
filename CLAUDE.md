# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

Ninja Soul — a 3D isometric action game built in Unity 6000.0.80f1 (Unity 6), using URP (Universal Render Pipeline 17.0.4), the new Input System, and Cinemachine. There is no README content beyond the project name; treat this file as the primary orientation doc.

## Working in this repo

This is a Unity project, not a CLI-buildable package — there are no npm/dotnet build or test scripts to run from the shell. Development happens through the Unity Editor:

- Open the project in Unity Editor 6000.0.80f1 (exact version pinned in `ProjectSettings/ProjectVersion.txt`; opening with a different version will trigger a reimport and can cause asset churn — avoid unless intentionally upgrading).
- Play Mode in the Editor is the primary way to run/test the game. There is no dedicated automated test suite in `Assets` (no EditMode/PlayMode test assemblies exist yet, despite `com.unity.test-framework` being installed as a package).
- C# scripts can be edited directly with Read/Edit tools; Unity will recompile them next time the Editor regains focus or on `-batchmode` command-line builds. There is no separate linter config — follow existing code style (see below).
- `.csproj`/`.sln(x)` files at the repo root are Unity-generated; don't hand-edit them.
- Scenes (`.unity`) and prefabs (`.prefab`) are YAML text but are meant to be edited through the Editor — avoid hand-editing them unless making a small, well-understood targeted change (e.g. a GUID reference), since merge/serialization mistakes easily corrupt them.

## Architecture

### Global systems (singletons)
Most cross-cutting systems are `Singleton<T>` (`Assets/Scripts/Utilities/Singleton.cs`) — lazy `DontDestroyOnLoad` MonoBehaviours. Two are force-instantiated at startup from `Resources/Systems/` via `Bootstrapper.cs` (`RuntimeInitializeOnLoadMethod`): `TransitionManager` and `PlayerData`. Others (e.g. `AudioManager`, `SaveManager`, `EventManager`) are lazily created on first access.

`GameManager` (`Assets/Scripts/Gameplay/GameManager.cs`) is a plain (non-singleton) `instance`-pattern MonoBehaviour holding the global `GameState` enum (`GAMEPLAY`, `WEAPON_SELECTION`, `CUTSCENE`, `PAUSE`, `GAMEOVER`, `CLEAR`). Most input-gathering code checks `GameManager.instance.currentGameState` before acting, and state transitions are broadcast via the messaging system below — check this before wiring up new gameplay input.

### Messaging / pub-sub
Cross-system communication goes through a custom `MessagingCenter` (`Assets/Scripts/Utilities/MessagingCenter.cs`, an in-house pattern similar to Xamarin.Forms' MessagingCenter) rather than direct references or UnityEvents. Each publisher defines `public const string Message...` names and calls `MessagingCenter.Send(this, MessageName, payload)`; subscribers call `MessagingCenter.Subscribe<TSender>(this, MessageName, callback)` in `Awake`/`OnEnable` and must `Unsubscribe` in `OnDestroy`/`OnDisable`. When adding new decoupled systems (game state changes, stage-clear events, HUD requests, dialogue triggers), follow this existing convention rather than introducing a different event mechanism.

### Player — finite state machine
`PlayerStateMachine` (`Assets/Scripts/Player/PlayerStateMachine.cs`) drives player movement/combat via a classic FSM: `PlayerBaseState` (`Assets/Scripts/Player/States/`) subclasses (`Idle`, `Walk`, `Run`, `Dash`, `Combat`, `Soul`) are created through `PlayerStateFactory` and swapped with `ChangeState()`. Movement is isometric — raw input is transformed via the `Isometric.ToIso()` extension (45° rotation matrix) before being applied. Input comes from `InputActionReference`s wired in the Inspector (`Assets/Scripts/Player/InputManager.inputactions`), gathered into `Pressed*`/`AxisInput` fields, and gated by `GameManager.currentGameState == GAMEPLAY`.

Combat combos are data-driven: `ComboFactory`/`Combo` classes under `Assets/Scripts/Player/Combos/`, with per-weapon combo subfolders (`Chokuto`, `Javelin`, `Knuckles`, `Sickles`, `Sword`) selected via `PlayerEquipment`.

### Enemies — mirrored FSM
`EnemyStateMachine` + `EnemyBaseState` subclasses (`Idle`, `Chase`, `Combat`, `Prepare`, `Knock`) under `Assets/Scripts/Enemy/States/` follow the same FSM pattern as the player. `EnemyManager` tracks per-enemy state and fires `MessageOnEnemyDead`, which `EventManager` listens for to progress eliminate-type objectives. `Boss.cs` extends this for boss encounters.

### Damage / interaction contracts
`Assets/Scripts/Interfaces/` defines the cross-cutting contracts: `IDamageable`, `IDamageDealer`, `IInteract`. Anything that can take damage, deal damage, or be interacted with should implement these rather than being special-cased.

### Stages
Each of the game's stages (Genbu, Seiryu, Byakko, Suzaku, Hikari, Final) has its own folder under `Assets/Scripts/Stages/<Stage>/` with a `<Stage>Manager` that fires a stage-specific `MessageClear<Stage>Stage` message. `GameManager` subscribes to each of these individually to update `PlayerData` (unlocking weapons/clans) and trigger `LevelComplete()` — when adding a new stage, follow this same per-stage-manager + clear-message pattern and wire the new message into `GameManager`.

### Persistence
`PlayerData` (Bootstrapper-instantiated singleton) holds the live/session player state (hp, soul, spawn point, per-stage completion flags, equipped weapons). `SaveManager` (`Assets/Scripts/Utilities/SaveManager.cs`) persists a `Player` model to disk. `GameManager.LevelComplete()` shows the read/write pattern: mutate `PlayerData`, then `SaveManager.Instance.Save(player)`.

### Dialogue / cutscenes / tutorial
`Assets/Scripts/Dialogue/` (with per-cutscene/tutorial dialogue data under `Dialogues/`) and `Assets/Scripts/Cutscene/`, `Assets/Scripts/Tutorial/` drive scripted sequences; these also flip `GameManager` into `CUTSCENE` state via the messaging center while playing.

### Scenes
Key scenes live in `Assets/Scenes/`: `Menu`, `Intro`, `HUD` (persistent overlay), `Level`, `Tutorial`, `Trailer`, plus one scene per stage under `Assets/Scenes/Stages/`.

### Third-party / vendored code
Large parts of `Assets/` are vendored asset-store packages, not project code — treat these as read-only unless a task specifically targets them: `Tools/LeanTween`, `Tools/ModularMotion`, `JMO Assets/Cartoon FX (legacy)`, `Models/Stylized Water For URP`, `QuickOutline`, `ToonyColorsPro`, `Samples/Input System`. Project-owned gameplay code lives almost entirely under `Assets/Scripts/`.

## Conventions observed in existing code
- Private serialized fields use a leading underscore (`_speed`) with a public expression-bodied property exposing them (`public float Speed => _speed`).
- State/manager classes separate the `MonoBehaviour` "context" (fields, Unity lifecycle) from the FSM state classes, which are plain C# classes holding a reference back to the context.
- Game/stage/system messages are defined as `public const string Message...` on the class that raises them, not in a separate constants file.
