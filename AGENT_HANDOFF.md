# Agent Handoff: Momo's Defense

Last updated: 2026-04-26

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

Phase 1 first playable prototype is implemented enough for manual playtest. Phase 2 is complete for the prototype milestone with three selectable heroes, multiple tower families, four enemy types, in-battle hero leveling, placeholder audio, a four-wave level pass, and a readable landscape HUD control row. Phase 3 has started with persistent crystals, victory rewards, and a Momo Pop skill-rank upgrade loop.

Latest automated scene generation on 2026-04-25 succeeded after a small runtime hardening pass. The log scan found no compile errors, exceptions, null refs, missing refs, fatal errors, or build failures; only the known Unity `abort_threads` shutdown noise appeared.

Latest Phase 2 Slice 1 scene generation on 2026-04-25 also succeeded. The generated scene now includes a `Hero Selection Manager`, `Momo Portrait Button`, Momo `Selection Ring`, and `EventSystem`.

Latest three-hero scene generation on 2026-04-25 succeeded as well. The generated scene now includes `Bulwark Prototype Hero`, `Sprout Prototype Hero`, portrait buttons for all three heroes, and a selected-hero `Skill Button`.

Latest Phase 2 polish scene generation on 2026-04-25 also succeeded. Runner enemies were added to later waves, clicking a hero in the world now selects that hero, and clicking a built tower should trigger its upgrade path again.

Latest Phase 2 final validation on 2026-04-26 succeeded. The generated scene now includes a `Tower Build Manager`, three tower family buttons, shared hero XP leveling, armored enemies in later waves, placeholder procedural audio feedback through scripts, four tuned waves, and the objective prompt moved above the bottom controls.

Latest Phase 3 scene generation and Android smoke build on 2026-04-26 succeeded. The generated scene now includes a `Progression Service`, crystal counter, victory reward text, and a Momo Pop upgrade button. `ProgressionService` saves crystals and Momo Pop rank through `PlayerPrefs`.

Current prototype scene:

- `Assets/_MomosDefense/Scenes/Prototype_MomoDefense.unity`

Scene generation menu:

- `Momo's Defense > Build Prototype Scene`

Android build menu:

- `Momo's Defense > Build Android Prototype`

Android smoke build output:

- `Builds/Android/MomosDefensePrototype.apk`
- Latest Phase 2 Android smoke build succeeded; output is about 40 MB.

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
- Runner enemy type from wave 2 onward.
- Armored enemy type from wave 3 onward.
- Lives and gold.
- Victory/defeat states.
- Restart button.
- Enemy death/leak events initialized defensively.
- Build nodes.
- Tower placement.
- Tower placement feedback.
- One starter tower.
- Burst tower family.
- Frost tower family.
- One tower upgrade step.
- Three-tier tower upgrade flow through tower stats.
- Tower build and upgrade attempts are blocked after the battle ends.
- Clicking a built tower triggers upgrade interaction through its build node.

### Momo

- Momo is Hero 001 and main character.
- Selectable hero state.
- Prototype portrait button.
- Selection ring.
- Click/tap movement.
- Only selected Momo receives movement commands.
- UI taps no longer issue movement commands.
- Clicking heroes in the world changes selection.
- Auto attack.
- Momo Pop active skill.
- Momo Pop damages and slows enemies.
- Selected-hero HUD skill button.
- Shared in-battle hero XP and leveling.

### Additional Heroes

- Bulwark placeholder hero added.
- Bulwark uses Ground Slam.
- Sprout placeholder hero added.
- Sprout uses Bloom Song to buff nearby towers.

### Audio

- Placeholder procedural music and SFX added in scripts.
- Build, upgrade, selection, skill, wave start, victory, and defeat sounds are supported.

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

The user asked to finish Phase 2. The prototype milestone is now complete and the next recommended work is Phase 3 progression foundation.

Next steps:

1. Split prototype progression controls into a cleaner upgrade screen.
2. Add debug reset controls for local progression testing.
3. Add persistent progression for Bulwark, Sprout, and tower families.
4. Add first tower family rank upgrades.
5. Run `docs/PHASE_2_PLAYTEST_CHECKLIST.md` before any public/demo build to capture hands-on feel notes.

## Phase 2 Direction

Use `docs/PHASE_2_VERTICAL_SLICE_PLAN.md`.

First implementation slice should be:

- Done in code and automated scene generation: three selectable heroes, three tower families, four enemy types, hero leveling, selected-hero skill HUD, and placeholder audio.
- Still useful to playtest: tune wave pacing, tower family value, and hero level pacing in live play.

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
