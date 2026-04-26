# Phase 4: Content Pipeline Tasks

## Goal

Make new heroes, enemies, towers, waves, upgrades, and equipment easier to add without rewriting prototype systems.

## Slice 1: Content Assets

- [x] Generate prototype hero assets.
- [x] Generate prototype skill assets.
- [x] Generate prototype tower assets.
- [x] Generate prototype enemy assets.
- [x] Generate prototype equipment assets.
- [x] Generate prototype upgrade assets.

## Slice 2: Level And Wave Authoring

- [x] Add wave definition data.
- [x] Add level definition data.
- [x] Make `WaveSpawner` consume level/wave data.
- [x] Add enemy prefab catalog lookup.
- [x] Keep legacy fallback spawning for safety.

## Slice 3: Balance Data

- [x] Add a prototype balance CSV.
- [x] Record current hero, enemy, tower, and upgrade values.
- [x] Keep generated ScriptableObject assets as the editable Unity-side source.

## Slice 4: Debug Tools

- [x] Add editor command to grant prototype crystals.
- [x] Add editor command to max progression.
- [x] Add editor command to reset progression.
- [x] Keep scene generation available through the existing menu.

## Slice 5: Validation

- [x] Confirm prototype scene generation succeeds.
- [x] Confirm generated scene references `PrototypeLevel01`.
- [x] Confirm generated content assets exist under `Assets/_MomosDefense/Data/Prototype`.
- [x] Confirm no blocking Console errors.
- [x] Confirm Android smoke build succeeds.
