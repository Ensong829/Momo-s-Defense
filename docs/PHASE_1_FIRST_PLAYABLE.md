# Phase 1: First Playable

## Goal

Create a rough but playable tower defense match starring Momo.

## Current Prototype Contents

- Scene: `Assets/_MomosDefense/Scenes/Prototype_MomoDefense.unity`
- Momo placeholder hero.
- One enemy path.
- One prototype enemy prefab.
- One starter tower.
- Wave spawner.
- Basic lives and gold state.
- Enemy rewards when defeated.
- Base life loss when enemies reach the end.

## Next Implementation Steps

1. Add visible HUD for lives, gold, wave state, and selected hero.
2. Add Momo's first active skill.
3. Add tower build nodes instead of a pre-placed tower only.
4. Add simple win/loss screen.
5. Add a second enemy type.
6. Add one tower upgrade step.

## Momo Prototype Ability Candidates

Candidate A: Momo Pop

- Short range area burst around Momo.
- Damages and briefly slows nearby enemies.
- Easy to understand and useful for teaching hero positioning.

Candidate B: Brave Bonk

- Momo strikes one tough enemy for high damage and a short stun.
- Good for teaching priority targeting.

Candidate C: Guard Puff

- Momo creates a small protective zone that slows enemies passing through.
- More strategic, but slightly harder for a first tutorial.

Current recommendation:

- Start with Momo Pop because it teaches movement, timing, and crowd control quickly.

## Test Checklist

- Enemies spawn and follow the full path.
- Enemies reduce lives when reaching the end.
- Tower damages enemies in range.
- Momo moves when the player clicks or taps the ground.
- Momo damages enemies in range.
- Defeated enemies grant gold.
- Match can run for several waves without console errors.

