# Project Status

Last updated: 2026-04-28

## Current Milestone

Phase 5: Demo Content

Goal: turn the prototype systems into a small playable demo with multiple levels, clearer onboarding, and production-facing menus.

Current focus inside Phase 5: build the shell around the existing prototype, clean up prototype-only runtime wiring, and make scene/content flow work cleanly through main menu, level select, battle, and return paths.

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

### Progression

- Phase 3 task list added.
- Prototype `ProgressionService` added with `PlayerPrefs` save/load.
- Victory grants persistent crystals once per battle.
- Prototype HUD shows persistent crystals and victory reward text.
- Momo Pop has a persistent skill rank.
- Prototype HUD includes an upgrade panel with hero skill and tower family rank buttons.
- Momo Pop rank persists between sessions and improves damage, radius, cooldown, and rank text in battle.
- Bulwark and Sprout skill ranks persist and improve their skill output.
- Momo, Bulwark, and Sprout have persistent hero levels that grant small passive battle bonuses.
- Star, Burst, and Frost tower family ranks persist and improve newly built tower stats.
- Star, Burst, and Frost tower families each have a first explicit specialization choice.
- Starter equipment loadout is saved with weapon, charm, and relic slots.
- Training Charm applies prototype hero skill damage and tower attack speed bonuses.
- ScriptableObject definition classes added for heroes, skills, towers, enemies, equipment, and upgrades.

### Content Pipeline

- Prototype content asset folder added at `Assets/_MomosDefense/Data/Prototype`.
- Generated hero assets for Momo, Bulwark, and Sprout.
- Generated skill assets for Momo Pop, Ground Slam, and Bloom Song.
- Generated tower assets for Star, Burst, and Frost.
- Generated enemy assets for Basic, Tough, Runner, and Armored.
- Generated equipment and upgrade assets.
- Level and wave definition assets added for the prototype battle.
- `WaveSpawner` now consumes `LevelDefinition` and `WaveDefinition` data through an enemy prefab catalog.
- Legacy wave fallback remains for safety.
- Prototype balance CSV added at `Assets/_MomosDefense/Data/Balance/prototype_balance.csv`.
- Editor debug tools added for granting crystals, maxing progression, and resetting progression.

## In Progress

- Phase 5 shell implementation around the prototype battle scene.
- Data-driven runtime cleanup for scene bootstrapping and content flow.
- Main menu and level select work.
- Better shell-to-battle-to-shell flow.
- Prototype-only UI and controls remain acceptable until demo flow requirements are proven.

## Next Up

1. Finish the runtime cleanup needed to support shell screens cleanly.
2. Add a basic main menu and level select.
3. Improve battle entry, completion, and return flow.
4. Plan 3-5 demo levels using the new level/wave data.
5. Add tutorial/onboarding flow for first-time players.

## Remaining Manual Validation

- Manual Unity playtest pass.
- Balance pass after playtest.
- Recheck wave pacing, tower family value, and hero progression feel once the current shell work is in place.

Phase 1 playtest checklist:

- `docs/PLAYTEST_CHECKLIST.md`

Phase 2 playtest checklist:

- `docs/PHASE_2_PLAYTEST_CHECKLIST.md`

## Later Scope

### Phase 2: Three-Hero Vertical Slice

Status: complete for prototype milestone.

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

Status: complete for prototype milestone.

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
- `docs/PHASE_3_TASKS.md`

### Phase 4: Content Pipeline

Status: complete for prototype milestone.

- ScriptableObject content templates.
- Level/wave authoring workflow.
- Enemy stat tables.
- Tower stat tables.
- Hero stat tables.
- Balance spreadsheet or data source.
- Debug tools for progression testing.

Planning doc:

- `docs/PHASE_4_TASKS.md`

## Testing Standard

Before each committed checkpoint:

- Unity must compile.
- Prototype scene generation must run if the scene builder was touched.
- Current Unity logs must be checked for compile errors and blocking runtime errors.
- Blocking errors must be fixed before moving to the next feature.

Latest Android smoke build:

- Output: `Builds/Android/MomosDefensePrototype.apk`
- Result: succeeded
- Size: about 40 MB

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

Phase 2 final validation:

- Date: 2026-04-26
- Command: `PrototypeSceneBuilder.BuildPrototypeScene`
- Result: succeeded.
- Log scan: no compile errors, exceptions, null refs, missing refs, fatal errors, or build failures found.
- Scene check: generated four-wave Phase 2 prototype with three heroes, three tower families, four enemy types, hero XP leveling, placeholder audio, and bottom HUD guidance moved above the control row for readability.
- Android smoke build: succeeded, output `Builds/Android/MomosDefensePrototype.apk`, about 40 MB.
- Notes: a hands-on feel pass is still useful before locking demo balance, but Phase 2 is complete enough to move into Phase 3 progression systems.

Latest Phase 3 progression slice:

- Date: 2026-04-26
- Command: `PrototypeSceneBuilder.BuildPrototypeScene`
- Result: succeeded.
- Log scan: no compile errors, exceptions, null refs, missing refs, fatal errors, or build failures found.
- Scene check: generated `Progression Service`, crystal counter, victory reward text, and Momo Pop upgrade button.
- Android smoke build: succeeded, output `Builds/Android/MomosDefensePrototype.apk`, about 40 MB.
- Notes: Phase 3 Slice 1 and Slice 2 are implemented as prototype HUD controls; a cleaner upgrade screen is next.

Latest expanded Phase 3 scene generation:

- Date: 2026-04-26
- Command: `PrototypeSceneBuilder.BuildPrototypeScene`
- Result: succeeded.
- Log scan: no compile errors, exceptions, null refs, missing refs, fatal errors, or build failures found.
- Scene check: generated upgrade panel, reset control, all hero skill rank buttons, all tower family rank buttons, persistent hero level/passive hooks, starter equipment bonuses, and ScriptableObject definition scripts.
- Android smoke build: succeeded, output `Builds/Android/MomosDefensePrototype.apk`, about 40 MB.
- Notes: Phase 3 is complete for the prototype milestone and ready to move into Phase 4 content pipeline work.

Latest Phase 4 content pipeline validation:

- Date: 2026-04-26
- Command: `PrototypeSceneBuilder.BuildPrototypeScene`
- Result: succeeded.
- Log scan: no compile errors, exceptions, null refs, missing refs, fatal errors, or build failures found.
- Scene check: generated `PrototypeLevel01`, four wave assets, prototype hero/skill/tower/enemy/equipment/upgrade assets, and a scene wired to `LevelDefinition` plus enemy prefab catalog.
- Android smoke build: succeeded, output `Builds/Android/MomosDefensePrototype.apk`, about 40 MB.
- Notes: Phase 4 is complete for the prototype milestone and ready to move into Phase 5 demo content.

## Recent Stable Checkpoints

- `a85956e` - Set up Unity project foundation.
- `dd62f0c` - Add prototype HUD and readability pass.
- `7aa2514` - Add Momo Pop prototype skill.
- `fd646a0` - Add prototype tower build nodes.
- `7c7a51b` - Add prototype win loss restart flow.
