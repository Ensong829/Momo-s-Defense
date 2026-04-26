using System.Collections.Generic;
using MomosDefense.Combat;
using MomosDefense.Core;
using MomosDefense.Enemies;
using MomosDefense.Heroes;
using MomosDefense.Towers;
using MomosDefense.UI;
using MomosDefense.Waves;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace MomosDefense.Editor
{
    public static class PrototypeSceneBuilder
    {
        private const string ScenePath = "Assets/_MomosDefense/Scenes/Prototype_MomoDefense.unity";
        private const string EnemyPrefabPath = "Assets/_MomosDefense/Prefabs/Enemies/PrototypeEnemy.prefab";
        private const string ToughEnemyPrefabPath = "Assets/_MomosDefense/Prefabs/Enemies/PrototypeToughEnemy.prefab";
        private const string RunnerEnemyPrefabPath = "Assets/_MomosDefense/Prefabs/Enemies/PrototypeRunnerEnemy.prefab";
        private const string ArmoredEnemyPrefabPath = "Assets/_MomosDefense/Prefabs/Enemies/PrototypeArmoredEnemy.prefab";
        private const string TowerPrefabPath = "Assets/_MomosDefense/Prefabs/Towers/PrototypeStarterTower.prefab";
        private const string BurstTowerPrefabPath = "Assets/_MomosDefense/Prefabs/Towers/PrototypeBurstTower.prefab";
        private const string FrostTowerPrefabPath = "Assets/_MomosDefense/Prefabs/Towers/PrototypeFrostTower.prefab";
        private const string MaterialFolder = "Assets/_MomosDefense/Materials";
        private const string ContentFolder = "Assets/_MomosDefense/Data/Prototype";

        [MenuItem("Momo's Defense/Build Prototype Scene")]
        public static void BuildPrototypeScene()
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            GameObject gameStateObject = new GameObject("Game State");
            GameState gameState = gameStateObject.AddComponent<GameState>();

            EnsureMaterialFolder();
            CreateCamera();
            CreateLight();
            CreateGround();
            EnemyPath path = CreatePath();
            GameObject enemyPrefab = CreateEnemyPrefab();
            GameObject toughEnemyPrefab = CreateToughEnemyPrefab();
            GameObject runnerEnemyPrefab = CreateRunnerEnemyPrefab();
            GameObject armoredEnemyPrefab = CreateArmoredEnemyPrefab();
            LevelDefinition levelDefinition = CreatePrototypeContentAssets();
            GameObject towerPrefab = CreateTowerPrefab(
                "Prototype Starter Tower",
                TowerPrefabPath,
                "Prototype_Tower",
                new Color(0.25f, 0.43f, 0.86f),
                "Star Tower",
                "Star",
                4f,
                1f,
                1,
                80,
                1,
                0.75f,
                0.25f,
                1.15f);
            GameObject burstTowerPrefab = CreateTowerPrefab(
                "Prototype Burst Tower",
                BurstTowerPrefabPath,
                "Prototype_BurstTower",
                new Color(0.92f, 0.42f, 0.32f),
                "Burst Tower",
                "Burst",
                3.2f,
                0.65f,
                3,
                95,
                2,
                0.5f,
                0.15f,
                1.14f);
            GameObject frostTowerPrefab = CreateTowerPrefab(
                "Prototype Frost Tower",
                FrostTowerPrefabPath,
                "Prototype_FrostTower",
                new Color(0.48f, 0.85f, 0.95f),
                "Frost Tower",
                "Frost",
                3.6f,
                1.1f,
                1,
                85,
                1,
                0.65f,
                0.2f,
                1.14f,
                1.5f,
                0.65f);
            PrototypeHeroController momo = CreateHero(
                "Momo",
                "Prototype_Momo",
                new Color(0.95f, 0.58f, 0.74f),
                "Prototype_MomoSelection",
                new Color(1f, 0.94f, 0.35f),
                new Vector3(-5f, 1f, 0.3f),
                new Vector3(0.9f, 1.1f, 0.9f),
                PrototypeHeroController.HeroSkillType.MomoPop,
                "Momo Pop",
                5f,
                2f,
                1f,
                2,
                3f,
                4,
                8f,
                2.5f,
                0.45f);
            PrototypeHeroController bulwark = CreateHero(
                "Bulwark",
                "Prototype_Bulwark",
                new Color(0.92f, 0.72f, 0.38f),
                "Prototype_BulwarkSelection",
                new Color(1f, 0.84f, 0.3f),
                new Vector3(-6.8f, 1f, -1.9f),
                new Vector3(1.15f, 1.3f, 1.15f),
                PrototypeHeroController.HeroSkillType.GroundSlam,
                "Ground Slam",
                4.2f,
                1.65f,
                0.85f,
                3,
                2.4f,
                5,
                10f,
                2.2f,
                0.12f);
            PrototypeHeroController sprout = CreateHero(
                "Sprout",
                "Prototype_Sprout",
                new Color(0.54f, 0.88f, 0.58f),
                "Prototype_SproutSelection",
                new Color(0.72f, 1f, 0.56f),
                new Vector3(-3.1f, 1f, -2.1f),
                new Vector3(0.82f, 1f, 0.82f),
                PrototypeHeroController.HeroSkillType.TowerBloom,
                "Bloom Song",
                5.4f,
                2.4f,
                1.15f,
                1,
                3.75f,
                0,
                12f,
                0f,
                1f,
                5.5f,
                1,
                1.75f);
            ProgressionService progressionService = CreateProgressionService();
            HeroSelectionManager heroSelection = CreateHeroSelectionManager(momo, bulwark, sprout);
            TowerBuildManager buildManager = CreateTowerBuildManager(towerPrefab, burstTowerPrefab, frostTowerPrefab);
            CreateBuildNodes(gameState, progressionService, buildManager);
            WaveSpawner waveSpawner = CreateWaveSpawner(enemyPrefab, toughEnemyPrefab, runnerEnemyPrefab, armoredEnemyPrefab, levelDefinition, path, gameState, heroSelection);
            CreateEventSystem();
            CreateHud(gameState, progressionService, waveSpawner, heroSelection, buildManager, momo, bulwark, sprout);

            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void CreateCamera()
        {
            GameObject cameraObject = new GameObject("Main Camera");
            Camera camera = cameraObject.AddComponent<Camera>();
            cameraObject.tag = "MainCamera";
            cameraObject.transform.position = new Vector3(0f, 14f, -12f);
            cameraObject.transform.rotation = Quaternion.Euler(55f, 0f, 0f);
            camera.orthographic = true;
            camera.orthographicSize = 7.8f;
        }

        private static void CreateLight()
        {
            GameObject lightObject = new GameObject("Directional Light");
            Light light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 0.85f;
            lightObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
        }

        private static void CreateGround()
        {
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Prototype Ground";
            ground.transform.localScale = new Vector3(2f, 1f, 2f);
            AssignMaterial(ground, "Prototype_Ground", new Color(0.42f, 0.72f, 0.38f));
        }

        private static EnemyPath CreatePath()
        {
            GameObject pathObject = new GameObject("Enemy Path");
            Material pathMaterial = GetOrCreateMaterial("Prototype_Path", new Color(0.68f, 0.52f, 0.34f));
            List<Transform> waypoints = new List<Transform>();
            Vector3[] points =
            {
                new Vector3(-8f, 0.1f, 4f),
                new Vector3(-3f, 0.1f, 4f),
                new Vector3(-3f, 0.1f, -2f),
                new Vector3(4f, 0.1f, -2f),
                new Vector3(8f, 0.1f, 2f)
            };

            for (int i = 0; i < points.Length; i++)
            {
                GameObject waypoint = new GameObject($"Waypoint {i + 1}");
                waypoint.transform.SetParent(pathObject.transform);
                waypoint.transform.position = points[i];
                waypoints.Add(waypoint.transform);

                GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
                marker.name = $"Path Marker {i + 1}";
                marker.transform.SetParent(pathObject.transform);
                marker.transform.position = points[i] + Vector3.up * 0.01f;
                marker.transform.localScale = new Vector3(0.7f, 0.03f, 0.7f);
                marker.GetComponent<Renderer>().sharedMaterial = pathMaterial;

                if (i > 0)
                {
                    CreatePathSegment(pathObject.transform, points[i - 1], points[i], pathMaterial, i);
                }
            }

            EnemyPath path = pathObject.AddComponent<EnemyPath>();
            SerializedObject serializedPath = new SerializedObject(path);
            SerializedProperty waypointProperty = serializedPath.FindProperty("waypoints");
            waypointProperty.arraySize = waypoints.Count;

            for (int i = 0; i < waypoints.Count; i++)
            {
                waypointProperty.GetArrayElementAtIndex(i).objectReferenceValue = waypoints[i];
            }

            serializedPath.ApplyModifiedPropertiesWithoutUndo();
            return path;
        }

        private static GameObject CreateEnemyPrefab()
        {
            GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            enemy.name = "Prototype Enemy";
            enemy.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            AssignMaterial(enemy, "Prototype_Enemy", new Color(0.86f, 0.26f, 0.25f));
            enemy.AddComponent<Health>();
            enemy.AddComponent<EnemyPathFollower>();

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(enemy, EnemyPrefabPath);
            Object.DestroyImmediate(enemy);
            return prefab;
        }

        private static GameObject CreateToughEnemyPrefab()
        {
            GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            enemy.name = "Prototype Tough Enemy";
            enemy.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            AssignMaterial(enemy, "Prototype_ToughEnemy", new Color(0.55f, 0.18f, 0.78f));

            Health health = enemy.AddComponent<Health>();
            EnemyPathFollower follower = enemy.AddComponent<EnemyPathFollower>();

            SerializedObject serializedHealth = new SerializedObject(health);
            serializedHealth.FindProperty("maxHealth").intValue = 18;
            serializedHealth.ApplyModifiedPropertiesWithoutUndo();

            SerializedObject serializedFollower = new SerializedObject(follower);
            serializedFollower.FindProperty("moveSpeed").floatValue = 1.25f;
            serializedFollower.FindProperty("goldReward").intValue = 22;
            serializedFollower.FindProperty("experienceReward").intValue = 2;
            serializedFollower.ApplyModifiedPropertiesWithoutUndo();

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(enemy, ToughEnemyPrefabPath);
            Object.DestroyImmediate(enemy);
            return prefab;
        }

        private static GameObject CreateRunnerEnemyPrefab()
        {
            GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            enemy.name = "Prototype Runner Enemy";
            enemy.transform.localScale = new Vector3(0.68f, 0.72f, 0.68f);
            AssignMaterial(enemy, "Prototype_RunnerEnemy", new Color(0.95f, 0.9f, 0.32f));

            Health health = enemy.AddComponent<Health>();
            EnemyPathFollower follower = enemy.AddComponent<EnemyPathFollower>();

            SerializedObject serializedHealth = new SerializedObject(health);
            serializedHealth.FindProperty("maxHealth").intValue = 6;
            serializedHealth.ApplyModifiedPropertiesWithoutUndo();

            SerializedObject serializedFollower = new SerializedObject(follower);
            serializedFollower.FindProperty("moveSpeed").floatValue = 3.3f;
            serializedFollower.FindProperty("goldReward").intValue = 8;
            serializedFollower.FindProperty("experienceReward").intValue = 1;
            serializedFollower.ApplyModifiedPropertiesWithoutUndo();

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(enemy, RunnerEnemyPrefabPath);
            Object.DestroyImmediate(enemy);
            return prefab;
        }

        private static GameObject CreateArmoredEnemyPrefab()
        {
            GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            enemy.name = "Prototype Armored Enemy";
            enemy.transform.localScale = new Vector3(1.25f, 1.35f, 1.25f);
            AssignMaterial(enemy, "Prototype_ArmoredEnemy", new Color(0.4f, 0.45f, 0.52f));

            Health health = enemy.AddComponent<Health>();
            EnemyPathFollower follower = enemy.AddComponent<EnemyPathFollower>();

            SerializedObject serializedHealth = new SerializedObject(health);
            serializedHealth.FindProperty("maxHealth").intValue = 28;
            serializedHealth.ApplyModifiedPropertiesWithoutUndo();

            SerializedObject serializedFollower = new SerializedObject(follower);
            serializedFollower.FindProperty("moveSpeed").floatValue = 0.95f;
            serializedFollower.FindProperty("goldReward").intValue = 30;
            serializedFollower.FindProperty("experienceReward").intValue = 3;
            serializedFollower.ApplyModifiedPropertiesWithoutUndo();

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(enemy, ArmoredEnemyPrefabPath);
            Object.DestroyImmediate(enemy);
            return prefab;
        }

        private static LevelDefinition CreatePrototypeContentAssets()
        {
            EnsureContentFolder();

            CreateHeroDefinition("Momo", "Momo", "Flexible control", 1);
            CreateHeroDefinition("Bulwark", "Bulwark", "Tank control", 1);
            CreateHeroDefinition("Sprout", "Sprout", "Support", 1);
            CreateSkillDefinition("MomoPop", "Momo Pop", "Momo", 4, 3f, 8f);
            CreateSkillDefinition("GroundSlam", "Ground Slam", "Bulwark", 5, 2.4f, 10f);
            CreateSkillDefinition("BloomSong", "Bloom Song", "Sprout", 0, 3.75f, 12f);
            CreateTowerDefinition("Star", "Star Tower", 60, 1, 4f, 1f);
            CreateTowerDefinition("Burst", "Burst Tower", 75, 3, 3.2f, 0.65f);
            CreateTowerDefinition("Frost", "Frost Tower", 70, 1, 3.6f, 1.1f);
            CreateEnemyDefinition("Basic", "Basic Enemy", 10, 2f, 10, 1);
            CreateEnemyDefinition("Tough", "Tough Enemy", 18, 1.25f, 22, 2);
            CreateEnemyDefinition("Runner", "Runner Enemy", 6, 3.3f, 8, 1);
            CreateEnemyDefinition("Armored", "Armored Enemy", 28, 0.95f, 30, 3);
            CreateEquipmentDefinition("TrainingCharm", "Training Charm", EquipmentDefinition.EquipmentSlot.Charm, 1, 0.05f, "Starter");
            CreateUpgradeDefinition("MomoSkillRank", "Momo Pop Rank", "Momo", 5, 50, 35);
            CreateUpgradeDefinition("TowerFamilyRank", "Tower Family Rank", "Tower", 5, 45, 30);

            WaveDefinition wave1 = CreateWaveDefinition("Wave01", new[] { ("Basic", 8) });
            WaveDefinition wave2 = CreateWaveDefinition("Wave02", new[] { ("Basic", 4), ("Runner", 2), ("Tough", 2) });
            WaveDefinition wave3 = CreateWaveDefinition("Wave03", new[] { ("Basic", 3), ("Runner", 2), ("Tough", 2), ("Armored", 1) });
            WaveDefinition wave4 = CreateWaveDefinition("Wave04", new[] { ("Runner", 3), ("Tough", 3), ("Armored", 2) });

            LevelDefinition level = LoadOrCreateAsset<LevelDefinition>($"{ContentFolder}/PrototypeLevel01.asset");
            SerializedObject serializedLevel = new SerializedObject(level);
            serializedLevel.FindProperty("levelId").stringValue = "PrototypeLevel01";
            serializedLevel.FindProperty("displayName").stringValue = "Prototype Crossing";
            serializedLevel.FindProperty("timeBetweenEnemies").floatValue = 0.7f;
            serializedLevel.FindProperty("timeBetweenWaves").floatValue = 3f;
            serializedLevel.FindProperty("victoryCurrencyReward").intValue = 35;
            SerializedProperty wavesProperty = serializedLevel.FindProperty("waves");
            wavesProperty.arraySize = 4;
            wavesProperty.GetArrayElementAtIndex(0).objectReferenceValue = wave1;
            wavesProperty.GetArrayElementAtIndex(1).objectReferenceValue = wave2;
            wavesProperty.GetArrayElementAtIndex(2).objectReferenceValue = wave3;
            wavesProperty.GetArrayElementAtIndex(3).objectReferenceValue = wave4;
            serializedLevel.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(level);

            AssetDatabase.SaveAssets();
            return level;
        }

        private static PrototypeHeroController CreateHero(
            string heroName,
            string materialName,
            Color bodyColor,
            string selectionMaterialName,
            Color selectionColor,
            Vector3 position,
            Vector3 scale,
            PrototypeHeroController.HeroSkillType skillType,
            string skillName,
            float moveSpeed,
            float attackRange,
            float attacksPerSecond,
            int attackDamage,
            float skillRadius,
            int skillDamage,
            float skillCooldown,
            float slowDuration,
            float slowMultiplier,
            float towerBuffDuration = 5f,
            int towerBuffDamageBonus = 1,
            float towerBuffAttackSpeedMultiplier = 1.6f)
        {
            GameObject hero = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            hero.name = $"{heroName} Prototype Hero";
            hero.transform.position = position;
            hero.transform.localScale = scale;
            AssignMaterial(hero, materialName, bodyColor);

            GameObject selectionRing = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            selectionRing.name = "Selection Ring";
            selectionRing.transform.SetParent(hero.transform);
            selectionRing.transform.localPosition = new Vector3(0f, -0.92f, 0f);
            selectionRing.transform.localScale = new Vector3(1.55f, 0.025f, 1.55f);
            AssignMaterial(selectionRing, selectionMaterialName, selectionColor);
            Object.DestroyImmediate(selectionRing.GetComponent<Collider>());

            PrototypeHeroController controller = hero.AddComponent<PrototypeHeroController>();
            SerializedObject serializedHero = new SerializedObject(controller);
            serializedHero.FindProperty("heroName").stringValue = heroName;
            serializedHero.FindProperty("skillName").stringValue = skillName;
            serializedHero.FindProperty("skillType").enumValueIndex = (int)skillType;
            serializedHero.FindProperty("selectionIndicator").objectReferenceValue = selectionRing;
            serializedHero.FindProperty("moveSpeed").floatValue = moveSpeed;
            serializedHero.FindProperty("attackRange").floatValue = attackRange;
            serializedHero.FindProperty("attacksPerSecond").floatValue = attacksPerSecond;
            serializedHero.FindProperty("attackDamage").intValue = attackDamage;
            serializedHero.FindProperty("skillRadius").floatValue = skillRadius;
            serializedHero.FindProperty("skillDamage").intValue = skillDamage;
            serializedHero.FindProperty("skillCooldown").floatValue = skillCooldown;
            serializedHero.FindProperty("slowDuration").floatValue = slowDuration;
            serializedHero.FindProperty("slowMultiplier").floatValue = slowMultiplier;
            serializedHero.FindProperty("towerBuffDuration").floatValue = towerBuffDuration;
            serializedHero.FindProperty("towerBuffDamageBonus").intValue = towerBuffDamageBonus;
            serializedHero.FindProperty("towerBuffAttackSpeedMultiplier").floatValue = towerBuffAttackSpeedMultiplier;
            serializedHero.ApplyModifiedPropertiesWithoutUndo();

            return controller;
        }

        private static HeroSelectionManager CreateHeroSelectionManager(
            PrototypeHeroController momo,
            PrototypeHeroController bulwark,
            PrototypeHeroController sprout)
        {
            GameObject selectionObject = new GameObject("Hero Selection Manager");
            HeroSelectionManager selectionManager = selectionObject.AddComponent<HeroSelectionManager>();

            SerializedObject serializedSelection = new SerializedObject(selectionManager);
            SerializedProperty heroesProperty = serializedSelection.FindProperty("heroes");
            heroesProperty.arraySize = 3;
            heroesProperty.GetArrayElementAtIndex(0).objectReferenceValue = momo;
            heroesProperty.GetArrayElementAtIndex(1).objectReferenceValue = bulwark;
            heroesProperty.GetArrayElementAtIndex(2).objectReferenceValue = sprout;
            serializedSelection.FindProperty("startingHero").objectReferenceValue = momo;
            serializedSelection.ApplyModifiedPropertiesWithoutUndo();

            return selectionManager;
        }

        private static ProgressionService CreateProgressionService()
        {
            GameObject progressionObject = new GameObject("Progression Service");
            return progressionObject.AddComponent<ProgressionService>();
        }

        private static GameObject CreateTowerPrefab(
            string towerObjectName,
            string prefabPath,
            string materialName,
            Color bodyColor,
            string towerName,
            string towerFamilyId,
            float attackRange,
            float attacksPerSecond,
            int attackDamage,
            int upgradeCost,
            int damagePerUpgrade,
            float rangePerUpgrade,
            float attackSpeedPerUpgrade,
            float scaleMultiplierPerUpgrade,
            float slowDurationOnHit = 0f,
            float slowMultiplierOnHit = 1f)
        {
            GameObject tower = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            tower.name = towerObjectName;
            tower.transform.localScale = new Vector3(1f, 1.5f, 1f);
            AssignMaterial(tower, materialName, bodyColor);
            TowerAttack towerAttack = tower.AddComponent<TowerAttack>();

            SerializedObject serializedTower = new SerializedObject(towerAttack);
            serializedTower.FindProperty("towerName").stringValue = towerName;
            serializedTower.FindProperty("towerFamilyId").stringValue = towerFamilyId;
            serializedTower.FindProperty("attackRange").floatValue = attackRange;
            serializedTower.FindProperty("attacksPerSecond").floatValue = attacksPerSecond;
            serializedTower.FindProperty("attackDamage").intValue = attackDamage;
            serializedTower.FindProperty("upgradeCost").intValue = upgradeCost;
            serializedTower.FindProperty("damagePerUpgrade").intValue = damagePerUpgrade;
            serializedTower.FindProperty("rangePerUpgrade").floatValue = rangePerUpgrade;
            serializedTower.FindProperty("attackSpeedPerUpgrade").floatValue = attackSpeedPerUpgrade;
            serializedTower.FindProperty("scaleMultiplierPerUpgrade").floatValue = scaleMultiplierPerUpgrade;
            serializedTower.FindProperty("slowDurationOnHit").floatValue = slowDurationOnHit;
            serializedTower.FindProperty("slowMultiplierOnHit").floatValue = slowMultiplierOnHit;
            serializedTower.ApplyModifiedPropertiesWithoutUndo();

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(tower, prefabPath);
            Object.DestroyImmediate(tower);
            return prefab;
        }

        private static TowerBuildManager CreateTowerBuildManager(GameObject towerPrefab, GameObject burstTowerPrefab, GameObject frostTowerPrefab)
        {
            GameObject managerObject = new GameObject("Tower Build Manager");
            TowerBuildManager buildManager = managerObject.AddComponent<TowerBuildManager>();

            SerializedObject serializedManager = new SerializedObject(buildManager);
            SerializedProperty optionsProperty = serializedManager.FindProperty("buildOptions");
            optionsProperty.arraySize = 3;

            ConfigureBuildOption(optionsProperty.GetArrayElementAtIndex(0), "Star", towerPrefab, 60);
            ConfigureBuildOption(optionsProperty.GetArrayElementAtIndex(1), "Burst", burstTowerPrefab, 75);
            ConfigureBuildOption(optionsProperty.GetArrayElementAtIndex(2), "Frost", frostTowerPrefab, 70);

            serializedManager.FindProperty("startingOptionIndex").intValue = 0;
            serializedManager.ApplyModifiedPropertiesWithoutUndo();
            return buildManager;
        }

        private static void ConfigureBuildOption(SerializedProperty optionProperty, string displayName, GameObject towerPrefab, int buildCost)
        {
            optionProperty.FindPropertyRelative("displayName").stringValue = displayName;
            optionProperty.FindPropertyRelative("towerPrefab").objectReferenceValue = towerPrefab;
            optionProperty.FindPropertyRelative("buildCost").intValue = buildCost;
        }

        private static void CreateBuildNodes(GameState gameState, ProgressionService progressionService, TowerBuildManager buildManager)
        {
            Vector3[] nodePositions =
            {
                new Vector3(-5.6f, 0.08f, 1.3f),
                new Vector3(-0.2f, 0.08f, 1.5f),
                new Vector3(2.2f, 0.08f, -4.2f),
                new Vector3(5.5f, 0.08f, 0.4f)
            };

            for (int i = 0; i < nodePositions.Length; i++)
            {
                GameObject node = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                node.name = $"Build Node {i + 1}";
                node.transform.position = nodePositions[i];
                node.transform.localScale = new Vector3(1.25f, 0.08f, 1.25f);
                AssignMaterial(node, "Prototype_BuildNode", new Color(0.95f, 0.82f, 0.24f));

                TowerBuildNode buildNode = node.AddComponent<TowerBuildNode>();
                SerializedObject serializedNode = new SerializedObject(buildNode);
                serializedNode.FindProperty("gameState").objectReferenceValue = gameState;
                serializedNode.FindProperty("progressionService").objectReferenceValue = progressionService;
                serializedNode.FindProperty("buildManager").objectReferenceValue = buildManager;
                serializedNode.ApplyModifiedPropertiesWithoutUndo();
            }
        }

        private static WaveSpawner CreateWaveSpawner(
            GameObject enemyPrefab,
            GameObject toughEnemyPrefab,
            GameObject runnerEnemyPrefab,
            GameObject armoredEnemyPrefab,
            LevelDefinition levelDefinition,
            EnemyPath path,
            GameState gameState,
            HeroSelectionManager heroSelection)
        {
            GameObject spawnerObject = new GameObject("Wave Spawner");
            WaveSpawner spawner = spawnerObject.AddComponent<WaveSpawner>();

            SerializedObject serializedSpawner = new SerializedObject(spawner);
            serializedSpawner.FindProperty("enemyPrefab").objectReferenceValue = enemyPrefab;
            serializedSpawner.FindProperty("toughEnemyPrefab").objectReferenceValue = toughEnemyPrefab;
            serializedSpawner.FindProperty("runnerEnemyPrefab").objectReferenceValue = runnerEnemyPrefab;
            serializedSpawner.FindProperty("armoredEnemyPrefab").objectReferenceValue = armoredEnemyPrefab;
            serializedSpawner.FindProperty("levelDefinition").objectReferenceValue = levelDefinition;
            SerializedProperty catalogProperty = serializedSpawner.FindProperty("enemyCatalog");
            catalogProperty.arraySize = 4;
            ConfigureEnemyCatalogEntry(catalogProperty.GetArrayElementAtIndex(0), "Basic", enemyPrefab);
            ConfigureEnemyCatalogEntry(catalogProperty.GetArrayElementAtIndex(1), "Tough", toughEnemyPrefab);
            ConfigureEnemyCatalogEntry(catalogProperty.GetArrayElementAtIndex(2), "Runner", runnerEnemyPrefab);
            ConfigureEnemyCatalogEntry(catalogProperty.GetArrayElementAtIndex(3), "Armored", armoredEnemyPrefab);
            serializedSpawner.FindProperty("enemyPath").objectReferenceValue = path;
            serializedSpawner.FindProperty("gameState").objectReferenceValue = gameState;
            serializedSpawner.FindProperty("heroSelection").objectReferenceValue = heroSelection;
            serializedSpawner.FindProperty("totalWaves").intValue = 4;
            serializedSpawner.FindProperty("enemiesPerWave").intValue = 8;
            serializedSpawner.FindProperty("timeBetweenEnemies").floatValue = 0.7f;
            serializedSpawner.FindProperty("timeBetweenWaves").floatValue = 3f;
            serializedSpawner.ApplyModifiedPropertiesWithoutUndo();

            return spawner;
        }

        private static void ConfigureEnemyCatalogEntry(SerializedProperty entryProperty, string enemyId, GameObject prefab)
        {
            entryProperty.FindPropertyRelative("enemyId").stringValue = enemyId;
            entryProperty.FindPropertyRelative("prefab").objectReferenceValue = prefab;
        }

        private static void CreateEventSystem()
        {
            GameObject eventSystemObject = new GameObject("EventSystem");
            eventSystemObject.AddComponent<EventSystem>();
            eventSystemObject.AddComponent<StandaloneInputModule>();
        }

        private static void CreateHud(
            GameState gameState,
            ProgressionService progressionService,
            WaveSpawner waveSpawner,
            HeroSelectionManager heroSelection,
            TowerBuildManager buildManager,
            PrototypeHeroController momo,
            PrototypeHeroController bulwark,
            PrototypeHeroController sprout)
        {
            GameObject canvasObject = new GameObject("Prototype HUD");
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
            canvasObject.AddComponent<GraphicRaycaster>();

            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            Text livesText = CreateHudText(canvasObject.transform, "Lives Text", "Lives: 20", new Vector2(16f, -16f), TextAnchor.UpperLeft, font);
            Text goldText = CreateHudText(canvasObject.transform, "Gold Text", "Gold: 120", new Vector2(16f, -48f), TextAnchor.UpperLeft, font);
            Text softCurrencyText = CreateHudText(canvasObject.transform, "Soft Currency Text", "Crystals: 0", new Vector2(16f, -80f), TextAnchor.UpperLeft, font);
            Text waveText = CreateHudText(canvasObject.transform, "Wave Text", "Wave: 0/4", new Vector2(-16f, -16f), TextAnchor.UpperRight, font);
            Text rewardText = CreateHudText(canvasObject.transform, "Reward Text", string.Empty, new Vector2(0f, 74f), TextAnchor.MiddleCenter, font);
            Text messageText = CreateHudText(canvasObject.transform, "Message Text", string.Empty, new Vector2(0f, -18f), TextAnchor.UpperCenter, font);
            Text objectiveText = CreateHudText(canvasObject.transform, "Objective Text", "Build towers. Start waves. Defend the path.", new Vector2(0f, 18f), TextAnchor.LowerCenter, font);
            Button skillButton = CreateHudButton(canvasObject.transform, "Skill Button", new Vector2(16f, 82f), new Vector2(220f, 54f), font, out Text skillText);
            Button momoPortraitButton = CreateHudButton(canvasObject.transform, "Momo Portrait Button", new Vector2(16f, 16f), new Vector2(150f, 54f), font, out Text momoPortraitText);
            Button bulwarkPortraitButton = CreateHudButton(canvasObject.transform, "Bulwark Portrait Button", new Vector2(172f, 16f), new Vector2(150f, 54f), font, out Text bulwarkPortraitText);
            Button sproutPortraitButton = CreateHudButton(canvasObject.transform, "Sprout Portrait Button", new Vector2(328f, 16f), new Vector2(150f, 54f), font, out Text sproutPortraitText);
            Button starBuildButton = CreateHudButton(canvasObject.transform, "Star Tower Button", new Vector2(488f, 16f), new Vector2(132f, 54f), font, out Text starBuildText);
            Button burstBuildButton = CreateHudButton(canvasObject.transform, "Burst Tower Button", new Vector2(626f, 16f), new Vector2(132f, 54f), font, out Text burstBuildText);
            Button frostBuildButton = CreateHudButton(canvasObject.transform, "Frost Tower Button", new Vector2(764f, 16f), new Vector2(132f, 54f), font, out Text frostBuildText);
            Button progressionToggleButton = CreateHudButton(canvasObject.transform, "Upgrade Toggle Button", new Vector2(-216f, -76f), new Vector2(200f, 54f), font, out Text progressionToggleText);
            GameObject progressionPanel = CreateProgressionPanel(canvasObject.transform, font, out Text progressionTitleText, out Text progressionSummaryText, out Text momoSkillUpgradeText, out Button momoSkillUpgradeButton, out Text bulwarkSkillUpgradeText, out Button bulwarkSkillUpgradeButton, out Text sproutSkillUpgradeText, out Button sproutSkillUpgradeButton, out Text starTowerUpgradeText, out Button starTowerUpgradeButton, out Text burstTowerUpgradeText, out Button burstTowerUpgradeButton, out Text frostTowerUpgradeText, out Button frostTowerUpgradeButton, out Text starSpecializationText, out Button starSpecializationButton, out Text burstSpecializationText, out Button burstSpecializationButton, out Text frostSpecializationText, out Button frostSpecializationButton, out Text resetProgressionText, out Button resetProgressionButton);
            Button startWaveButton = CreateHudButton(canvasObject.transform, "Start Wave Button", new Vector2(-216f, 16f), font, out Text startWaveText);
            Button restartButton = CreateHudButton(canvasObject.transform, "Restart Button", new Vector2(0f, -72f), font, out Text restartText);
            Text resultText = CreateHudText(canvasObject.transform, "Result Text", string.Empty, Vector2.zero, TextAnchor.MiddleCenter, font);
            resultText.fontSize = 42;
            momoPortraitText.text = "Momo*";
            bulwarkPortraitText.text = "Bulwark";
            sproutPortraitText.text = "Sprout";
            skillText.text = "Momo Pop";
            startWaveText.text = "Start Wave";

            Image momoPortraitImage = momoPortraitButton.GetComponent<Image>();
            momoPortraitImage.color = new Color(1f, 0.7f, 0.88f, 0.95f);
            Image bulwarkPortraitImage = bulwarkPortraitButton.GetComponent<Image>();
            bulwarkPortraitImage.color = new Color(0.42f, 0.36f, 0.42f, 0.9f);
            Image sproutPortraitImage = sproutPortraitButton.GetComponent<Image>();
            sproutPortraitImage.color = new Color(0.42f, 0.36f, 0.42f, 0.9f);

            RectTransform startWaveRect = startWaveButton.GetComponent<RectTransform>();
            startWaveRect.anchorMin = new Vector2(1f, 0f);
            startWaveRect.anchorMax = new Vector2(1f, 0f);
            startWaveRect.pivot = new Vector2(1f, 0f);
            startWaveRect.anchoredPosition = new Vector2(-16f, 16f);

            RectTransform progressionToggleRect = progressionToggleButton.GetComponent<RectTransform>();
            progressionToggleRect.anchorMin = new Vector2(1f, 1f);
            progressionToggleRect.anchorMax = new Vector2(1f, 1f);
            progressionToggleRect.pivot = new Vector2(1f, 1f);
            progressionToggleRect.anchoredPosition = new Vector2(-16f, -76f);

            RectTransform rewardRect = rewardText.GetComponent<RectTransform>();
            rewardRect.anchorMin = new Vector2(0.5f, 0.5f);
            rewardRect.anchorMax = new Vector2(0.5f, 0.5f);
            rewardRect.pivot = new Vector2(0.5f, 0.5f);
            rewardRect.anchoredPosition = new Vector2(0f, 74f);
            rewardRect.sizeDelta = new Vector2(420f, 48f);
            rewardText.color = new Color(0.58f, 0.9f, 1f);

            RectTransform messageRect = messageText.GetComponent<RectTransform>();
            messageRect.anchorMin = new Vector2(0.5f, 1f);
            messageRect.anchorMax = new Vector2(0.5f, 1f);
            messageRect.pivot = new Vector2(0.5f, 1f);
            messageRect.anchoredPosition = new Vector2(0f, -18f);
            messageRect.sizeDelta = new Vector2(520f, 48f);
            messageText.color = new Color(1f, 0.95f, 0.55f);

            RectTransform objectiveRect = objectiveText.GetComponent<RectTransform>();
            objectiveRect.anchorMin = new Vector2(0.5f, 0f);
            objectiveRect.anchorMax = new Vector2(0.5f, 0f);
            objectiveRect.pivot = new Vector2(0.5f, 0f);
            objectiveRect.anchoredPosition = new Vector2(0f, 84f);
            objectiveRect.sizeDelta = new Vector2(720f, 48f);
            objectiveText.fontSize = 22;
            objectiveText.color = new Color(0.92f, 1f, 0.9f);

            RectTransform resultRect = resultText.GetComponent<RectTransform>();
            resultRect.anchorMin = new Vector2(0f, 0f);
            resultRect.anchorMax = new Vector2(1f, 1f);
            resultRect.offsetMin = Vector2.zero;
            resultRect.offsetMax = Vector2.zero;

            RectTransform restartRect = restartButton.GetComponent<RectTransform>();
            restartRect.anchorMin = new Vector2(0.5f, 0.5f);
            restartRect.anchorMax = new Vector2(0.5f, 0.5f);
            restartRect.pivot = new Vector2(0.5f, 0.5f);
            restartRect.anchoredPosition = new Vector2(0f, -72f);
            restartText.text = "Restart";
            restartButton.gameObject.SetActive(false);

            PrototypeHud hud = canvasObject.AddComponent<PrototypeHud>();
            SerializedObject serializedHud = new SerializedObject(hud);
            serializedHud.FindProperty("gameState").objectReferenceValue = gameState;
            serializedHud.FindProperty("progressionService").objectReferenceValue = progressionService;
            serializedHud.FindProperty("waveSpawner").objectReferenceValue = waveSpawner;
            serializedHud.FindProperty("heroSelection").objectReferenceValue = heroSelection;
            serializedHud.FindProperty("buildManager").objectReferenceValue = buildManager;
            serializedHud.FindProperty("momoHero").objectReferenceValue = momo;
            serializedHud.FindProperty("bulwarkHero").objectReferenceValue = bulwark;
            serializedHud.FindProperty("sproutHero").objectReferenceValue = sprout;
            serializedHud.FindProperty("livesText").objectReferenceValue = livesText;
            serializedHud.FindProperty("goldText").objectReferenceValue = goldText;
            serializedHud.FindProperty("waveText").objectReferenceValue = waveText;
            serializedHud.FindProperty("softCurrencyText").objectReferenceValue = softCurrencyText;
            serializedHud.FindProperty("skillText").objectReferenceValue = skillText;
            serializedHud.FindProperty("skillButton").objectReferenceValue = skillButton;
            serializedHud.FindProperty("momoPortraitText").objectReferenceValue = momoPortraitText;
            serializedHud.FindProperty("momoPortraitButton").objectReferenceValue = momoPortraitButton;
            serializedHud.FindProperty("momoPortraitImage").objectReferenceValue = momoPortraitImage;
            serializedHud.FindProperty("bulwarkPortraitText").objectReferenceValue = bulwarkPortraitText;
            serializedHud.FindProperty("bulwarkPortraitButton").objectReferenceValue = bulwarkPortraitButton;
            serializedHud.FindProperty("bulwarkPortraitImage").objectReferenceValue = bulwarkPortraitImage;
            serializedHud.FindProperty("sproutPortraitText").objectReferenceValue = sproutPortraitText;
            serializedHud.FindProperty("sproutPortraitButton").objectReferenceValue = sproutPortraitButton;
            serializedHud.FindProperty("sproutPortraitImage").objectReferenceValue = sproutPortraitImage;
            serializedHud.FindProperty("towerBuildText").objectReferenceValue = starBuildText;
            serializedHud.FindProperty("towerBuildButton").objectReferenceValue = starBuildButton;
            serializedHud.FindProperty("burstBuildText").objectReferenceValue = burstBuildText;
            serializedHud.FindProperty("burstBuildButton").objectReferenceValue = burstBuildButton;
            serializedHud.FindProperty("frostBuildText").objectReferenceValue = frostBuildText;
            serializedHud.FindProperty("frostBuildButton").objectReferenceValue = frostBuildButton;
            serializedHud.FindProperty("startWaveText").objectReferenceValue = startWaveText;
            serializedHud.FindProperty("startWaveButton").objectReferenceValue = startWaveButton;
            serializedHud.FindProperty("progressionPanel").objectReferenceValue = progressionPanel;
            serializedHud.FindProperty("progressionTitleText").objectReferenceValue = progressionTitleText;
            serializedHud.FindProperty("progressionSummaryText").objectReferenceValue = progressionSummaryText;
            serializedHud.FindProperty("progressionToggleText").objectReferenceValue = progressionToggleText;
            serializedHud.FindProperty("progressionToggleButton").objectReferenceValue = progressionToggleButton;
            serializedHud.FindProperty("momoSkillUpgradeText").objectReferenceValue = momoSkillUpgradeText;
            serializedHud.FindProperty("momoSkillUpgradeButton").objectReferenceValue = momoSkillUpgradeButton;
            serializedHud.FindProperty("bulwarkSkillUpgradeText").objectReferenceValue = bulwarkSkillUpgradeText;
            serializedHud.FindProperty("bulwarkSkillUpgradeButton").objectReferenceValue = bulwarkSkillUpgradeButton;
            serializedHud.FindProperty("sproutSkillUpgradeText").objectReferenceValue = sproutSkillUpgradeText;
            serializedHud.FindProperty("sproutSkillUpgradeButton").objectReferenceValue = sproutSkillUpgradeButton;
            serializedHud.FindProperty("starTowerUpgradeText").objectReferenceValue = starTowerUpgradeText;
            serializedHud.FindProperty("starTowerUpgradeButton").objectReferenceValue = starTowerUpgradeButton;
            serializedHud.FindProperty("burstTowerUpgradeText").objectReferenceValue = burstTowerUpgradeText;
            serializedHud.FindProperty("burstTowerUpgradeButton").objectReferenceValue = burstTowerUpgradeButton;
            serializedHud.FindProperty("frostTowerUpgradeText").objectReferenceValue = frostTowerUpgradeText;
            serializedHud.FindProperty("frostTowerUpgradeButton").objectReferenceValue = frostTowerUpgradeButton;
            serializedHud.FindProperty("starSpecializationText").objectReferenceValue = starSpecializationText;
            serializedHud.FindProperty("starSpecializationButton").objectReferenceValue = starSpecializationButton;
            serializedHud.FindProperty("burstSpecializationText").objectReferenceValue = burstSpecializationText;
            serializedHud.FindProperty("burstSpecializationButton").objectReferenceValue = burstSpecializationButton;
            serializedHud.FindProperty("frostSpecializationText").objectReferenceValue = frostSpecializationText;
            serializedHud.FindProperty("frostSpecializationButton").objectReferenceValue = frostSpecializationButton;
            serializedHud.FindProperty("resetProgressionText").objectReferenceValue = resetProgressionText;
            serializedHud.FindProperty("resetProgressionButton").objectReferenceValue = resetProgressionButton;
            serializedHud.FindProperty("rewardText").objectReferenceValue = rewardText;
            serializedHud.FindProperty("messageText").objectReferenceValue = messageText;
            serializedHud.FindProperty("resultText").objectReferenceValue = resultText;
            serializedHud.FindProperty("restartButton").objectReferenceValue = restartButton;
            serializedHud.FindProperty("restartText").objectReferenceValue = restartText;
            serializedHud.ApplyModifiedPropertiesWithoutUndo();
            progressionPanel.SetActive(false);
        }

        private static GameObject CreateProgressionPanel(
            Transform parent,
            Font font,
            out Text titleText,
            out Text summaryText,
            out Text momoSkillText,
            out Button momoSkillButton,
            out Text bulwarkSkillText,
            out Button bulwarkSkillButton,
            out Text sproutSkillText,
            out Button sproutSkillButton,
            out Text starTowerText,
            out Button starTowerButton,
            out Text burstTowerText,
            out Button burstTowerButton,
            out Text frostTowerText,
            out Button frostTowerButton,
            out Text starSpecializationText,
            out Button starSpecializationButton,
            out Text burstSpecializationText,
            out Button burstSpecializationButton,
            out Text frostSpecializationText,
            out Button frostSpecializationButton,
            out Text resetText,
            out Button resetButton)
        {
            GameObject panelObject = new GameObject("Progression Panel");
            panelObject.transform.SetParent(parent);

            Image image = panelObject.AddComponent<Image>();
            image.color = new Color(0.13f, 0.15f, 0.18f, 0.92f);

            RectTransform panelRect = panelObject.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(1f, 1f);
            panelRect.anchorMax = new Vector2(1f, 1f);
            panelRect.pivot = new Vector2(1f, 1f);
            panelRect.anchoredPosition = new Vector2(-16f, -138f);
            panelRect.sizeDelta = new Vector2(520f, 430f);

            titleText = CreateHudText(panelObject.transform, "Progression Title", "Upgrades", new Vector2(16f, -14f), TextAnchor.UpperLeft, font);
            titleText.fontSize = 28;
            titleText.color = new Color(1f, 0.9f, 0.55f);

            summaryText = CreateHudText(panelObject.transform, "Progression Summary", "Crystals 0", new Vector2(250f, -16f), TextAnchor.UpperLeft, font);
            summaryText.alignment = TextAnchor.UpperRight;
            RectTransform summaryRect = summaryText.GetComponent<RectTransform>();
            summaryRect.sizeDelta = new Vector2(250f, 40f);

            momoSkillButton = CreateHudButton(panelObject.transform, "Momo Skill Rank Button", new Vector2(16f, 292f), new Vector2(236f, 48f), font, out momoSkillText);
            bulwarkSkillButton = CreateHudButton(panelObject.transform, "Bulwark Skill Rank Button", new Vector2(268f, 292f), new Vector2(236f, 48f), font, out bulwarkSkillText);
            sproutSkillButton = CreateHudButton(panelObject.transform, "Sprout Skill Rank Button", new Vector2(16f, 232f), new Vector2(236f, 48f), font, out sproutSkillText);
            starTowerButton = CreateHudButton(panelObject.transform, "Star Tower Rank Button", new Vector2(268f, 232f), new Vector2(236f, 48f), font, out starTowerText);
            burstTowerButton = CreateHudButton(panelObject.transform, "Burst Tower Rank Button", new Vector2(16f, 172f), new Vector2(236f, 48f), font, out burstTowerText);
            frostTowerButton = CreateHudButton(panelObject.transform, "Frost Tower Rank Button", new Vector2(268f, 172f), new Vector2(236f, 48f), font, out frostTowerText);
            starSpecializationButton = CreateHudButton(panelObject.transform, "Star Specialization Button", new Vector2(16f, 112f), new Vector2(152f, 44f), font, out starSpecializationText);
            burstSpecializationButton = CreateHudButton(panelObject.transform, "Burst Specialization Button", new Vector2(184f, 112f), new Vector2(152f, 44f), font, out burstSpecializationText);
            frostSpecializationButton = CreateHudButton(panelObject.transform, "Frost Specialization Button", new Vector2(352f, 112f), new Vector2(152f, 44f), font, out frostSpecializationText);
            resetButton = CreateHudButton(panelObject.transform, "Reset Progression Button", new Vector2(16f, 24f), new Vector2(160f, 48f), font, out resetText);

            Text noteText = CreateHudText(panelObject.transform, "Progression Note", "Specializations unlock at tower rank 3.", new Vector2(16f, 82f), TextAnchor.UpperLeft, font);
            noteText.fontSize = 20;
            noteText.color = new Color(0.82f, 0.88f, 0.9f);
            RectTransform noteRect = noteText.GetComponent<RectTransform>();
            noteRect.sizeDelta = new Vector2(480f, 44f);

            return panelObject;
        }

        private static Text CreateHudText(Transform parent, string name, string text, Vector2 anchoredPosition, TextAnchor alignment, Font font)
        {
            GameObject textObject = new GameObject(name);
            textObject.transform.SetParent(parent);

            Text label = textObject.AddComponent<Text>();
            label.text = text;
            label.font = font;
            label.fontSize = 24;
            label.color = Color.white;
            label.alignment = alignment;
            label.raycastTarget = false;

            RectTransform rectTransform = label.GetComponent<RectTransform>();
            rectTransform.anchorMin = AnchorFor(alignment);
            rectTransform.anchorMax = AnchorFor(alignment);
            rectTransform.pivot = PivotFor(alignment);
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.sizeDelta = new Vector2(280f, 48f);

            return label;
        }

        private static Button CreateHudButton(Transform parent, string name, Vector2 anchoredPosition, Font font, out Text label)
        {
            return CreateHudButton(parent, name, anchoredPosition, new Vector2(190f, 54f), font, out label);
        }

        private static Button CreateHudButton(Transform parent, string name, Vector2 anchoredPosition, Vector2 size, Font font, out Text label)
        {
            GameObject buttonObject = new GameObject(name);
            buttonObject.transform.SetParent(parent);

            Image image = buttonObject.AddComponent<Image>();
            image.color = new Color(0.94f, 0.58f, 0.74f, 0.9f);
            Button button = buttonObject.AddComponent<Button>();

            RectTransform rectTransform = buttonObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0f, 0f);
            rectTransform.anchorMax = new Vector2(0f, 0f);
            rectTransform.pivot = new Vector2(0f, 0f);
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.sizeDelta = size;

            GameObject labelObject = new GameObject("Label");
            labelObject.transform.SetParent(buttonObject.transform);
            label = labelObject.AddComponent<Text>();
            label.text = "Momo Pop";
            label.font = font;
            label.fontSize = 22;
            label.color = Color.white;
            label.alignment = TextAnchor.MiddleCenter;
            label.raycastTarget = false;

            RectTransform labelRect = label.GetComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;

            ColorBlock colors = button.colors;
            colors.normalColor = Color.white;
            colors.highlightedColor = new Color(1f, 0.92f, 0.96f);
            colors.pressedColor = new Color(0.85f, 0.38f, 0.6f);
            colors.disabledColor = new Color(0.45f, 0.45f, 0.45f, 0.7f);
            button.colors = colors;

            return button;
        }

        private static Vector2 AnchorFor(TextAnchor anchor)
        {
            return anchor switch
            {
                TextAnchor.UpperRight => new Vector2(1f, 1f),
                TextAnchor.MiddleCenter => new Vector2(0.5f, 0.5f),
                _ => new Vector2(0f, 1f)
            };
        }

        private static Vector2 PivotFor(TextAnchor anchor)
        {
            return anchor switch
            {
                TextAnchor.UpperRight => new Vector2(1f, 1f),
                TextAnchor.MiddleCenter => new Vector2(0.5f, 0.5f),
                _ => new Vector2(0f, 1f)
            };
        }

        private static void CreatePathSegment(Transform parent, Vector3 start, Vector3 end, Material material, int index)
        {
            Vector3 midpoint = (start + end) * 0.5f + Vector3.up * 0.01f;
            float length = Vector3.Distance(start, end);

            GameObject segment = GameObject.CreatePrimitive(PrimitiveType.Cube);
            segment.name = $"Path Segment {index}";
            segment.transform.SetParent(parent);
            segment.transform.position = midpoint;
            segment.transform.localScale = new Vector3(0.6f, 0.025f, length);
            segment.transform.rotation = Quaternion.LookRotation(end - start, Vector3.up);
            segment.GetComponent<Renderer>().sharedMaterial = material;
        }

        private static void EnsureMaterialFolder()
        {
            if (!AssetDatabase.IsValidFolder(MaterialFolder))
            {
                AssetDatabase.CreateFolder("Assets/_MomosDefense", "Materials");
            }
        }

        private static void EnsureContentFolder()
        {
            if (!AssetDatabase.IsValidFolder("Assets/_MomosDefense/Data"))
            {
                AssetDatabase.CreateFolder("Assets/_MomosDefense", "Data");
            }

            if (!AssetDatabase.IsValidFolder(ContentFolder))
            {
                AssetDatabase.CreateFolder("Assets/_MomosDefense/Data", "Prototype");
            }
        }

        private static T LoadOrCreateAsset<T>(string path) where T : ScriptableObject
        {
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset != null)
            {
                return asset;
            }

            asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
            return asset;
        }

        private static void CreateHeroDefinition(string heroId, string displayName, string role, int startingLevel)
        {
            HeroDefinition hero = LoadOrCreateAsset<HeroDefinition>($"{ContentFolder}/Hero_{heroId}.asset");
            SerializedObject serializedHero = new SerializedObject(hero);
            serializedHero.FindProperty("heroId").stringValue = heroId;
            serializedHero.FindProperty("displayName").stringValue = displayName;
            serializedHero.FindProperty("role").stringValue = role;
            serializedHero.FindProperty("startingLevel").intValue = startingLevel;
            serializedHero.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(hero);
        }

        private static void CreateSkillDefinition(string skillId, string displayName, string ownerHeroId, int baseDamage, float baseRadius, float baseCooldown)
        {
            SkillDefinition skill = LoadOrCreateAsset<SkillDefinition>($"{ContentFolder}/Skill_{skillId}.asset");
            SerializedObject serializedSkill = new SerializedObject(skill);
            serializedSkill.FindProperty("skillId").stringValue = skillId;
            serializedSkill.FindProperty("displayName").stringValue = displayName;
            serializedSkill.FindProperty("ownerHeroId").stringValue = ownerHeroId;
            serializedSkill.FindProperty("baseDamage").intValue = baseDamage;
            serializedSkill.FindProperty("baseRadius").floatValue = baseRadius;
            serializedSkill.FindProperty("baseCooldown").floatValue = baseCooldown;
            serializedSkill.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(skill);
        }

        private static void CreateTowerDefinition(string towerFamilyId, string displayName, int buildCost, int baseDamage, float baseRange, float attacksPerSecond)
        {
            TowerDefinition tower = LoadOrCreateAsset<TowerDefinition>($"{ContentFolder}/Tower_{towerFamilyId}.asset");
            SerializedObject serializedTower = new SerializedObject(tower);
            serializedTower.FindProperty("towerFamilyId").stringValue = towerFamilyId;
            serializedTower.FindProperty("displayName").stringValue = displayName;
            serializedTower.FindProperty("buildCost").intValue = buildCost;
            serializedTower.FindProperty("baseDamage").intValue = baseDamage;
            serializedTower.FindProperty("baseRange").floatValue = baseRange;
            serializedTower.FindProperty("attacksPerSecond").floatValue = attacksPerSecond;
            serializedTower.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(tower);
        }

        private static void CreateEnemyDefinition(string enemyId, string displayName, int maxHealth, float moveSpeed, int goldReward, int experienceReward)
        {
            EnemyDefinition enemy = LoadOrCreateAsset<EnemyDefinition>($"{ContentFolder}/Enemy_{enemyId}.asset");
            SerializedObject serializedEnemy = new SerializedObject(enemy);
            serializedEnemy.FindProperty("enemyId").stringValue = enemyId;
            serializedEnemy.FindProperty("displayName").stringValue = displayName;
            serializedEnemy.FindProperty("maxHealth").intValue = maxHealth;
            serializedEnemy.FindProperty("moveSpeed").floatValue = moveSpeed;
            serializedEnemy.FindProperty("goldReward").intValue = goldReward;
            serializedEnemy.FindProperty("experienceReward").intValue = experienceReward;
            serializedEnemy.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(enemy);
        }

        private static void CreateEquipmentDefinition(string equipmentId, string displayName, EquipmentDefinition.EquipmentSlot slot, int skillDamageBonus, float towerAttackSpeedBonus, string specialModifier)
        {
            EquipmentDefinition equipment = LoadOrCreateAsset<EquipmentDefinition>($"{ContentFolder}/Equipment_{equipmentId}.asset");
            SerializedObject serializedEquipment = new SerializedObject(equipment);
            serializedEquipment.FindProperty("equipmentId").stringValue = equipmentId;
            serializedEquipment.FindProperty("displayName").stringValue = displayName;
            serializedEquipment.FindProperty("slot").enumValueIndex = (int)slot;
            serializedEquipment.FindProperty("heroSkillDamageBonus").intValue = skillDamageBonus;
            serializedEquipment.FindProperty("towerAttackSpeedBonus").floatValue = towerAttackSpeedBonus;
            serializedEquipment.FindProperty("specialModifier").stringValue = specialModifier;
            serializedEquipment.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(equipment);
        }

        private static void CreateUpgradeDefinition(string upgradeId, string displayName, string targetId, int maxRank, int baseCost, int costStep)
        {
            UpgradeDefinition upgrade = LoadOrCreateAsset<UpgradeDefinition>($"{ContentFolder}/Upgrade_{upgradeId}.asset");
            SerializedObject serializedUpgrade = new SerializedObject(upgrade);
            serializedUpgrade.FindProperty("upgradeId").stringValue = upgradeId;
            serializedUpgrade.FindProperty("displayName").stringValue = displayName;
            serializedUpgrade.FindProperty("targetId").stringValue = targetId;
            serializedUpgrade.FindProperty("maxRank").intValue = maxRank;
            serializedUpgrade.FindProperty("baseCost").intValue = baseCost;
            serializedUpgrade.FindProperty("costStep").intValue = costStep;
            serializedUpgrade.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(upgrade);
        }

        private static WaveDefinition CreateWaveDefinition(string waveId, (string enemyId, int count)[] groups)
        {
            WaveDefinition wave = LoadOrCreateAsset<WaveDefinition>($"{ContentFolder}/{waveId}.asset");
            SerializedObject serializedWave = new SerializedObject(wave);
            serializedWave.FindProperty("waveId").stringValue = waveId;
            SerializedProperty groupsProperty = serializedWave.FindProperty("spawnGroups");
            groupsProperty.arraySize = groups.Length;

            for (int index = 0; index < groups.Length; index++)
            {
                SerializedProperty groupProperty = groupsProperty.GetArrayElementAtIndex(index);
                groupProperty.FindPropertyRelative("enemyId").stringValue = groups[index].enemyId;
                groupProperty.FindPropertyRelative("count").intValue = groups[index].count;
            }

            serializedWave.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(wave);
            return wave;
        }

        private static void AssignMaterial(GameObject target, string materialName, Color color)
        {
            Renderer renderer = target.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = GetOrCreateMaterial(materialName, color);
            }
        }

        private static Material GetOrCreateMaterial(string materialName, Color color)
        {
            string path = $"{MaterialFolder}/{materialName}.mat";
            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (material != null)
            {
                material.color = color;
                EditorUtility.SetDirty(material);
                return material;
            }

            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null)
            {
                shader = Shader.Find("Standard");
            }

            material = new Material(shader);
            material.color = color;
            AssetDatabase.CreateAsset(material, path);
            return material;
        }
    }
}
