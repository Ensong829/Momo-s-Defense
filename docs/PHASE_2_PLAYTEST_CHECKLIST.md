# Phase 2 Playtest Checklist

Use this checklist in Unity Editor with `Assets/_MomosDefense/Scenes/Prototype_MomoDefense.unity`.

## Setup

- Open the prototype scene.
- Press Play.
- Confirm the scene starts with no Console errors.
- Confirm enemies do not spawn until `Start Wave` is pressed.
- Set Game view to a 16:9 landscape aspect and confirm the bottom control row is readable.

## Three-Hero Controls

- Select Momo, Bulwark, and Sprout with portrait buttons.
- Select Momo, Bulwark, and Sprout with `1`, `2`, and `3`.
- Select each hero by clicking the hero in the world.
- Move each hero and confirm only the selected hero moves.
- Click UI buttons and confirm heroes do not receive unwanted move commands.

## Hero Combat And Skills

- Confirm each hero auto-attacks nearby enemies.
- Use Momo Pop near a clustered wave and confirm damage, slow, SFX, and cooldown.
- Use Ground Slam near runners or tough enemies and confirm strong slow/control.
- Use Bloom Song near towers and confirm boosted tower color and faster damage output.
- Confirm hero portrait levels increase from shared enemy XP.

## Towers And Economy

- Select Star, Burst, and Frost tower families.
- Build each tower family on different build nodes.
- Confirm each build spends the correct gold and shows HUD feedback.
- Click occupied build nodes and built towers to upgrade.
- Confirm max-upgraded and insufficient-gold feedback is readable.
- Confirm Frost slows enemies and Burst feels meaningfully different from Star.

## Waves And Battle Flow

- Start all four waves.
- Confirm runners appear from wave 2 onward.
- Confirm tough enemies appear from wave 2 onward.
- Confirm armored enemies appear from wave 3 onward.
- Confirm waves create a reason to reposition heroes.
- Confirm victory appears after wave 4 is cleared.
- Let enough enemies leak in a separate run and confirm defeat and restart.

## Phase 2 Exit Decision

Phase 2 can be marked complete when:

- The battle can be played start to finish with all three heroes.
- The player can build, select, upgrade, and use every tower family.
- All three hero skills are useful and readable.
- The HUD remains readable in 16:9 landscape.
- No blocking Console errors appear during a full run.
