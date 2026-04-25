# Phase 3: RPG Progression Plan

## Goal

Build the first persistent progression layer for heroes, towers, and equipment after the three-hero vertical slice proves the combat direction.

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

- Completing a battle grants soft currency.
- Currency can upgrade Momo's skill rank.
- Upgrade persists between play sessions.
- Upgrade has a visible effect in battle.

