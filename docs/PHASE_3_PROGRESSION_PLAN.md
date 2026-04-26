# Phase 3: RPG Progression Plan

## Goal

Build the first persistent progression layer for heroes, towers, and equipment after the three-hero vertical slice proves the combat direction.

Status: complete for the prototype milestone.

## Progression Principles

- Start simple.
- Avoid too many currencies.
- Upgrades should change choices, not only numbers.
- Do not add monetization pressure to progression during prototype development.

## Hero Progression

First pass:

- Hero level.
- Skill rank.
- One passive upgrade path per hero.

Later:

- Talent branches.
- Equipment slot unlocks.
- Hero cosmetic skins.

## Tower Progression

First pass:

- Persistent tower family rank.
- In-battle upgrade tier.
- One specialization choice per tower family.

Later:

- Branching tower upgrades.
- Tower skins.
- Tower synergy bonuses from equipment or heroes.

## Equipment Progression

First pass:

- Simple equipment definitions.
- Weapon, charm, and relic slots.
- Flat stat bonuses and one special modifier.

Later:

- Rarity.
- Set bonuses.
- Crafting or upgrade materials.

## Data Direction

Use ScriptableObjects for:

- Hero definitions.
- Skill definitions.
- Tower definitions.
- Enemy definitions.
- Equipment definitions.
- Upgrade definitions.

## First Progression Prototype

Minimum viable version:

- [x] Completing a battle grants soft currency.
- [x] Currency can upgrade Momo's skill rank.
- [x] Upgrade persists between play sessions.
- [x] Upgrade has a visible effect in battle.

Implementation notes:

- `ProgressionService` stores prototype progression in `PlayerPrefs`.
- Victory grants crystals once per battle.
- Momo Pop rank increases damage and radius while slightly reducing cooldown.
- The prototype HUD shows crystals, victory reward text, selected skill rank text, and an upgrade panel.
- Momo, Bulwark, and Sprout have persistent skill ranks and persistent hero levels.
- Star, Burst, and Frost have persistent tower family ranks.
- Star, Burst, and Frost have first-pass specialization choices.
- A starter Training Charm applies prototype equipment bonuses.
- ScriptableObject definition classes now exist for future content authoring.
- Automated scene generation and Android smoke build both passed for the final Phase 3 slice.

Detailed task list:

- `docs/PHASE_3_TASKS.md`
