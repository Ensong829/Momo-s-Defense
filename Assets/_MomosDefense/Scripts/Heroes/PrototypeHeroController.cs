using MomosDefense.Audio;
using MomosDefense.Core;
using MomosDefense.Enemies;
using MomosDefense.Towers;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MomosDefense.Heroes
{
    [ExecuteAlways]
    public sealed class PrototypeHeroController : MonoBehaviour
    {
        [SerializeField] private HeroDefinition heroDefinition;
        [SerializeField] private SkillDefinition skillDefinition;
        [SerializeField] private string heroName = "Momo";
        [SerializeField] private string skillName = "Momo Pop";
        [SerializeField] private Camera worldCamera;
        [SerializeField] private GameObject selectionIndicator;
        [SerializeField] private LayerMask groundMask = ~0;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float attacksPerSecond = 1f;
        [SerializeField] private int attackDamage = 2;
        [SerializeField] private float skillRadius = 3f;
        [SerializeField] private int skillDamage = 4;
        [SerializeField] private float skillCooldown = 8f;
        [SerializeField] private float slowDuration = 2.5f;
        [SerializeField] private float slowMultiplier = 0.45f;
        [SerializeField] private float towerBuffDuration = 5f;
        [SerializeField] private int towerBuffDamageBonus = 1;
        [SerializeField] private float towerBuffAttackSpeedMultiplier = 1.6f;
        [SerializeField] private int experienceToNextLevel = 4;
        [SerializeField] private int attackDamagePerLevel = 1;
        [SerializeField] private int skillDamagePerLevel = 1;
        [SerializeField] private float moveSpeedPerLevel = 0.2f;

        private const string MomoHeroName = "Momo";
        private const string BulwarkHeroName = "Bulwark";
        private const string SproutHeroName = "Sprout";
        private const string MomoPortraitResourcePath = "Momo/momo";
        private const string MomoWalkDownResourcePath = "Momo/momo_walk_down";
        private const string MomoWalkLeftResourcePath = "Momo/momo_walk_left";
        private const string MomoWalkRightResourcePath = "Momo/momo_walk_right";
        private const string MomoWalkUpResourcePath = "Momo/momo_walk_up";
        private const string BulwarkPortraitResourcePath = "Bulwark/bulwark";
        private const string BulwarkWalkDownResourcePath = "Bulwark/bulwark_walk_down";
        private const string BulwarkWalkLeftResourcePath = "Bulwark/bulwark_walk_left";
        private const string BulwarkWalkRightResourcePath = "Bulwark/bulwark_walk_right";
        private const string BulwarkWalkUpResourcePath = "Bulwark/bulwark_walk_up";
        private const string MomoSpriteChildName = "Momo Sprite";
        private const string BulwarkSpriteChildName = "Bulwark Sprite";
        private const int MomoWalkDirections = 4;
        private const int MomoWalkFramesPerDirection = 6;
        private const float MomoWalkFrameRate = 10f;
        private const float MomoSpritePixelsPerUnit = 256f;
        private const float MomoWalkPivotY = 0.045f;
        private const int DefaultSpriteSortingOrder = 10;
        private const int SelectedSpriteSortingOrder = 1000;
        private const int SelectedMaterialRenderQueue = 5000;

        private Vector3 destination;
        private float attackTimer;
        private float skillCooldownRemaining;
        private bool isSelected;
        private int currentExperience;
        private int level = 1;
        private int appliedPersistentHeroLevel = 1;
        private int appliedPersistentSkillRank = 1;
        private int appliedEquipmentSkillDamageBonus;
        [SerializeField] private Vector3 momoSpriteScale = new Vector3(2f, 2f, 2f);
        private Sprite portraitSprite;
        private SpriteRenderer momoSpriteRenderer;
        private Sprite[][] momoWalkSprites;
        private MomoFacingDirection momoFacingDirection = MomoFacingDirection.Down;
        private Material[] heroMaterials = Array.Empty<Material>();
        private int[] heroMaterialDefaultQueues = Array.Empty<int>();

        public HeroDefinition HeroDefinition => heroDefinition;
        public SkillDefinition SkillDefinition => skillDefinition;
        public string HeroName => heroName;
        public string SkillName => skillName;
        public float SkillCooldownRemaining => skillCooldownRemaining;
        public bool CanUseSkill => skillCooldownRemaining <= 0f;
        public bool IsSelected => isSelected;
        public int Level => level;
        public int CurrentExperience => currentExperience;
        public int ExperienceToNextLevel => experienceToNextLevel;
        public int SkillDamage => skillDamage;
        public float SkillRadius => skillRadius;
        public Sprite PortraitSprite => portraitSprite;

        private enum MomoFacingDirection
        {
            Down = 0,
            Left = 1,
            Right = 2,
            Up = 3
        }

        private void Awake()
        {
            ApplyDefinitions();
            destination = transform.position;
            worldCamera = worldCamera == null ? Camera.main : worldCamera;
            TryInitializeMomoSpritePresentation();
            CacheRenderPriorityState();
            ApplyRenderPriority();
            UpdateSelectionVisual();
        }

        private void OnEnable()
        {
            if (!Application.isPlaying)
            {
                RefreshEditorPreview();
            }
        }

        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                RefreshEditorPreview();
            }
        }

        private void Update()
        {
            if (!Application.isPlaying)
            {
                RefreshEditorPreview();
                return;
            }

            UpdateCooldowns();
            Move();
            AttackNearestEnemy();
            UpdateMomoSpritePresentation();

            if (isSelected && Input.GetKeyDown(KeyCode.Space))
            {
                TryUseSkill();
            }
        }

        private void OnMouseDown()
        {
            HeroSelectionManager selectionManager = FindFirstObjectByType<HeroSelectionManager>();
            selectionManager?.SelectHero(this);
        }

        public void SetSelected(bool selected)
        {
            if (isSelected == selected)
            {
                return;
            }

            isSelected = selected;
            ApplyRenderPriority();
            UpdateSelectionVisual();
        }

        public bool TryMoveToPointer()
        {
            if (worldCamera == null || IsPointerOverUi())
            {
                return false;
            }

            Ray ray = worldCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, 100f)
                .OrderBy(hit => hit.distance)
                .ToArray();

            for (int hitIndex = 0; hitIndex < hits.Length; hitIndex++)
            {
                RaycastHit hit = hits[hitIndex];
                if (hit.collider == null)
                {
                    continue;
                }

                if (hit.collider.GetComponentInParent<PrototypeHeroController>() != null)
                {
                    return false;
                }

                if (hit.collider.GetComponentInParent<TowerBuildNode>() != null
                    || hit.collider.GetComponentInParent<TowerAttack>() != null)
                {
                    return false;
                }

                if (((1 << hit.collider.gameObject.layer) & groundMask.value) == 0)
                {
                    continue;
                }

                destination = hit.point;
                destination.y = transform.position.y;
                return true;
            }

            return false;
        }

        public void TryUseSkill()
        {
            if (!CanUseSkill)
            {
                return;
            }

            if (skillDefinition != null && skillDefinition.Behavior == SkillDefinition.SkillBehavior.TowerBuffArea)
            {
                UseTowerBloom();
            }
            else
            {
                UseAreaDamageSkill();
            }

            PrototypeAudioDirector.PlaySkill(GetSkillPitch());
            skillCooldownRemaining = skillCooldown;
        }

        public void GainExperience(int experience)
        {
            if (experience <= 0)
            {
                return;
            }

            currentExperience += experience;

            while (currentExperience >= experienceToNextLevel)
            {
                currentExperience -= experienceToNextLevel;
                LevelUp();
            }
        }

        public void ApplyPersistentSkillRank(int skillRank)
        {
            skillRank = Mathf.Max(1, skillRank);
            if (skillRank <= appliedPersistentSkillRank)
            {
                return;
            }

            int bonusRanks = skillRank - appliedPersistentSkillRank;
            skillDamage += bonusRanks * 2;
            skillRadius += bonusRanks * 0.25f;
            skillCooldown = Mathf.Max(4f, skillCooldown - (bonusRanks * 0.35f));
            appliedPersistentSkillRank = skillRank;
        }

        public void ApplyPersistentHeroLevel(int heroLevel)
        {
            heroLevel = Mathf.Max(1, heroLevel);
            if (heroLevel <= appliedPersistentHeroLevel)
            {
                return;
            }

            int bonusLevels = heroLevel - appliedPersistentHeroLevel;
            attackDamage += bonusLevels;
            moveSpeed += bonusLevels * 0.08f;
            appliedPersistentHeroLevel = heroLevel;
        }

        public void ApplyEquipmentBonus(int skillDamageBonus)
        {
            int clampedBonus = Mathf.Max(0, skillDamageBonus);
            skillDamage += clampedBonus - appliedEquipmentSkillDamageBonus;
            appliedEquipmentSkillDamageBonus = clampedBonus;
        }

        private void Move()
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
        }

        private void AttackNearestEnemy()
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer > 0f)
            {
                return;
            }

            EnemyPathFollower nearest = EnemyRegistry.FindNearest(transform.position, attackRange);
            if (nearest == null || nearest.Health == null)
            {
                return;
            }

            nearest.Health.TakeDamage(attackDamage);
            attackTimer = 1f / attacksPerSecond;
        }

        private void UseAreaDamageSkill()
        {
            for (int index = 0; index < EnemyRegistry.ActiveEnemies.Count; index++)
            {
                EnemyPathFollower enemy = EnemyRegistry.ActiveEnemies[index];
                if (enemy == null || enemy.Health == null || !enemy.Health.IsAlive)
                {
                    continue;
                }

                if (Vector3.Distance(transform.position, enemy.transform.position) > skillRadius)
                {
                    continue;
                }

                enemy.Health.TakeDamage(skillDamage);
                enemy.ApplySlow(slowDuration, slowMultiplier);
            }
        }

        private void UseTowerBloom()
        {
            TowerAttack[] towers = FindObjectsByType<TowerAttack>(FindObjectsInactive.Exclude);

            foreach (TowerAttack tower in towers)
            {
                if (Vector3.Distance(transform.position, tower.transform.position) > skillRadius)
                {
                    continue;
                }

                tower.ApplyTemporaryBoost(towerBuffDuration, towerBuffDamageBonus, towerBuffAttackSpeedMultiplier);
            }
        }

        private void UpdateCooldowns()
        {
            if (skillCooldownRemaining > 0f)
            {
                skillCooldownRemaining = Mathf.Max(0f, skillCooldownRemaining - Time.deltaTime);
            }
        }

        private void LevelUp()
        {
            level++;
            attackDamage += attackDamagePerLevel;
            skillDamage += skillDamagePerLevel;
            moveSpeed += moveSpeedPerLevel;
            experienceToNextLevel += 2;
        }

        private float GetSkillPitch()
        {
            return heroName switch
            {
                "Momo" => 1.1f,
                "Bulwark" => 0.82f,
                "Sprout" => 1.22f,
                _ => 1f
            };
        }

        private void ApplyDefinitions()
        {
            if (heroDefinition != null)
            {
                heroName = heroDefinition.DisplayName;
                moveSpeed = heroDefinition.MoveSpeed;
                attackRange = heroDefinition.AttackRange;
                attacksPerSecond = heroDefinition.AttacksPerSecond;
                attackDamage = heroDefinition.AttackDamage;
                experienceToNextLevel = heroDefinition.ExperienceToNextLevel;
                attackDamagePerLevel = heroDefinition.AttackDamagePerLevel;
                skillDamagePerLevel = heroDefinition.SkillDamagePerLevel;
                moveSpeedPerLevel = heroDefinition.MoveSpeedPerLevel;
            }

            if (skillDefinition != null)
            {
                skillName = skillDefinition.DisplayName;
                skillDamage = skillDefinition.BaseDamage;
                skillRadius = skillDefinition.BaseRadius;
                skillCooldown = skillDefinition.BaseCooldown;
                slowDuration = skillDefinition.SlowDuration;
                slowMultiplier = skillDefinition.SlowMultiplier;
                towerBuffDuration = skillDefinition.TowerBuffDuration;
                towerBuffDamageBonus = skillDefinition.TowerBuffDamageBonus;
                towerBuffAttackSpeedMultiplier = skillDefinition.TowerBuffAttackSpeedMultiplier;
            }
        }

        private void UpdateSelectionVisual()
        {
            if (selectionIndicator != null)
            {
                selectionIndicator.SetActive(isSelected);
            }
        }

        private void TryInitializeMomoSpritePresentation()
        {
            if (!SupportsSpritePresentation())
            {
                ResetToMeshPresentation();
                return;
            }

            portraitSprite = LoadPortraitSprite();
            if (portraitSprite == null)
            {
                ResetToMeshPresentation();
                return;
            }

            momoWalkSprites = LoadWalkSpritesFromImports();
            if (momoWalkSprites == null || momoWalkSprites.Length != MomoWalkDirections)
            {
                ResetToMeshPresentation();
                return;
            }

            for (int rendererIndex = 0; rendererIndex < transform.childCount; rendererIndex++)
            {
                Transform child = transform.GetChild(rendererIndex);
                if (child.name == GetSpriteChildName())
                {
                    momoSpriteRenderer = child.GetComponent<SpriteRenderer>();
                    break;
                }
            }

            SetNonSpriteRenderersEnabled(false);

            if (momoSpriteRenderer == null)
            {
                GameObject spriteObject = new GameObject(GetSpriteChildName());
                spriteObject.transform.SetParent(transform, false);
                spriteObject.transform.localPosition = new Vector3(0f, 0.04f, 0f);
                momoSpriteRenderer = spriteObject.AddComponent<SpriteRenderer>();
            }

            momoSpriteRenderer.transform.localScale = GetMomoSpriteScale();
            momoSpriteRenderer.sprite = momoWalkSprites[(int)MomoFacingDirection.Down][0];
            momoSpriteRenderer.sortingOrder = DefaultSpriteSortingOrder;
        }

        private void UpdateMomoSpritePresentation()
        {
            if (momoSpriteRenderer == null || momoWalkSprites == null)
            {
                return;
            }

            if (worldCamera == null)
            {
                worldCamera = Camera.main;
            }

            if (worldCamera != null)
            {
                momoSpriteRenderer.transform.rotation = Quaternion.LookRotation(worldCamera.transform.forward, Vector3.up);
            }

            Vector3 movement = destination - transform.position;
            movement.y = 0f;
            bool isMoving = movement.sqrMagnitude > 0.0025f;
            if (isMoving)
            {
                momoFacingDirection = ResolveFacingDirection(movement);
            }

            int frameIndex = isMoving
                ? Mathf.FloorToInt(Time.time * MomoWalkFrameRate) % MomoWalkFramesPerDirection
                : 0;
            momoSpriteRenderer.sprite = momoWalkSprites[(int)momoFacingDirection][frameIndex];
        }

        private static MomoFacingDirection ResolveFacingDirection(Vector3 movement)
        {
            if (Mathf.Abs(movement.x) >= Mathf.Abs(movement.z))
            {
                return movement.x >= 0f ? MomoFacingDirection.Right : MomoFacingDirection.Left;
            }

            return movement.z >= 0f ? MomoFacingDirection.Up : MomoFacingDirection.Down;
        }

        private Sprite[][] LoadWalkSpritesFromImports()
        {
            Sprite[][] sprites = new Sprite[MomoWalkDirections][];
            sprites[(int)MomoFacingDirection.Down] = LoadWalkStripSprites(GetWalkDownResourcePath(), GetWalkDownFileName());
            sprites[(int)MomoFacingDirection.Left] = LoadWalkStripSprites(GetWalkLeftResourcePath(), GetWalkLeftFileName());
            sprites[(int)MomoFacingDirection.Right] = LoadWalkStripSprites(GetWalkRightResourcePath(), GetWalkRightFileName());
            sprites[(int)MomoFacingDirection.Up] = LoadWalkStripSprites(GetWalkUpResourcePath(), GetWalkUpFileName());

            for (int directionIndex = 0; directionIndex < sprites.Length; directionIndex++)
            {
                if (sprites[directionIndex] == null || sprites[directionIndex].Length != MomoWalkFramesPerDirection)
                {
                    return null;
                }
            }

            return sprites;
        }

        private Sprite LoadPortraitSprite()
        {
            Sprite importedPortrait = Resources.Load<Sprite>(GetPortraitResourcePath());
            if (importedPortrait != null)
            {
                return importedPortrait;
            }

            Texture2D portraitTexture = LoadHeroTexture(GetPortraitResourcePath(), GetPortraitFileName());
            if (portraitTexture == null)
            {
                return null;
            }

            return CreateSprite(
                portraitTexture,
                new Rect(0f, 0f, portraitTexture.width, portraitTexture.height),
                new Vector2(0.5f, 0.5f));
        }

        private Sprite[] LoadWalkStripSprites(string resourcePath, string fileName)
        {
            Sprite[] importedSprites = Resources.LoadAll<Sprite>(resourcePath);
            if (importedSprites != null && importedSprites.Length >= MomoWalkFramesPerDirection)
            {
                return importedSprites
                    .OrderBy(sprite => sprite.name, StringComparer.Ordinal)
                    .Take(MomoWalkFramesPerDirection)
                    .ToArray();
            }

            Texture2D texture = LoadHeroTexture(resourcePath, fileName);
            return CreateWalkStripSprites(texture);
        }

        private static Sprite[] CreateWalkStripSprites(Texture2D texture)
        {
            if (texture == null)
            {
                return null;
            }

            int frameWidth = texture.width / MomoWalkFramesPerDirection;
            int frameHeight = texture.height;
            if (frameWidth <= 0 || frameHeight <= 0)
            {
                return null;
            }

            Sprite[] sprites = new Sprite[MomoWalkFramesPerDirection];
            for (int frameIndex = 0; frameIndex < MomoWalkFramesPerDirection; frameIndex++)
            {
                float x = frameIndex * frameWidth;
                sprites[frameIndex] = CreateSprite(
                    texture,
                    new Rect(x, 0f, frameWidth, frameHeight),
                    new Vector2(0.5f, MomoWalkPivotY));
            }

            return sprites;
        }

        private static Sprite CreateSprite(Texture2D texture, Rect rect, Vector2 pivot)
        {
            return Sprite.Create(texture, rect, pivot, MomoSpritePixelsPerUnit, 0u, SpriteMeshType.Tight);
        }

        private Texture2D LoadHeroTexture(string resourcePath, string fileName)
        {
            Texture2D resourceTexture = Resources.Load<Texture2D>(resourcePath);
            if (resourceTexture != null)
            {
                return resourceTexture;
            }

            string projectTexturePath = Path.Combine(Application.dataPath, "Resources", GetHeroResourceFolderName(), fileName);
            if (!File.Exists(projectTexturePath))
            {
                return null;
            }

            byte[] bytes = File.ReadAllBytes(projectTexturePath);
            Texture2D fileTexture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            if (!fileTexture.LoadImage(bytes))
            {
                UnityEngine.Object.Destroy(fileTexture);
                return null;
            }

            fileTexture.name = Path.GetFileNameWithoutExtension(fileName);
            return fileTexture;
        }

        private void SetNonSpriteRenderersEnabled(bool enabled)
        {
            Renderer[] renderers = GetComponentsInChildren<Renderer>(true);
            foreach (Renderer renderer in renderers)
            {
                if (renderer is SpriteRenderer)
                {
                    continue;
                }

                renderer.enabled = enabled;
            }
        }

        private void ResetToMeshPresentation()
        {
            portraitSprite = null;
            momoWalkSprites = null;
            SetHeroSpriteRenderersEnabled(false);
            SetNonSpriteRenderersEnabled(true);
            momoSpriteRenderer = null;
        }

        private void SetHeroSpriteRenderersEnabled(bool enabled)
        {
            SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                spriteRenderer.enabled = enabled;
            }
        }

        private void CacheRenderPriorityState()
        {
            if (!Application.isPlaying)
            {
                heroMaterials = Array.Empty<Material>();
                heroMaterialDefaultQueues = Array.Empty<int>();
                return;
            }

            List<Material> materials = new List<Material>();
            Renderer[] renderers = GetComponentsInChildren<Renderer>(true);
            foreach (Renderer renderer in renderers)
            {
                if (renderer is SpriteRenderer spriteRenderer && spriteRenderer == momoSpriteRenderer)
                {
                    continue;
                }

                Material[] rendererMaterials = renderer.materials;
                for (int materialIndex = 0; materialIndex < rendererMaterials.Length; materialIndex++)
                {
                    Material material = rendererMaterials[materialIndex];
                    if (material != null)
                    {
                        materials.Add(material);
                    }
                }
            }

            heroMaterials = materials.ToArray();
            heroMaterialDefaultQueues = new int[heroMaterials.Length];
            for (int materialIndex = 0; materialIndex < heroMaterials.Length; materialIndex++)
            {
                heroMaterialDefaultQueues[materialIndex] = heroMaterials[materialIndex].renderQueue;
            }
        }

        private void ApplyRenderPriority()
        {
            if (!Application.isPlaying)
            {
                if (momoSpriteRenderer != null)
                {
                    momoSpriteRenderer.sortingOrder = isSelected
                        ? SelectedSpriteSortingOrder
                        : DefaultSpriteSortingOrder;
                }

                return;
            }

            if (momoSpriteRenderer != null)
            {
                momoSpriteRenderer.sortingOrder = isSelected
                    ? SelectedSpriteSortingOrder
                    : DefaultSpriteSortingOrder;
            }

            for (int materialIndex = 0; materialIndex < heroMaterials.Length; materialIndex++)
            {
                heroMaterials[materialIndex].renderQueue = isSelected
                    ? SelectedMaterialRenderQueue
                    : heroMaterialDefaultQueues[materialIndex];
            }
        }

        private void RefreshEditorPreview()
        {
            ApplyDefinitions();
            destination = transform.position;
            worldCamera = worldCamera == null ? Camera.main : worldCamera;
            TryInitializeMomoSpritePresentation();
            ApplyRenderPriority();
            UpdateMomoSpritePresentation();
            UpdateSelectionVisual();
        }

        private Vector3 GetMomoSpriteScale()
        {
            return momoSpriteScale == Vector3.zero
                ? new Vector3(2f, 2f, 2f)
                : momoSpriteScale;
        }

        private bool SupportsSpritePresentation()
        {
            return string.Equals(heroName, MomoHeroName, StringComparison.OrdinalIgnoreCase)
                || string.Equals(heroName, BulwarkHeroName, StringComparison.OrdinalIgnoreCase)
                || string.Equals(heroName, SproutHeroName, StringComparison.OrdinalIgnoreCase) && HasSpriteResourcesForHero();
        }

        private string GetHeroResourceFolderName()
        {
            if (string.Equals(heroName, BulwarkHeroName, StringComparison.OrdinalIgnoreCase))
            {
                return BulwarkHeroName;
            }

            if (string.Equals(heroName, SproutHeroName, StringComparison.OrdinalIgnoreCase))
            {
                return SproutHeroName;
            }

            return MomoHeroName;
        }

        private string GetPortraitResourcePath()
        {
            if (string.Equals(heroName, BulwarkHeroName, StringComparison.OrdinalIgnoreCase))
            {
                return BulwarkPortraitResourcePath;
            }

            if (string.Equals(heroName, SproutHeroName, StringComparison.OrdinalIgnoreCase))
            {
                return $"{SproutHeroName}/{char.ToLowerInvariant(SproutHeroName[0])}{SproutHeroName.Substring(1)}";
            }

            return MomoPortraitResourcePath;
        }

        private string GetWalkDownResourcePath()
        {
            if (string.Equals(heroName, BulwarkHeroName, StringComparison.OrdinalIgnoreCase))
            {
                return BulwarkWalkDownResourcePath;
            }

            if (string.Equals(heroName, SproutHeroName, StringComparison.OrdinalIgnoreCase))
            {
                return $"{SproutHeroName}/sprout_walk_down";
            }

            return MomoWalkDownResourcePath;
        }

        private string GetWalkLeftResourcePath()
        {
            if (string.Equals(heroName, BulwarkHeroName, StringComparison.OrdinalIgnoreCase))
            {
                return BulwarkWalkLeftResourcePath;
            }

            if (string.Equals(heroName, SproutHeroName, StringComparison.OrdinalIgnoreCase))
            {
                return $"{SproutHeroName}/sprout_walk_left";
            }

            return MomoWalkLeftResourcePath;
        }

        private string GetWalkRightResourcePath()
        {
            if (string.Equals(heroName, BulwarkHeroName, StringComparison.OrdinalIgnoreCase))
            {
                return BulwarkWalkRightResourcePath;
            }

            if (string.Equals(heroName, SproutHeroName, StringComparison.OrdinalIgnoreCase))
            {
                return $"{SproutHeroName}/sprout_walk_right";
            }

            return MomoWalkRightResourcePath;
        }

        private string GetWalkUpResourcePath()
        {
            if (string.Equals(heroName, BulwarkHeroName, StringComparison.OrdinalIgnoreCase))
            {
                return BulwarkWalkUpResourcePath;
            }

            if (string.Equals(heroName, SproutHeroName, StringComparison.OrdinalIgnoreCase))
            {
                return $"{SproutHeroName}/sprout_walk_up";
            }

            return MomoWalkUpResourcePath;
        }

        private string GetPortraitFileName()
        {
            if (string.Equals(heroName, BulwarkHeroName, StringComparison.OrdinalIgnoreCase))
            {
                return "bulwark.png";
            }

            if (string.Equals(heroName, SproutHeroName, StringComparison.OrdinalIgnoreCase))
            {
                return "sprout.png";
            }

            return "momo.png";
        }

        private string GetWalkDownFileName()
        {
            if (string.Equals(heroName, BulwarkHeroName, StringComparison.OrdinalIgnoreCase))
            {
                return "bulwark_walk_down.png";
            }

            if (string.Equals(heroName, SproutHeroName, StringComparison.OrdinalIgnoreCase))
            {
                return "sprout_walk_down.png";
            }

            return "momo_walk_down.png";
        }

        private string GetWalkLeftFileName()
        {
            if (string.Equals(heroName, BulwarkHeroName, StringComparison.OrdinalIgnoreCase))
            {
                return "bulwark_walk_left.png";
            }

            if (string.Equals(heroName, SproutHeroName, StringComparison.OrdinalIgnoreCase))
            {
                return "sprout_walk_left.png";
            }

            return "momo_walk_left.png";
        }

        private string GetWalkRightFileName()
        {
            if (string.Equals(heroName, BulwarkHeroName, StringComparison.OrdinalIgnoreCase))
            {
                return "bulwark_walk_right.png";
            }

            if (string.Equals(heroName, SproutHeroName, StringComparison.OrdinalIgnoreCase))
            {
                return "sprout_walk_right.png";
            }

            return "momo_walk_right.png";
        }

        private string GetWalkUpFileName()
        {
            if (string.Equals(heroName, BulwarkHeroName, StringComparison.OrdinalIgnoreCase))
            {
                return "bulwark_walk_up.png";
            }

            if (string.Equals(heroName, SproutHeroName, StringComparison.OrdinalIgnoreCase))
            {
                return "sprout_walk_up.png";
            }

            return "momo_walk_up.png";
        }

        private string GetSpriteChildName()
        {
            if (string.Equals(heroName, BulwarkHeroName, StringComparison.OrdinalIgnoreCase))
            {
                return BulwarkSpriteChildName;
            }

            if (string.Equals(heroName, SproutHeroName, StringComparison.OrdinalIgnoreCase))
            {
                return "Sprout Sprite";
            }

            return MomoSpriteChildName;
        }

        private bool HasSpriteResourcesForHero()
        {
            return LoadHeroTexture(GetPortraitResourcePath(), GetPortraitFileName()) != null
                && LoadHeroTexture(GetWalkDownResourcePath(), GetWalkDownFileName()) != null
                && LoadHeroTexture(GetWalkLeftResourcePath(), GetWalkLeftFileName()) != null
                && LoadHeroTexture(GetWalkRightResourcePath(), GetWalkRightFileName()) != null
                && LoadHeroTexture(GetWalkUpResourcePath(), GetWalkUpFileName()) != null;
        }

        private static bool IsPointerOverUi()
        {
            if (EventSystem.current == null)
            {
                return false;
            }

            if (EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }

            for (int touchIndex = 0; touchIndex < Input.touchCount; touchIndex++)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(touchIndex).fingerId))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
