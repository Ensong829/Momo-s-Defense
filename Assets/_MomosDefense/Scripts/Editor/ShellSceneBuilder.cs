using System.Collections.Generic;
using MomosDefense.Core;
using MomosDefense.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MomosDefense.Editor
{
    public static class ShellSceneBuilder
    {
        private const string MainMenuScenePath = "Assets/_MomosDefense/Scenes/MainMenu.unity";
        private const string LevelSelectScenePath = "Assets/_MomosDefense/Scenes/LevelSelect.unity";
        private const string BattleScenePath = "Assets/_MomosDefense/Scenes/Prototype_MomoDefense.unity";
        private const string ContentFolder = "Assets/_MomosDefense/Data/Prototype";

        [MenuItem("Momo's Defense/Build Shell Scenes")]
        public static void BuildShellScenes()
        {
            EnsureScenesFolder();
            PrototypeSceneBuilder.BuildPrototypeScene();
            BuildMainMenuScene();
            BuildLevelSelectScene();
            SyncBuildSettings();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [MenuItem("Momo's Defense/Build Main Menu Scene")]
        public static void BuildMainMenuScene()
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            MainMenuShellController controller = new GameObject("Main Menu Shell").AddComponent<MainMenuShellController>();
            SerializedObject serializedController = new SerializedObject(controller);
            serializedController.FindProperty("levelSelectSceneName").stringValue = "LevelSelect";
            serializedController.FindProperty("quickPlaySceneName").stringValue = "Prototype_MomoDefense";
            serializedController.ApplyModifiedPropertiesWithoutUndo();

            Canvas canvas = CreateCanvas("Main Menu Canvas");
            CreateEventSystem();

            CreatePanel(canvas.transform, new Vector2(0.5f, 0.5f), new Vector2(560f, 520f), new Color(0.11f, 0.14f, 0.18f, 0.94f));
            Text title = CreateText(canvas.transform, "Title", "Momo's Defense", new Vector2(0.5f, 0.5f), new Vector2(0f, 186f), new Vector2(520f, 56f), font, 40, TextAnchor.MiddleCenter);
            title.color = new Color(1f, 0.92f, 0.62f);

            Text subtitle = CreateText(canvas.transform, "Subtitle", "Small demo shell for the three-hero prototype", new Vector2(0.5f, 0.5f), new Vector2(0f, 134f), new Vector2(480f, 44f), font, 22, TextAnchor.MiddleCenter);
            subtitle.color = new Color(0.85f, 0.9f, 0.95f);

            Button playButton = CreateButton(canvas.transform, "Play Button", "Campaign", new Vector2(0.5f, 0.5f), new Vector2(0f, 40f), new Vector2(260f, 56f), font);
            playButton.onClick.AddListener(controller.OpenLevelSelect);

            Button quickPlayButton = CreateButton(canvas.transform, "Quick Play Button", "Quick Play", new Vector2(0.5f, 0.5f), new Vector2(0f, -32f), new Vector2(260f, 56f), font);
            quickPlayButton.onClick.AddListener(controller.StartQuickPlay);

            Button quitButton = CreateButton(canvas.transform, "Quit Button", "Quit", new Vector2(0.5f, 0.5f), new Vector2(0f, -104f), new Vector2(260f, 56f), font);
            quitButton.onClick.AddListener(controller.QuitGame);

            EditorSceneManager.SaveScene(scene, MainMenuScenePath);
        }

        [MenuItem("Momo's Defense/Build Level Select Scene")]
        public static void BuildLevelSelectScene()
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            LevelDefinition[] levels = LoadLevels();

            LevelSelectShellController controller = new GameObject("Level Select Shell").AddComponent<LevelSelectShellController>();
            SerializedObject serializedController = new SerializedObject(controller);
            serializedController.FindProperty("mainMenuSceneName").stringValue = "MainMenu";
            serializedController.FindProperty("battleSceneName").stringValue = "Prototype_MomoDefense";
            if (levels.Length > 0)
            {
                serializedController.FindProperty("defaultLevel").objectReferenceValue = levels[0];
            }

            Canvas canvas = CreateCanvas("Level Select Canvas");
            CreateEventSystem();

            CreatePanel(canvas.transform, new Vector2(0.5f, 0.5f), new Vector2(860f, 620f), new Color(0.11f, 0.14f, 0.18f, 0.94f));
            Text title = CreateText(canvas.transform, "Title", "Select Level", new Vector2(0.5f, 0.5f), new Vector2(0f, 246f), new Vector2(520f, 56f), font, 36, TextAnchor.MiddleCenter);
            title.color = new Color(1f, 0.92f, 0.62f);

            List<LevelSelectOptionButton> levelButtons = new List<LevelSelectOptionButton>();
            for (int index = 0; index < levels.Length; index++)
            {
                float y = 120f - (index * 94f);
                LevelSelectOptionButton optionButton = CreateLevelOption(canvas.transform, font, levels[index], new Vector2(0f, y));
                levelButtons.Add(optionButton);
            }

            Button startButton = CreateButton(canvas.transform, "Start Button", "Start", new Vector2(0.5f, 0.5f), new Vector2(-108f, -236f), new Vector2(200f, 54f), font);
            startButton.onClick.AddListener(controller.StartSelectedLevel);

            Button backButton = CreateButton(canvas.transform, "Back Button", "Back", new Vector2(0.5f, 0.5f), new Vector2(108f, -236f), new Vector2(200f, 54f), font);
            backButton.onClick.AddListener(controller.BackToMainMenu);

            SerializedProperty buttonsProperty = serializedController.FindProperty("levelButtons");
            buttonsProperty.arraySize = levelButtons.Count;
            for (int index = 0; index < levelButtons.Count; index++)
            {
                buttonsProperty.GetArrayElementAtIndex(index).objectReferenceValue = levelButtons[index];
            }

            serializedController.ApplyModifiedPropertiesWithoutUndo();
            EditorSceneManager.SaveScene(scene, LevelSelectScenePath);
        }

        [MenuItem("Momo's Defense/Sync Build Settings")]
        public static void SyncBuildSettings()
        {
            List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();

            AddSceneIfExists(scenes, MainMenuScenePath);
            AddSceneIfExists(scenes, LevelSelectScenePath);
            AddSceneIfExists(scenes, BattleScenePath);

            EditorBuildSettings.scenes = scenes.ToArray();
        }

        private static LevelDefinition[] LoadLevels()
        {
            List<LevelDefinition> levels = new List<LevelDefinition>
            {
                AssetDatabase.LoadAssetAtPath<LevelDefinition>($"{ContentFolder}/PrototypeLevel01.asset"),
                AssetDatabase.LoadAssetAtPath<LevelDefinition>($"{ContentFolder}/PrototypeLevel02.asset"),
                AssetDatabase.LoadAssetAtPath<LevelDefinition>($"{ContentFolder}/PrototypeLevel03.asset")
            };

            levels.RemoveAll(level => level == null);
            return levels.ToArray();
        }

        private static LevelSelectOptionButton CreateLevelOption(Transform parent, Font font, LevelDefinition level, Vector2 anchoredPosition)
        {
            GameObject buttonObject = new GameObject($"{level.DisplayName} Button");
            buttonObject.transform.SetParent(parent);

            Image background = buttonObject.AddComponent<Image>();
            background.color = Color.white;
            Button button = buttonObject.AddComponent<Button>();
            RectTransform rect = buttonObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = new Vector2(640f, 72f);

            Text label = CreateText(buttonObject.transform, "Label", level.DisplayName, new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(600f, 40f), font, 24, TextAnchor.MiddleCenter);

            LevelSelectOptionButton option = buttonObject.AddComponent<LevelSelectOptionButton>();
            SerializedObject serializedOption = new SerializedObject(option);
            serializedOption.FindProperty("level").objectReferenceValue = level;
            serializedOption.FindProperty("button").objectReferenceValue = button;
            serializedOption.FindProperty("label").objectReferenceValue = label;
            serializedOption.FindProperty("background").objectReferenceValue = background;
            serializedOption.ApplyModifiedPropertiesWithoutUndo();

            return option;
        }

        private static Canvas CreateCanvas(string name)
        {
            GameObject canvasObject = new GameObject(name);
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            canvasObject.AddComponent<GraphicRaycaster>();
            return canvas;
        }

        private static void CreateEventSystem()
        {
            if (Object.FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>() != null)
            {
                return;
            }

            GameObject eventSystemObject = new GameObject("EventSystem");
            eventSystemObject.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystemObject.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }

        private static GameObject CreatePanel(Transform parent, Vector2 anchor, Vector2 size, Color color)
        {
            GameObject panel = new GameObject("Panel");
            panel.transform.SetParent(parent);
            Image image = panel.AddComponent<Image>();
            image.color = color;
            RectTransform rect = panel.GetComponent<RectTransform>();
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = size;
            return panel;
        }

        private static Button CreateButton(Transform parent, string name, string text, Vector2 anchor, Vector2 position, Vector2 size, Font font)
        {
            GameObject buttonObject = new GameObject(name);
            buttonObject.transform.SetParent(parent);
            Image image = buttonObject.AddComponent<Image>();
            image.color = new Color(0.94f, 0.58f, 0.74f, 0.9f);
            Button button = buttonObject.AddComponent<Button>();

            RectTransform rect = buttonObject.GetComponent<RectTransform>();
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = position;
            rect.sizeDelta = size;

            CreateText(buttonObject.transform, "Label", text, new Vector2(0.5f, 0.5f), Vector2.zero, size, font, 24, TextAnchor.MiddleCenter);
            return button;
        }

        private static Text CreateText(Transform parent, string name, string text, Vector2 anchor, Vector2 position, Vector2 size, Font font, int fontSize, TextAnchor alignment)
        {
            GameObject textObject = new GameObject(name);
            textObject.transform.SetParent(parent);
            Text label = textObject.AddComponent<Text>();
            label.text = text;
            label.font = font;
            label.fontSize = fontSize;
            label.alignment = alignment;
            label.color = Color.white;
            label.raycastTarget = false;

            RectTransform rect = label.GetComponent<RectTransform>();
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
            return label;
        }

        private static void EnsureScenesFolder()
        {
            if (!AssetDatabase.IsValidFolder("Assets/_MomosDefense/Scenes"))
            {
                AssetDatabase.CreateFolder("Assets/_MomosDefense", "Scenes");
            }
        }

        private static void AddSceneIfExists(List<EditorBuildSettingsScene> scenes, string scenePath)
        {
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath) != null)
            {
                scenes.Add(new EditorBuildSettingsScene(scenePath, true));
            }
        }
    }
}
