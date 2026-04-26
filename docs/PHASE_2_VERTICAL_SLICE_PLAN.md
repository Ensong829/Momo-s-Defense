# Phase 2: Three-Hero Vertical Slice Plan

## Goal

Prove the signature identity of Momo's Defense: a classic tower defense battle where the player controls three heroes at once.

Phase 2 now has a three-hero prototype in code and generated scene content. A manual playtest is still needed before calling the slice stable.

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
- [ ] Confirm Momo remains fully functional in a manual Unity Editor playtest.

### Slice 2: Three Placeholder Heroes

- [x] Add two placeholder hero prefabs.
- [x] Place all three heroes in the prototype scene.
- [x] Add portraits for all three.
- [ ] Confirm selection and movement work for each in a manual playtest.

### Slice 3: Hero Skills

- [x] Keep Momo Pop.
- [x] Add tank ground slam.
- [x] Add support tower buff.
- [x] Add selected-hero cooldown/skill button UI.

### Slice 4: Hero Combat Tuning

- [x] Give each hero different attack range, damage, and role feel.
- [x] Add visual color coding.
- [ ] Make enemies create at least one reason to reposition heroes.

### Slice 5: Phase 2 Level Pass

- Adjust path/build nodes for three-hero play.
- Add enemy mix that tests all three roles.
- Add basic placeholder effects for skills.

## Risks

- UI can become crowded on mobile.
- Three heroes may make tap targeting confusing.
- Hero control can compete with tower building input.
- Skills need clear targeting rules.

## Testing Requirements

- Player can select each hero reliably.
- Move commands affect only the selected hero.
- Tower building still works.
- Momo Pop still works after the selection refactor.
- No Console errors during a full match.
- HUD remains readable at 16:9 landscape.
