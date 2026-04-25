# Game Design Blueprint v0.1

## Working Title

Momo's Defense

## Main Character

Momo is the main character, mascot, and first playable hero.

Momo should be introduced before any other hero and should define the player's first impression of the game. The first prototype and vertical slice should build Momo before the wider hero roster.

Early character direction:

- Cute stylized fantasy hero.
- Approachable and expressive.
- Easy to recognize at small mobile size.
- Strong silhouette that works from the classic tower defense camera.
- Mechanically simple enough for a new player to learn, but useful enough to stay relevant later.

Initial gameplay role:

- Flexible starter hero.
- Can block or slow enemies near the path.
- Has one clear active skill for the prototype.
- Can later branch toward damage, support, or control through upgrades/equipment.

Design goal:

- Momo should feel like the player's companion, not just a unit.
- Upgrades should make Momo feel more personal over time.
- Momo's abilities should teach the player how hero control works.

## Target

- Platform: Android first
- Engine: Unity 6 LTS
- Camera: Classic fixed/mostly fixed tower defense camera
- Visual style: Cute stylized fantasy
- Business model: Decide later between premium and cosmetics-first
- Production target: Long-term commercial game, built through a small demo and vertical slice first

## Design Pillars

1. Tactical hero control
   - The player controls 3 heroes at the same time.
   - Heroes are not passive decorations. They reposition, block, cast skills, and carry a major part of the player's strategy.

2. RPG-style progression
   - Heroes, towers, and equipment all have meaningful upgrades.
   - Upgrades should create different builds instead of only increasing numbers.

3. Readable mobile battles
   - The battlefield must stay clear on a phone screen.
   - Enemies, towers, hero states, skill cooldowns, and danger moments should be easy to understand quickly.

4. Cute but tactical
   - The world should feel charming and approachable, but the combat choices should still have depth.

5. Expandable content pipeline
   - New heroes, towers, enemies, items, and levels should be added through data-driven systems where possible.

## Core Loop

1. Choose level.
2. Choose 3 heroes and equipment loadouts.
3. Enter battle.
4. Build and upgrade towers.
5. Move heroes, use skills, and respond to enemy waves.
6. Earn rewards.
7. Upgrade heroes, towers, and equipment.
8. Unlock harder levels and new build options.

## Battle Systems

### Level Structure

- Fixed enemy paths.
- Tower build nodes.
- Hero movement zones or walkable lanes.
- Enemy waves with previews.
- Lives/base health.
- Gold or battle currency earned by defeating enemies.
- Optional side objectives later.

### Towers

Initial tower families:

- Archer: Fast single-target physical damage.
- Mage: Slower magical damage, useful against armored enemies.
- Barracks/Guard: Summons blockers or creates defensive presence.
- Artillery: Area damage, slower reload.

Long-term direction:

- Each tower family can branch into 2 specializations.
- Towers should have active identity, not only damage scaling.

### Heroes

Each battle uses 3 heroes.

Momo is Hero 001 and should be the first hero implemented.

Hero roles:

- Tank/control hero: Blocks enemies, taunts, stuns, protects.
- Damage hero: Burst damage, assassinations, area attacks.
- Support hero: Healing, buffs, slows, shields, resource support.

Hero controls:

- Tap hero portrait or model to select.
- Tap valid ground to move.
- Tap skill button to activate.
- Skills should support mobile-first targeting.

### Enemies

Enemy traits should force tower and hero choices.

Initial enemy types:

- Basic runner.
- Tough armored enemy.
- Fast weak enemy.
- Ranged or disruptive enemy.
- Flying enemy later.
- Boss enemy later.

## Upgrade Systems

The upgrade system is a major feature, not a side menu.

### Hero Progression

Persistent upgrades:

- Hero level.
- Skill unlocks.
- Skill ranks.
- Passive talents.
- Equipment slots.
- Star/rank ascension if we choose a deeper RPG model.

In-battle upgrades:

- Heroes gain temporary battle levels.
- Level-ups improve stats or unlock temporary choices.
- Active skills can become stronger during the match.

Design goal:

- A level 1 hero should feel useful.
- A developed hero should feel meaningfully different, not merely inflated.

### Tower Progression

Persistent upgrades:

- Global tower family upgrades.
- Branch unlocks.
- Special modifiers.

In-battle upgrades:

- Spend gold to improve placed towers.
- Upgrade tiers should change visuals and behavior.
- Final tier can force specialization choices.

Design goal:

- Tower upgrades provide strategic identity.
- Persistent tower upgrades should not make early levels trivial without limits.

### Equipment Progression

Equipment slots:

- Weapon
- Armor or charm
- Relic/accessory

Equipment effects:

- Stat bonuses.
- Skill modifiers.
- Battle-start bonuses.
- Tower synergy bonuses.
- Hero role specialization.

Design constraints:

- Avoid too much inventory complexity early.
- Start with simple equipment, then expand once hero/tower combat is fun.

### Upgrade Economy

Potential currencies:

- Soft currency from battles.
- Upgrade materials from level rewards.
- Hero shards or tokens only if we decide the game needs that style.

Early recommendation:

- Use one simple soft currency and one material type during the prototype.
- Add more currencies only when they solve a design problem.

## Monetization Direction

Most compatible options:

1. Premium
   - Cleaner design.
   - Easier trust with players.
   - Harder to market, but simpler and less risky for a first commercial game.

2. Premium plus cosmetics
   - Best long-term fit if the game grows.
   - Cosmetic skins for heroes, towers, effects, and base themes.
   - Avoids pay-to-win pressure.

3. Free with cosmetics
   - Larger audience potential.
   - Requires stronger live-ops, analytics, and content cadence.

Current recommendation:

- Design the game as premium-first.
- Keep cosmetics technically possible.
- Do not design progression around ads, energy, or pay-to-win upgrades.

## Art Direction

Style:

- Cute stylized fantasy.
- Bright readable silhouettes.
- Toy-like proportions.
- Clear enemy and tower shapes.
- Expressive heroes.

Asset strategy:

- Prototype with placeholder assets.
- Use asset packs carefully for the first vertical slice.
- Gradually replace with custom or modified assets.
- Keep a consistent style guide before final content production.

## Audio Direction

Prototype:

- Licensed placeholder SFX.
- Simple looping battle music.

Final:

- Cheerful fantasy exploration/menu tracks.
- Tense but playful battle tracks.
- Strong skill and tower feedback sounds.
- Short hero voice barks if budget/time allows.

## Technical Direction

Engine:

- Unity 6 LTS.

Architecture goals:

- Data-driven heroes, towers, enemies, waves, and equipment.
- ScriptableObjects for definitions.
- Runtime systems separated from content data.
- Mobile performance from the beginning.

Initial Unity systems:

- Wave manager.
- Enemy path follower.
- Tower targeting and attack system.
- Hero controller.
- Ability system.
- Upgrade/economy system.
- Save system.
- Mobile UI.

## First Playable Prototype

The first playable should include:

- 1 test level.
- 1 enemy path.
- 1 tower type.
- 1 hero.
- 3-5 waves.
- Basic gold economy.
- Basic win/loss.
- Basic Android aspect-ratio UI.

The prototype is successful when:

- A full match can be played from start to finish.
- It is understandable without explanation.
- Hero movement and tower building already feel promising.
