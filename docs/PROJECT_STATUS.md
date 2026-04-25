# Project Status

Last updated: 2026-04-25

## Current Milestone

Phase 1: First Playable Prototype

Goal: make a rough but complete tower defense match featuring Momo, one enemy path, buildable towers, basic waves, basic economy, and win/loss flow.

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
- Readable placeholder colors/materials added.
- Visible enemy path added.
- Scene can be regenerated from `Momo's Defense > Build Prototype Scene`.

### Core Battle Loop

- Basic `GameState` with lives and gold.
- Basic `Health` component.
- Enemy path follower.
- Wave spawner.
- Enemy leaks reduce lives.
- Defeated enemies grant gold.
- HUD shows lives, gold, wave state, and result text.

### Momo

- Momo is implemented as the first playable hero placeholder.
- Momo can move by click/tap-style input.
- Momo auto-attacks nearby enemies.
- Momo has first active skill: `Momo Pop`.
- Momo Pop damages nearby enemies.
- Momo Pop slows nearby enemies.
- Momo Pop can be activated through HUD button or Space in editor.
- HUD shows Momo Pop cooldown.

### Towers

- Starter tower prefab created.
- Tower targets and damages nearby enemies.
- Build nodes added to the prototype map.
- Player can click a build node to spend gold and place a starter tower.
- Build nodes mark themselves as occupied after use.
- Clicking build nodes no longer moves Momo.

## In Progress

- Phase 1 first playable loop.
- Basic tower defense interaction feel.
- Prototype-only UI and controls.

## Next Up

1. Add win/loss flow with restart button.
2. Add wave-start control instead of auto-starting immediately.
3. Add tower placement feedback for insufficient gold and occupied nodes.
4. Add one tower upgrade step.
5. Add one second enemy type.
6. Improve mobile aspect ratio framing.

## Remaining Phase 1 Tasks

- Win/loss restart flow.
- Clear victory and defeat states.
- Better tower build feedback.
- At least one tower upgrade.
- At least one additional enemy type.
- Basic tutorial prompts or minimal guidance.
- Manual playtest pass in Unity Editor.
- Optional Android build smoke test.

## Later Scope

### Phase 2: Three-Hero Vertical Slice

- Three controllable heroes.
- Hero portraits/selection.
- One active skill per hero.
- Three tower families.
- Four enemy types.
- In-battle hero leveling.
- Tower upgrade tiers.
- First pass level art.
- Placeholder music and SFX.

### Phase 3: RPG Progression Foundation

- Persistent hero levels.
- Hero skill upgrades.
- Tower family upgrades.
- Equipment definitions.
- Equipment inventory.
- Save/load.
- Reward screen.
- Upgrade menus.

## Testing Standard

Before each committed checkpoint:

- Unity must compile.
- Prototype scene generation must run if the scene builder was touched.
- Current Unity logs must be checked for compile errors and blocking runtime errors.
- Blocking errors must be fixed before moving to the next feature.

## Recent Stable Checkpoints

- `a85956e` - Set up Unity project foundation.
- `dd62f0c` - Add prototype HUD and readability pass.
- `7aa2514` - Add Momo Pop prototype skill.
- `fd646a0` - Add prototype tower build nodes.

