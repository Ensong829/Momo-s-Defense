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
        private const string TowerPrefabPath = "Assets/_MomosDefense/Prefabs/Towers/PrototypeStarterTower.prefab";
        private const string MaterialFolder = "Assets/_MomosDefense/Materials";

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
            GameObject towerPrefab = CreateTowerPrefab();
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
            HeroSelectionManager heroSelection = CreateHeroSelectionManager(momo, bulwark, sprout);
            CreateBuildNodes(gameState, towerPrefab);
            WaveSpawner waveSpawner = CreateWaveSpawner(enemyPrefab, toughEnemyPrefab, runnerEnemyPrefab, path, gameState);
            CreateEventSystem();
            CreateHud(gameState, waveSpawner, heroSelection, momo, bulwark, sprout);

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
            serializedFollower.ApplyModifiedPropertiesWithoutUndo();

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(enemy, RunnerEnemyPrefabPath);
            Object.DestroyImmediate(enemy);
            return prefab;
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

        private static GameObject CreateTowerPrefab()
        {
            GameObject tower = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            tower.name = "Prototype Starter Tower";
            tower.transform.localScale = new Vector3(1f, 1.5f, 1f);
            AssignMaterial(tower, "Prototype_Tower", new Color(0.25f, 0.43f, 0.86f));
            tower.AddComponent<TowerAttack>();

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(tower, TowerPrefabPath);
            Object.DestroyImmediate(tower);
            return prefab;
        }

        private static void CreateBuildNodes(GameState gameState, GameObject towerPrefab)
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
                serializedNode.FindProperty("towerPrefab").objectReferenceValue = towerPrefab;
                serializedNode.ApplyModifiedPropertiesWithoutUndo();
            }
        }

        private static WaveSpawner CreateWaveSpawner(
            GameObject enemyPrefab,
            GameObject toughEnemyPrefab,
            GameObject runnerEnemyPrefab,
            EnemyPath path,
            GameState gameState)
        {
            GameObject spawnerObject = new GameObject("Wave Spawner");
            WaveSpawner spawner = spawnerObject.AddComponent<WaveSpawner>();

            SerializedObject serializedSpawner = new SerializedObject(spawner);
            serializedSpawner.FindProperty("enemyPrefab").objectReferenceValue = enemyPrefab;
            serializedSpawner.FindProperty("toughEnemyPrefab").objectReferenceValue = toughEnemyPrefab;
            serializedSpawner.FindProperty("runnerEnemyPrefab").objectReferenceValue = runnerEnemyPrefab;
            serializedSpawner.FindProperty("enemyPath").objectReferenceValue = path;
            serializedSpawner.FindProperty("gameState").objectReferenceValue = gameState;
            serializedSpawner.ApplyModifiedPropertiesWithoutUndo();

            return spawner;
        }

        private static void CreateEventSystem()
        {
            GameObject eventSystemObject = new GameObject("EventSystem");
            eventSystemObject.AddComponent<EventSystem>();
            eventSystemObject.AddComponent<StandaloneInputModule>();
        }

        private static void CreateHud(
            GameState gameState,
            WaveSpawner waveSpawner,
            HeroSelectionManager heroSelection,
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
            Text waveText = CreateHudText(canvasObject.transform, "Wave Text", "Wave: 0/3", new Vector2(-16f, -16f), TextAnchor.UpperRight, font);
            Text messageText = CreateHudText(canvasObject.transform, "Message Text", string.Empty, new Vector2(0f, -18f), TextAnchor.UpperCenter, font);
            Text objectiveText = CreateHudText(canvasObject.transform, "Objective Text", "Build towers. Start waves. Defend the path.", new Vector2(0f, 18f), TextAnchor.LowerCenter, font);
            Button skillButton = CreateHudButton(canvasObject.transform, "Skill Button", new Vector2(16f, 82f), new Vector2(220f, 54f), font, out Text skillText);
            Button momoPortraitButton = CreateHudButton(canvasObject.transform, "Momo Portrait Button", new Vector2(16f, 16f), new Vector2(150f, 54f), font, out Text momoPortraitText);
            Button bulwarkPortraitButton = CreateHudButton(canvasObject.transform, "Bulwark Portrait Button", new Vector2(172f, 16f), new Vector2(150f, 54f), font, out Text bulwarkPortraitText);
            Button sproutPortraitButton = CreateHudButton(canvasObject.transform, "Sprout Portrait Button", new Vector2(328f, 16f), new Vector2(150f, 54f), font, out Text sproutPortraitText);
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
            objectiveRect.anchoredPosition = new Vector2(0f, 18f);
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
            serializedHud.FindProperty("waveSpawner").objectReferenceValue = waveSpawner;
            serializedHud.FindProperty("heroSelection").objectReferenceValue = heroSelection;
            serializedHud.FindProperty("momoHero").objectReferenceValue = momo;
            serializedHud.FindProperty("bulwarkHero").objectReferenceValue = bulwark;
            serializedHud.FindProperty("sproutHero").objectReferenceValue = sprout;
            serializedHud.FindProperty("livesText").objectReferenceValue = livesText;
            serializedHud.FindProperty("goldText").objectReferenceValue = goldText;
            serializedHud.FindProperty("waveText").objectReferenceValue = waveText;
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
            serializedHud.FindProperty("startWaveText").objectReferenceValue = startWaveText;
            serializedHud.FindProperty("startWaveButton").objectReferenceValue = startWaveButton;
            serializedHud.FindProperty("messageText").objectReferenceValue = messageText;
            serializedHud.FindProperty("resultText").objectReferenceValue = resultText;
            serializedHud.FindProperty("restartButton").objectReferenceValue = restartButton;
            serializedHud.FindProperty("restartText").objectReferenceValue = restartText;
            serializedHud.ApplyModifiedPropertiesWithoutUndo();
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
