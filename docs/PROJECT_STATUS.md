# Project Status

Last updated: 2026-04-25

## Current Milestone

Phase 2: Three-Hero Vertical Slice

Goal: prove three-hero control, per-hero skills, and readable battle UI without breaking the first playable battle loop.

## Completed

### Project Foundation

- Unity project created with editor version `6000.4.4f1`.
- Android Build Support installed.
- Project switched to Android target.
- Git repository initialized.
- GitHub remote configured: `https://github.com/Ensong829/Momo-s-Defense`.
- Git LFS rules added for future binary assets.
- Core project folders created under `Assets/_MomosDefense`.
- Living docs created in `docs`.

### Prototype Scene

- Prototype scene created: `Assets/_MomosDefense/Scenes/Prototype_MomoDefense.unity`.
- Graybox map created.
- Classic fixed/orthographic camera created.
- Camera widened for landscape mobile framing.
- Readable placeholder colors/materials added.
- Visible enemy path added.
- Scene can be regenerated from `Momo's Defense > Build Prototype Scene`.

### Core Battle Loop

- Basic `GameState` with lives and gold.
- Basic `Health` component.
- Enemy path follower.
- Wave spawner.
- Basic enemy and tough enemy prefabs.
- Fast runner enemy prefab for later-wave pressure.
- Armored enemy prefab for late-wave durability pressure.
- Tough enemies appear from wave 2 onward.
- Runner enemies appear from wave 2 onward.
- Armored enemies appear from wave 3 onward.
- Waves wait for player input before starting.
- HUD has Start Wave / Next Wave control.
- Enemy leaks reduce lives.
- Defeated enemies grant gold.
- HUD shows lives, gold, wave state, and result text.
- Victory appears when all waves are cleared.
- Defeat appears when lives reach zero.
- Restart button appears after victory or defeat.
- Restart reloads the prototype scene.
- Compact objective prompt added for first-play guidance.
- HUD uses a 1920x1080 reference resolution for mobile landscape scaling.
- Runtime enemy death/leak events are initialized defensively to avoid null listener issues during wave cleanup.

### Momo

- Momo is implemented as the first playable hero placeholder.
- Momo can be selected through a prototype portrait button.
- Momo has a visible selection ring.
- Momo can move by click/tap-style input.
- Only the selected hero receives click/tap movement commands.
- UI taps are ignored by hero movement.
- Momo auto-attacks nearby enemies.
- Momo has first active skill: `Momo Pop`.
- Momo Pop damages nearby enemies.
- Momo Pop slows nearby enemies.
- Momo Pop can be activated through HUD button or Space in editor.
- HUD shows the selected hero skill cooldown.

### Hero Selection

- Prototype hero selection manager added.
- Momo is registered as the starting selected hero.
- Generated scene includes an `EventSystem` for prototype UI input.
- Generated HUD includes portrait buttons for Momo, Bulwark, and Sprout.
- `1`, `2`, and `3` select heroes in the editor.
- Clicking a hero in the world selects that hero.

### Additional Heroes

- Bulwark placeholder added as the second controllable hero.
- Bulwark uses `Ground Slam`, a short-range area hit with heavy slow.
- Sprout placeholder added as the third controllable hero.
- Sprout uses `Bloom Song`, a nearby tower buff skill.
- Heroes gain simple in-battle levels from shared enemy XP.

### Towers

- Starter tower prefab created.
- Burst tower family added.
- Frost tower family added.
- Tower targets and damages nearby enemies.
- Starter towers can be upgraded once by clicking the occupied build node.
- Upgraded towers gain damage, range, attack speed, size, and a color change.
- Build nodes added to the prototype map.
- Player can click a build node to spend gold and place a starter tower.
- HUD shows feedback for successful builds, occupied nodes, and insufficient gold.
- Build nodes mark themselves as occupied after use.
- Clicking build nodes no longer moves Momo.
- Build and upgrade attempts are blocked after the battle ends.
- Clicking a built tower routes upgrade interaction back to its build node.
- Towers can receive a temporary attack buff from Sprout.
- HUD supports tower family selection buttons for Star, Burst, and Frost towers.

### Audio

- Placeholder procedural background music added.
- Placeholder SFX added for selection, building, upgrading, skills, wave start, victory, and defeat.

## In Progress

- Phase 2 feature-complete prototype.
- Balance and readability tuning.
- Prototype-only UI and controls.

## Next Up

1. Focused balance playtest in Unity Editor.
2. Tune tower family costs, enemy pressure, and hero level pacing.
3. Decide whether to officially call Phase 2 complete and begin Phase 3 progression systems.

## Remaining Phase 1 Tasks

- Manual playtest pass in Unity Editor.
- Phase 1 balance pass after playtest.

Playtest checklist:

- `docs/PLAYTEST_CHECKLIST.md`

## Later Scope

### Phase 2: Three-Hero Vertical Slice

- Three controllable heroes.
- Hero portraits/selection. Prototype implemented.
- One active skill per hero. Prototype implemented.
- Three tower families. Prototype implemented.
- Four enemy types. Prototype implemented.
- In-battle hero leveling. Prototype implemented.
- Tower upgrade tiers. Prototype implemented.
- First pass level art.
- Placeholder music and SFX. Prototype implemented.

Planning doc:

- `docs/PHASE_2_VERTICAL_SLICE_PLAN.md`

### Phase 3: RPG Progression Foundation

- Persistent hero levels.
- Hero skill upgrades.
- Tower family upgrades.
- Equipment definitions.
- Equipment inventory.
- Save/load.
- Reward screen.
- Upgrade menus.

Planning doc:

- `docs/PHASE_3_PROGRESSION_PLAN.md`

## Testing Standard

Before each committed checkpoint:

- Unity must compile.
- Prototype scene generation must run if the scene builder was touched.
- Current Unity logs must be checked for compile errors and blocking runtime errors.
- Blocking errors must be fixed before moving to the next feature.

Latest Android smoke build:

- Output: `Builds/Android/MomosDefensePrototype.apk`
- Result: succeeded
- Size: about 29 MB

Latest automated scene generation:

- Date: 2026-04-25
- Command: `PrototypeSceneBuilder.BuildPrototypeScene`
- Result: succeeded
- Log scan: no compile errors, exceptions, null refs, missing refs, fatal errors, or build failures found.
- Notes: Unity still prints known `abort_threads` shutdown noise in batch mode.

Latest Phase 2 Slice 1 automated scene generation:

- Date: 2026-04-25
- Command: `PrototypeSceneBuilder.BuildPrototypeScene`
- Result: succeeded
- Log scan: no compile errors, exceptions, null refs, missing refs, fatal errors, or build failures found.
- Scene check: generated `Hero Selection Manager`, `Momo Portrait Button`, `Selection Ring`, and `EventSystem`.
- Notes: manual playtest is still needed before adding two more heroes.

Latest automated three-hero scene generation:

- Date: 2026-04-25
- Command: `PrototypeSceneBuilder.BuildPrototypeScene`
- Result: succeeded
- Log scan: no compile errors, exceptions, null refs, missing refs, fatal errors, or build failures found.
- Scene check: generated `Bulwark Prototype Hero`, `Sprout Prototype Hero`, `Bulwark Portrait Button`, `Sprout Portrait Button`, and `Skill Button`.
- Notes: Unity still prints known `abort_threads` shutdown noise in batch mode. Manual playtest is still needed before calling Phase 2 finished.

Latest Phase 2 polish scene generation:

- Date: 2026-04-25
- Command: `PrototypeSceneBuilder.BuildPrototypeScene`
- Result: succeeded
- Log scan: no compile errors, exceptions, null refs, missing refs, fatal errors, or build failures found.
- Scene check: generated `runnerEnemyPrefab` reference and rebuilt the scene after tower-upgrade interaction fixes.
- Notes: tower upgrades should now work by clicking the built tower itself.

Latest Phase 2 completion scene generation:

- Date: 2026-04-25
- Command: `PrototypeSceneBuilder.BuildPrototypeScene`
- Result: succeeded
- Log scan: no compile errors, exceptions, null refs, missing refs, fatal errors, or build failures found.
- Scene check: generated `Tower Build Manager`, tower family buttons, and the rebuilt feature-complete Phase 2 prototype scene.
- Notes: only known Unity `abort_threads` shutdown noise appeared.

## Recent Stable Checkpoints

- `a85956e` - Set up Unity project foundation.
- `dd62f0c` - Add prototype HUD and readability pass.
- `7aa2514` - Add Momo Pop prototype skill.
- `fd646a0` - Add prototype tower build nodes.
- `7c7a51b` - Add prototype win loss restart flow.
