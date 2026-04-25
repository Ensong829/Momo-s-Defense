# Phase 2: Three-Hero Vertical Slice Plan

## Goal

Prove the signature identity of Momo's Defense: a classic tower defense battle where the player controls three heroes at once.

Phase 2 has started with a Momo-only selection foundation. A manual playtest is still needed before adding the second and third heroes.

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

- Add two placeholder hero prefabs.
- Place all three heroes in the prototype scene.
- Add portraits for all three.
- Confirm selection and movement work for each.

### Slice 3: Hero Skills

- Keep Momo Pop.
- Add tank ground slam.
- Add support heal or tower buff.
- Add cooldown text/buttons for each selected hero or per-portrait skill UI.

### Slice 4: Hero Combat Tuning

- Give each hero different attack range, damage, and role feel.
- Add visual color coding.
- Make enemies create at least one reason to reposition heroes.

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
