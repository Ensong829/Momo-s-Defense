# Phase 3: RPG Progression Tasks

## Goal

Add the first persistent progression layer so battles can grant rewards, rewards can buy upgrades, and upgrades affect future battles.

## Slice 1: Save And Reward Loop

- [x] Add persistent save/load foundation.
- [x] Add one soft currency.
- [x] Grant currency after victory.
- [x] Show earned reward in the prototype HUD.
- [x] Persist currency between sessions.

## Slice 2: First Hero Upgrade

- [x] Add persistent Momo Pop skill rank.
- [x] Add a prototype upgrade button.
- [x] Spend currency to upgrade Momo Pop.
- [x] Persist Momo Pop rank between sessions.
- [x] Make Momo Pop rank visibly affect battle through stronger damage, radius, cooldown, and rank text.

## Slice 3: Upgrade Menu Foundation

- [x] Split prototype progression controls into a cleaner upgrade screen.
- [x] Add clear affordable, unaffordable, and max-rank states.
- [x] Add reset/debug controls for local prototype testing.
- [x] Make the reward screen distinct from victory text.

## Slice 4: Hero Progression Expansion

- [x] Add persistent hero level data for Momo, Bulwark, and Sprout.
- [x] Add one passive upgrade path per hero.
- [x] Add skill rank upgrades for Bulwark and Sprout.
- [x] Add hero and skill data asset definitions.

## Slice 5: Tower Progression

- [x] Add persistent tower family ranks for Star, Burst, and Frost.
- [x] Make tower ranks affect in-battle tower stats.
- [x] Add one specialization choice per tower family.
- [x] Add tower data asset definitions.

## Slice 6: Equipment Foundation

- [x] Add equipment definition data.
- [x] Add basic equipment loadout save data.
- [x] Add weapon, charm, and relic slots.
- [x] Apply simple flat stat bonuses in battle.
- [x] Add one special modifier field prototype.

## Slice 7: Content Data Pipeline

- [x] Create ScriptableObject definitions for heroes.
- [x] Create ScriptableObject definitions for skills.
- [x] Create ScriptableObject definitions for towers.
- [x] Create ScriptableObject definitions for enemies.
- [x] Create ScriptableObject definitions for equipment.
- [x] Create ScriptableObject definitions for upgrades.

## Slice 8: Validation

- [x] Confirm reward grant path is guarded to run once per completed battle.
- [x] Confirm upgrades use persistent `PlayerPrefs` save/load.
- [x] Confirm upgraded stats affect future battles.
- [x] Confirm no blocking Console errors in automated scene generation and build logs.
- [x] Confirm Android smoke build succeeds.
