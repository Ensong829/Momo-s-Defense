# Phase 1 Playtest Checklist

Use this checklist in Unity Editor with `Assets/_MomosDefense/Scenes/Prototype_MomoDefense.unity`.

## Setup

- Open the prototype scene.
- Press Play.
- Confirm the scene starts with no Console errors.
- Confirm enemies do not spawn until `Start Wave` is pressed.

## Basic Controls

- Click ground and confirm Momo moves.
- Click a build node and confirm a tower is built.
- Click an occupied build node and confirm upgrade feedback appears.
- Press `Start Wave` and confirm enemies spawn.
- Press `Momo Pop` near enemies and confirm cooldown starts.

## Combat Loop

- Confirm basic enemies follow the path.
- Confirm tough enemies appear from wave 2 onward.
- Confirm towers attack enemies.
- Confirm Momo attacks enemies in range.
- Confirm Momo Pop damages and slows enemies.
- Confirm defeated enemies grant gold.
- Confirm enemies reaching the end reduce lives.

## Economy

- Confirm building a tower costs gold.
- Confirm upgrading a tower costs gold.
- Confirm insufficient gold shows feedback.
- Confirm occupied/max-upgraded nodes show feedback.

## End States

- Clear all waves and confirm `Victory` appears.
- Let enough enemies through and confirm `Defeat` appears.
- Confirm `Restart` appears after victory or defeat.
- Click `Restart` and confirm the scene reloads.

## Mobile Framing

- Set Game view to a 16:9 landscape aspect.
- Confirm HUD text does not overlap.
- Confirm build nodes, Momo, path, and wave controls are visible.
- Confirm the objective prompt is readable.

## Balance Notes

Record notes for:

- Are the waves too easy or too hard?
- Does Momo feel useful?
- Does Momo Pop feel satisfying?
- Are tower build costs understandable?
- Are upgrades worth buying?
- Are tough enemies noticeably different?
- Is the screen too cluttered?

## Phase 1 Exit Decision

Phase 1 can be marked complete when:

- The prototype can be played start to finish.
- The player can build, upgrade, start waves, move Momo, use Momo Pop, win, lose, and restart.
- No blocking Console errors appear during a full run.
- The basic loop feels understandable enough to expand.

