# Momo's Defense

Momo's Defense is a cute stylized 3D mobile tower defense game for Android, built in Unity.

The long-term goal is a commercial tower defense game with RPG-style progression across heroes, towers, and equipment. The current milestone is turning the prototype foundation into a small playable demo with cleaner flow, production-facing shell screens, and data-driven content/runtime cleanup.

## Current Focus

Phase 5: demo shell and content flow around the existing prototype.

Current work:

- Clean up prototype-only runtime wiring so level/content flow is driven by data where possible.
- Add the Phase 5 shell around the prototype: main menu, level select, and clearer player progression through content.
- Keep manual Unity playtest and balance work visible as follow-up, especially for wave pacing, tower value, and hero feel.

## Project Layout

- `Assets/_MomosDefense/Scenes`: Unity scenes.
- `Assets/_MomosDefense/Scripts`: game code.
- `Assets/_MomosDefense/Data`: ScriptableObject content definitions.
- `Assets/_MomosDefense/Prefabs`: reusable Unity prefabs.
- `Assets/_MomosDefense/Art`: visual assets.
- `Assets/_MomosDefense/Audio`: music and sound effects.
- `Assets/_MomosDefense/UI`: UI assets.
- `Assets/_MomosDefense/Prototype`: temporary prototype-only assets.
- `docs`: planning, roadmap, and production notes.

## Engine

Unity editor version: `6000.4.4f1`.

Android is the target platform. The project is already set up for Android-first prototype and smoke-build work in Unity `6000.4.4f1`.
