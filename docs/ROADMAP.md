# Production Roadmap v0.1

Status dashboard: see `docs/PROJECT_STATUS.md`.

## Recommended Scope Strategy

The long-term goal is a commercial Android game, but the working path should be:

1. Prototype
2. Vertical slice
3. Small public demo
4. Expanded commercial production

This keeps the dream large while making the next step small enough to actually finish.

## Phase 0: Setup and Planning

Status: mostly complete.

Goal: Prepare the project so development does not become chaotic later.

Deliverables:

- [x] Unity project created with Android target.
- [x] Version control configured.
- [x] Folder structure created.
- [x] Basic coding conventions.
- [x] Living design docs.
- [x] Placeholder asset policy.

Done when:

- The project opens cleanly.
- A blank scene builds for Android or Android-compatible settings.
- The team can add systems without guessing where files belong.

## Phase 1: First Playable Prototype

Status: implemented, with manual playtest/balance follow-up still pending.

Goal: Prove the basic tower defense loop.

Deliverables:

- [x] One graybox map.
- [x] One enemy path.
- [x] Enemy spawner and wave manager.
- [x] One enemy type.
- [x] One tower type.
- [x] Momo as the first controllable hero.
- [x] Basic player lives.
- [x] Basic gold.
- [x] Win/loss condition.
- [x] Minimal mobile HUD.
- [x] Tower build nodes.
- [x] Tower placement feedback.
- [x] Momo's first active skill.
- [x] Wave-start control.
- [x] Tower upgrade step.
- [x] Second enemy type.
- [x] Restart flow.
- [x] Mobile landscape framing.
- [x] Basic guidance prompt.
- [x] Android smoke build.
- [x] Playtest checklist.
- [ ] Manual playtest and balance pass.

Done when:

- A player can finish a 2-3 minute match.
- Momo can move, interact with enemies, and use one prototype skill.
- Bugs blocking the core loop are fixed.
- The prototype runs in a mobile-shaped resolution.

## Phase 2: Three-Hero Vertical Slice

Status: complete for prototype milestone.

Goal: Prove the special identity of the game.

Deliverables:

- Three controllable heroes.
- Hero selection portraits. Prototype implemented.
- Hero movement and targeting.
- One active skill per hero. Prototype implemented.
- Three tower families. Prototype implemented.
- Four enemy types. Prototype implemented.
- In-battle hero leveling. Prototype implemented.
- Tower upgrade tiers. Prototype implemented.
- First pass level art.
- Placeholder music and SFX. Prototype implemented.

Done when:

- The game is fun enough to replay the same level several times. Prototype target met, with a longer four-wave pass ready for hands-on feel tuning.
- The three heroes create real tactical choices. Prototype target met with Momo, Bulwark, and Sprout roles.
- The UI remains readable on a phone screen. Prototype target met for 16:9 landscape after the bottom guidance prompt was moved above the control row.

## Phase 3: RPG Progression Foundation

Status: complete for prototype milestone.

Goal: Add the progression systems that make the game sticky.

Deliverables:

- Persistent hero levels.
- Hero skill upgrades. Prototype implemented for Momo, Bulwark, and Sprout.
- Tower family upgrades. Prototype implemented for Star, Burst, and Frost ranks.
- Tower specialization choices. Prototype implemented for Star, Burst, and Frost.
- Equipment definitions. ScriptableObject definition added.
- Equipment inventory. Prototype loadout slots added.
- Save/load. First `PlayerPrefs` progression save implemented.
- Reward screen. Prototype victory reward text implemented.
- Basic upgrade menus. Prototype upgrade panel implemented.

Done when:

- Playing battles grants rewards.
- Rewards can be spent on upgrades.
- Upgrades affect future battles.
- The system is understandable without too many currencies.

## Phase 4: Content Pipeline

Status: complete for prototype milestone.

Goal: Make it easy to add more game content.

Deliverables:

- ScriptableObject content templates. Prototype implemented.
- Level/wave authoring workflow. Prototype implemented with `LevelDefinition` and `WaveDefinition`.
- Enemy stat tables. Prototype assets and CSV implemented.
- Tower stat tables. Prototype assets and CSV implemented.
- Hero stat tables. Prototype assets and CSV implemented.
- Balance spreadsheet or data source. Prototype CSV implemented.
- Debug tools for spawning waves and testing upgrades. Prototype progression debug menu implemented.

Done when:

- A new enemy, tower, wave, or hero can be added without rewriting core systems.
- Balance values can be changed quickly.

## Phase 5: Demo Content

Status: in progress.

Goal: Build a small but polished demo.

Deliverables:

- 3-5 campaign levels.
- 3-5 heroes.
- 4 tower families.
- 8-12 enemies.
- 1 boss.
- Basic tutorial.
- Main menu. In progress.
- Level select. In progress.
- Upgrade menus.
- Settings.
- Performance pass.
- Data-driven runtime cleanup for shell and battle flow. In progress.
- Better content flow between startup, shell UI, battle, and return states. In progress.

Done when:

- The demo feels like a real small game.
- New players can understand it.
- It runs smoothly on a mid-range Android phone.
- Shell screens and runtime flow no longer depend on prototype-only shortcuts.

## Phase 6: Commercial Expansion

Status: not started.

Goal: Expand from demo to release candidate.

Deliverables:

- Full first campaign.
- More heroes.
- More enemy factions.
- More equipment.
- More tower branches.
- Finalized art direction.
- Final audio pass.
- Monetization implementation if chosen.
- App store page assets.
- Trailer.
- Closed beta.

Done when:

- The game has enough content for the chosen price/model.
- Retention and difficulty feel healthy from testing.
- Release-blocking bugs are resolved.

## Working Rules

- Build one playable layer at a time.
- Keep placeholder assets acceptable until gameplay proves itself.
- Test every completed chunk.
- Fix blocking bugs before adding more content.
- Keep design docs updated after important decisions.
- Prefer simple systems first, expandable systems second.
- Avoid permanent monetization decisions until the vertical slice is fun.

## Sub-Agent Workflow

When sub-agents are used:

- Each agent gets a specific responsibility.
- Each agent works on a small, testable task.
- Agents do not overwrite each other's work.
- The main agent reviews and integrates.
- Every implementation task ends with test results and known risks.

Potential roles:

- Gameplay systems agent.
- UI and mobile input agent.
- Content/data agent.
- Asset/audio pipeline agent.
- QA and bug reproduction agent.
