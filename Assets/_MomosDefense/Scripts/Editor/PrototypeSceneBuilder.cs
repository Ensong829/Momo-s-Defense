using MomosDefense.Combat;
using MomosDefense.Core;
using MomosDefense.Enemies;
using MomosDefense.Heroes;
using MomosDefense.Towers;
using MomosDefense.Waves;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MomosDefense.Editor
{
    public static class PrototypeSceneBuilder
    {
        private const string ScenePath = "Assets/_MomosDefense/Scenes/Prototype_MomoDefense.unity";
        private const string EnemyPrefabPath = "Assets/_MomosDefense/Prefabs/Enemies/PrototypeEnemy.prefab";

        [MenuItem("Momo's Defense/Build Prototype Scene")]
        public static void BuildPrototypeScene()
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            GameObject gameStateObject = new GameObject("Game State");
            GameState gameState = gameStateObject.AddComponent<GameState>();

            CreateCamera();
            CreateLight();
            CreateGround();
            EnemyPath path = CreatePath();
            GameObject enemyPrefab = CreateEnemyPrefab();
            CreateMomo();
            CreateStarterTower();
            CreateWaveSpawner(enemyPrefab, path, gameState);

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
            camera.orthographicSize = 8f;
        }

        private static void CreateLight()
        {
            GameObject lightObject = new GameObject("Directional Light");
            Light light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.2f;
            lightObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
        }

        private static void CreateGround()
        {
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Prototype Ground";
            ground.transform.localScale = new Vector3(2f, 1f, 2f);
        }

        private static EnemyPath CreatePath()
        {
            GameObject pathObject = new GameObject("Enemy Path");
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
            }

            return pathObject.AddComponent<EnemyPath>();
        }

        private static GameObject CreateEnemyPrefab()
        {
            GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            enemy.name = "Prototype Enemy";
            enemy.tag = "Enemy";
            enemy.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            enemy.AddComponent<Health>();
            enemy.AddComponent<EnemyPathFollower>();

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(enemy, EnemyPrefabPath);
            Object.DestroyImmediate(enemy);
            return prefab;
        }

        private static void CreateMomo()
        {
            GameObject momo = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            momo.name = "Momo Prototype Hero";
            momo.transform.position = new Vector3(-5f, 1f, 0f);
            momo.transform.localScale = new Vector3(0.9f, 1.1f, 0.9f);
            momo.AddComponent<MomoHeroController>();
        }

        private static void CreateStarterTower()
        {
            GameObject tower = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            tower.name = "Starter Tower";
            tower.transform.position = new Vector3(0f, 0.75f, 1.5f);
            tower.transform.localScale = new Vector3(1f, 1.5f, 1f);
            tower.AddComponent<TowerAttack>();
        }

        private static void CreateWaveSpawner(GameObject enemyPrefab, EnemyPath path, GameState gameState)
        {
            GameObject spawnerObject = new GameObject("Wave Spawner");
            WaveSpawner spawner = spawnerObject.AddComponent<WaveSpawner>();

            SerializedObject serializedSpawner = new SerializedObject(spawner);
            serializedSpawner.FindProperty("enemyPrefab").objectReferenceValue = enemyPrefab;
            serializedSpawner.FindProperty("enemyPath").objectReferenceValue = path;
            serializedSpawner.FindProperty("gameState").objectReferenceValue = gameState;
            serializedSpawner.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}

