# Agent Handoff: Momo's Defense

Last updated: 2026-04-25

## Read This First

This repository is a Unity 6 Android-first prototype for **Momo's Defense**, a cute stylized 3D mobile tower defense game inspired by classic tower defense and three-hero control systems.

The user wants careful incremental development:

- Plan small chunks.
- Implement one chunk.
- Test with Unity.
- Fix errors before moving on.
- Commit/push stable checkpoints.
- Keep docs updated.

Do not attempt to build the full game in one pass.

## Repository

- Local path: `C:\Users\masil\Desktop\PROJECTS\momo's defense`
- GitHub: `https://github.com/Ensong829/Momo-s-Defense`
- Main branch: `main`
- Unity version: `6000.4.4f1`
- Target platform: Android first

Git may need full path on this machine:

```powershell
& 'C:\Program Files\Git\cmd\git.exe' status --short
```

Unity command path:

```powershell
& "C:\Program Files\Unity\Hub\Editor\6000.4.4f1\Editor\Unity.com"
```

## Important Docs

Read these before coding:

- `docs/PROJECT_STATUS.md`
- `docs/ROADMAP.md`
- `docs/PLAYTEST_CHECKLIST.md`
- `docs/WORKING_RULES.md`
- `docs/PHASE_2_VERTICAL_SLICE_PLAN.md`
- `docs/PHASE_3_PROGRESSION_PLAN.md`

## Current State

Phase 1 first playable prototype is implemented enough for manual playtest. The project now has a working Phase 2 prototype with three selectable heroes in code and generated scene content.

Latest automated scene generation on 2026-04-25 succeeded after a small runtime hardening pass. The log scan found no compile errors, exceptions, null refs, missing refs, fatal errors, or build failures; only the known Unity `abort_threads` shutdown noise appeared.

Latest Phase 2 Slice 1 scene generation on 2026-04-25 also succeeded. The generated scene now includes a `Hero Selection Manager`, `Momo Portrait Button`, Momo `Selection Ring`, and `EventSystem`.

Latest three-hero scene generation on 2026-04-25 succeeded as well. The generated scene now includes `Bulwark Prototype Hero`, `Sprout Prototype Hero`, portrait buttons for all three heroes, and a selected-hero `Skill Button`.

Current prototype scene:

- `Assets/_MomosDefense/Scenes/Prototype_MomoDefense.unity`

Scene generation menu:

- `Momo's Defense > Build Prototype Scene`

Android build menu:

- `Momo's Defense > Build Android Prototype`

Android smoke build output:

- `Builds/Android/MomosDefensePrototype.apk`

## Implemented Features

### Foundation

- Unity project created.
- Android Build Support installed.
- Project switched to Android.
- GitHub remote configured.
- Git LFS rules configured.
- Unity folders created under `Assets/_MomosDefense`.

### Prototype Gameplay

- Graybox map.
- Fixed enemy path.
- Wave spawner.
- Start Wave / Next Wave control.
- Basic enemy type.
- Tough enemy type from wave 2 onward.
- Lives and gold.
- Victory/defeat states.
- Restart button.
- Enemy death/leak events initialized defensively.
- Build nodes.
- Tower placement.
- Tower placement feedback.
- One starter tower.
- One tower upgrade step.
- Tower build and upgrade attempts are blocked after the battle ends.

### Momo

- Momo is Hero 001 and main character.
- Selectable hero state.
- Prototype portrait button.
- Selection ring.
- Click/tap movement.
- Only selected Momo receives movement commands.
- UI taps no longer issue movement commands.
- Auto attack.
- Momo Pop active skill.
- Momo Pop damages and slows enemies.
- Selected-hero HUD skill button.

### Additional Heroes

- Bulwark placeholder hero added.
- Bulwark uses Ground Slam.
- Sprout placeholder hero added.
- Sprout uses Bloom Song to buff nearby towers.

### UI

- Lives, gold, wave display.
- Momo Pop button.
- Start Wave / Next Wave button.
- Restart button.
- Temporary message text.
- Basic objective prompt.
- 1920x1080 UI reference scaling.

## Testing Rules

Before committing:

1. Run Unity compile/scene generation when gameplay, scene builder, prefab, or project settings change.
2. Inspect logs for compile errors, exceptions, null refs, missing refs, and fatal errors.
3. Fix blocking issues before moving forward.
4. Commit and push stable checkpoints.

Scene generation command:

```powershell
& "C:\Program Files\Unity\Hub\Editor\6000.4.4f1\Editor\Unity.com" -batchmode -quit -projectPath "C:\Users\masil\Desktop\PROJECTS\momo's defense" -executeMethod MomosDefense.Editor.PrototypeSceneBuilder.BuildPrototypeScene -logFile "C:\Users\masil\Desktop\PROJECTS\momo's defense\unity-build-prototype.log"
```

Check Unity log:

```powershell
Select-String -Path 'unity-build-prototype.log' -Pattern 'error CS|Assets.*error|Exception|NullReferenceException|MissingReferenceException|fatal' -CaseSensitive:$false
```

Android smoke build command:

```powershell
& "C:\Program Files\Unity\Hub\Editor\6000.4.4f1\Editor\Unity.com" -batchmode -quit -projectPath "C:\Users\masil\Desktop\PROJECTS\momo's defense" -executeMethod MomosDefense.Editor.PrototypeAndroidBuilder.BuildAndroidPrototype -logFile "C:\Users\masil\Desktop\PROJECTS\momo's defense\unity-android-build.log"
```

## Batch Log Noise

Unity batch mode often logs licensing/access-token and thread cleanup messages such as:

- `Access token is unavailable`
- `Failed to handshake`
- `abort_threads`

These have not been blocking so far. Treat script compile errors, Unity exceptions, null refs, missing refs, build failures, and scene generation failures as blocking.

## Do Not Commit

Generated/local folders are ignored and should stay out of Git:

- `Library/`
- `Logs/`
- `Temp/`
- `Builds/`
- `.utmp/`
- `UserSettings/`

The APK output is ignored by `.gitignore`.

## Next Recommended Work

The user approved moving forward before a full manual playtest. The next step is to playtest the three-hero slice and tune it instead of expanding scope immediately.

Next steps:

1. Open Unity and run `docs/PLAYTEST_CHECKLIST.md`.
2. Confirm wave start, restart, tower build/upgrade, hero switching, and all three skills work in editor playtest.
3. Record balance/bug notes.
4. Fix blocking issues.
5. Tune Phase 2 readability and combat feel before moving to larger content additions.

## Phase 2 Direction

Use `docs/PHASE_2_VERTICAL_SLICE_PLAN.md`.

First implementation slice should be:

- Done in code and automated scene generation: three selectable heroes, portrait buttons, one skill per hero, and selected-hero skill HUD.
- Still needs manual playtest: ensure wave start, tower building, switching, and all three skills work cleanly in live play.

## Recent Stable Commits

- `f1de353` - Add Phase 2 and progression plans
- `5d8f4c2` - Add Phase 1 playtest checklist
- `d21d6b7` - Add Android prototype smoke build
- `be3e295` - Improve prototype mobile framing
- `72a016f` - Add tough enemy prototype type
- `699432e` - Add starter tower upgrade step
- `9d25901` - Add tower placement feedback
- `618b479` - Add prototype wave start control
- `7c7a51b` - Add prototype win loss restart flow
- `fd646a0` - Add prototype tower build nodes
- `7aa2514` - Add Momo Pop prototype skill

## User Preferences

- Android first.
- Cute stylized fantasy.
- Classic tower defense camera.
- Long-term commercial game, but built through manageable milestones.
- Premium or cosmetics-first monetization, not pay-to-win.
- Upgrade systems are very important: heroes, towers, and equipment.
- User is okay with placeholder assets while gameplay is proven.
