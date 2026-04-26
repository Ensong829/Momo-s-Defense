# Phase 2: Three-Hero Vertical Slice Plan

## Goal

Prove the signature identity of Momo's Defense: a classic tower defense battle where the player controls three heroes at once.

Phase 2 is complete for the prototype milestone. The generated scene now supports the full three-hero vertical slice, with remaining work moving into Phase 3 progression systems and later demo polish.

## Target Experience

- Player enters a polished prototype level.
- Player controls Momo plus two additional placeholder heroes.
- Each hero has a role, portrait, selection state, movement, auto-attack, and one active skill.
- Player builds and upgrades towers while managing all three heroes.
- Enemy waves create moments where hero positioning matters.

## Hero Roster For Vertical Slice

### Hero 001: Momo

Role: flexible control starter.

Current prototype:

- Move command.
- Auto-attack.
- Momo Pop: area damage and slow.

Phase 2 improvements:

- Selection portrait.
- Better visual selection ring.
- Clearer skill feedback.
- Starter upgrade/talent direction.

### Hero 002: Bulwark Placeholder

Role: tank/control.

Prototype concept:

- Higher health feel.
- Short-range attack.
- Active skill: ground slam that stuns or heavily slows enemies in a small area.

Design purpose:

- Teaches blocking/control positioning.
- Gives the player a defensive hero distinct from Momo.

### Hero 003: Sprout Placeholder

Role: support.

Prototype concept:

- Lower damage.
- Active skill: healing pulse or tower buff zone.

Design purpose:

- Teaches support timing.
- Creates a reason to group or reposition heroes/towers.

## Implementation Slices

### Slice 1: Hero Selection Foundation

- [x] Refactor current Momo-only control into a selectable hero control pattern.
- [x] Add selected/unselected visual state.
- [x] Add UI portrait button for Momo.
- [x] Only selected hero receives move commands.
- [x] Keep Momo fully functional after the refactor in automated scene generation.
- [x] Confirm Momo remains functional through automated scene generation and log validation.

### Slice 2: Three Placeholder Heroes

- [x] Add two placeholder hero prefabs.
- [x] Place all three heroes in the prototype scene.
- [x] Add portraits for all three.
- [x] Confirm selection and movement wiring is present in the generated scene and ready for hands-on feel testing.

### Slice 3: Hero Skills

- [x] Keep Momo Pop.
- [x] Add tank ground slam.
- [x] Add support tower buff.
- [x] Add selected-hero cooldown/skill button UI.

### Slice 4: Hero Combat Tuning

- [x] Give each hero different attack range, damage, and role feel.
- [x] Add visual color coding.
- [x] Make enemies create at least one reason to reposition heroes.

### Slice 5: Phase 2 Level Pass

- [x] Adjust path/build nodes for three-hero play.
- [x] Add enemy mix that tests all three roles.
- [x] Add basic placeholder effects and audio feedback for skills.

## Risks

- UI can become crowded on mobile.
- Three heroes may make tap targeting confusing.
- Hero control can compete with tower building input.
- Skills need clear targeting rules.

## Testing Requirements

- Player can select each hero reliably.
- Player can select heroes by portrait and by clicking them in the world.
- Move commands affect only the selected hero.
- Tower building still works.
- Tower upgrades still work after the three-hero refactor.
- Momo Pop still works after the selection refactor.
- No Console errors during a full match.
- HUD remains readable at 16:9 landscape.

## Final Validation

- Date: 2026-04-26.
- Unity scene generation succeeded through `PrototypeSceneBuilder.BuildPrototypeScene`.
- Log scan found no compile errors, Unity exceptions, null refs, missing refs, fatal errors, or build failures.
- The final Phase 2 scene is tuned to four waves with eight enemies per wave and a denser enemy cadence.
- The objective prompt was moved above the bottom control row to keep hero, tower, and wave controls readable in landscape.
- Manual playtesting is still recommended for feel and balance, but Phase 2 is complete enough to begin Phase 3.
