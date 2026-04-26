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

- [ ] Split prototype progression controls into a cleaner upgrade screen.
- [ ] Add clear locked, affordable, unaffordable, and max-rank states.
- [ ] Add reset/debug controls for local prototype testing.
- [ ] Make the reward screen distinct from victory text.

## Slice 4: Hero Progression Expansion

- [ ] Add persistent hero level data for Momo, Bulwark, and Sprout.
- [ ] Add one passive upgrade path per hero.
- [ ] Add skill rank upgrades for Bulwark and Sprout.
- [ ] Move hero progression values into data assets.

## Slice 5: Tower Progression

- [ ] Add persistent tower family ranks for Star, Burst, and Frost.
- [ ] Make tower ranks affect in-battle tower stats.
- [ ] Add one specialization choice per tower family.
- [ ] Move tower progression values into data assets.

## Slice 6: Equipment Foundation

- [ ] Add equipment definition data.
- [ ] Add basic inventory save data.
- [ ] Add weapon, charm, and relic slots.
- [ ] Apply simple flat stat bonuses in battle.
- [ ] Add one special modifier prototype.

## Slice 7: Content Data Pipeline

- [ ] Create ScriptableObject definitions for heroes.
- [ ] Create ScriptableObject definitions for skills.
- [ ] Create ScriptableObject definitions for towers.
- [ ] Create ScriptableObject definitions for enemies.
- [ ] Create ScriptableObject definitions for equipment.
- [ ] Create ScriptableObject definitions for upgrades.

## Slice 8: Validation

- [ ] Confirm rewards are granted once per completed battle.
- [ ] Confirm upgrades persist after restart and app relaunch.
- [ ] Confirm upgraded stats affect future battles.
- [ ] Confirm no blocking Console errors.
- [ ] Confirm Android smoke build succeeds.
