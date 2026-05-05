using MomosDefense.Combat;
using MomosDefense.Core;
using MomosDefense.Enemies;
using MomosDefense.Towers;
using System;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace MomosDefense.Heroes
{
    public class MomoHeroController : MonoBehaviour
    {
        private const float ImmediateMoveMinimumDeltaTime = 1f / 60f;
        private const float HorizontalMovementThreshold = 0.01f;

        [Header("Identity")]
        [SerializeField] private string heroName = "Momo";
        [SerializeField] private GameObject selectionIndicator;
        [SerializeField] private HeroDefinition heroDefinition;
        [SerializeField] private SkillDefinition skillDefinition;
        [SerializeField] private Sprite portraitSprite;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float stoppingDistance = 0.05f;

        [Header("Movement Sprites")]
        [SerializeField] private SpriteRenderer movementSpriteRenderer;
        [SerializeField] private string walkLeftResourcePath;
        [SerializeField] private string walkRightResourcePath;
        [SerializeField] private float walkFramesPerSecond = 10f;

        [Header("Basic Attack")]
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float attacksPerSecond = 1f;
        [SerializeField] private int attackDamage = 2;

        [Header("Momo Pop")]
        [SerializeField] private float momoPopRadius = 3f;
        [SerializeField] private int momoPopDamage = 4;
        [SerializeField] private float momoPopCooldown = 8f;
        [SerializeField] private float momoPopSlowDuration = 2.5f;
        [SerializeField] private float momoPopSlowMultiplier = 0.45f;

        [Header("Progression")]
        [SerializeField] private int experience;

        private Vector3 destination;
        private float attackTimer;
        private float skillCooldownRemaining;
        private bool isSelected;
        private int level = 1;
        private int persistentSkillRank = 1;
        private int equipmentSkillDamageBonus;
        private Sprite[] walkLeftSprites = Array.Empty<Sprite>();
        private Sprite[] walkRightSprites = Array.Empty<Sprite>();
        private Sprite idleSprite;
        private Vector3 configuredSpriteScale = Vector3.one;
        private float idleSpriteUnitsHeight = 1f;
        private float walkAnimationTimer;
        private int walkFrameIndex;
        private FacingDirection facingDirection = FacingDirection.Right;

#if UNITY_EDITOR
        private static bool isSyncingSceneEditsToDefinition;
#endif

        private enum FacingDirection
        {
            Left,
            Right
        }

        public string HeroName => string.IsNullOrWhiteSpace(heroName) ? gameObject.name : heroName;
        public HeroDefinition HeroDefinition => heroDefinition;
        public SkillDefinition SkillDefinition => skillDefinition;
        public Sprite PortraitSprite => portraitSprite;
        public int Experience => experience;
        public int Level => level;
        public Vector3 CurrentDestination => destination;
        public float MomoPopCooldownRemaining => skillCooldownRemaining;
        public float MomoPopCooldown => skillDefinition != null ? skillDefinition.BaseCooldown : momoPopCooldown;
        public float SkillCooldownRemaining => skillCooldownRemaining;
        public bool CanUseMomoPop => skillCooldownRemaining <= 0f;
        public bool CanUseSkill => skillCooldownRemaining <= 0f;
        public string SkillName => skillDefinition != null ? skillDefinition.DisplayName : "Momo Pop";
        public bool IsSelected => isSelected;

        protected virtual void Awake()
        {
            InitializeMovementSprites();
            RefreshFromDefinition();
            destination = transform.position;
            UpdateSelectionVisual();
        }

        protected virtual void OnValidate()
        {
#if UNITY_EDITOR
            if (Application.isPlaying
                || heroDefinition == null
                || isSyncingSceneEditsToDefinition
                || EditorSceneManager.IsPreviewSceneObject(gameObject))
            {
                return;
            }

            if (movementSpriteRenderer == null)
            {
                movementSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            }

            SyncSceneEditsToHeroDefinition();
#endif
        }

        protected virtual void Update()
        {
            UpdateCooldowns();
            Move(Time.deltaTime);
            AttackNearestEnemy();

            if (isSelected && Input.GetKeyDown(KeyCode.Space))
            {
                TryUseSkill();
            }
        }

        public void SetMoveDestination(Vector3 newDestination)
        {
            destination = newDestination;
            destination.y = transform.position.y;
            Move(Mathf.Max(Time.deltaTime, Time.unscaledDeltaTime, ImmediateMoveMinimumDeltaTime));
        }

        private void Move(float deltaTime)
        {
            if (Vector3.Distance(transform.position, destination) <= stoppingDistance)
            {
                UpdateMovementSprite(Vector3.zero, false, deltaTime);
                return;
            }

            Vector3 previousPosition = transform.position;
            transform.position = Vector3.MoveTowards(previousPosition, destination, moveSpeed * deltaTime);
            Vector3 movementDelta = transform.position - previousPosition;
            UpdateMovementSprite(movementDelta, movementDelta.sqrMagnitude > 0.000001f, deltaTime);
        }

        private void AttackNearestEnemy()
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer > 0f)
            {
                return;
            }

            EnemyPathFollower nearestEnemy = EnemyRegistry.FindNearest(transform.position, attackRange);
            if (nearestEnemy == null || nearestEnemy.Health == null)
            {
                return;
            }

            nearestEnemy.Health.TakeDamage(attackDamage);
            attackTimer = 1f / attacksPerSecond;
        }

        public void TryUseSkill()
        {
            if (!CanUseSkill)
            {
                return;
            }

            if (skillDefinition == null)
            {
                UseFallbackMomoPop();
                return;
            }

            switch (skillDefinition.Behavior)
            {
                case SkillDefinition.SkillBehavior.TowerBuffArea:
                    ApplyTowerBuffSkill();
                    break;
                default:
                    ApplyDamageSlowSkill();
                    break;
            }

            skillCooldownRemaining = skillDefinition.BaseCooldown;
        }

        public void TryUseMomoPop()
        {
            TryUseSkill();
        }

        public void RefreshFromDefinition()
        {
            if (heroDefinition == null)
            {
                return;
            }

            heroName = string.IsNullOrWhiteSpace(heroDefinition.DisplayName) ? heroDefinition.HeroId : heroDefinition.DisplayName;
            moveSpeed = heroDefinition.MoveSpeed + Mathf.Max(0, level - 1) * heroDefinition.MoveSpeedPerLevel;
            attackRange = heroDefinition.AttackRange;
            attacksPerSecond = heroDefinition.AttacksPerSecond;
            attackDamage = heroDefinition.AttackDamage + Mathf.Max(0, level - 1) * heroDefinition.AttackDamagePerLevel;
            level = Mathf.Max(heroDefinition.StartingLevel, level);

            if (heroDefinition.WorldScale != Vector3.zero)
            {
                transform.localScale = heroDefinition.WorldScale;
            }

            ApplyConfiguredSpriteScale();
        }

        public void ApplyPersistentHeroLevel(int persistentLevel)
        {
            level = Mathf.Max(1, persistentLevel);
            RefreshFromDefinition();
        }

        public void ApplyPersistentSkillRank(int skillRank)
        {
            persistentSkillRank = Mathf.Max(1, skillRank);
        }

        public void ApplyEquipmentBonus(int heroSkillDamageBonus)
        {
            equipmentSkillDamageBonus = Mathf.Max(0, heroSkillDamageBonus);
        }

        public void SetSelected(bool selected)
        {
            if (isSelected == selected)
            {
                return;
            }

            isSelected = selected;
            UpdateSelectionVisual();
        }

        public void GainExperience(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            experience += amount;

            if (heroDefinition == null)
            {
                return;
            }

            int requiredExperience = Mathf.Max(1, heroDefinition.ExperienceToNextLevel);
            while (experience >= requiredExperience)
            {
                experience -= requiredExperience;
                level++;
                RefreshFromDefinition();
            }
        }

        private void UpdateCooldowns()
        {
            if (skillCooldownRemaining > 0f)
            {
                skillCooldownRemaining = Mathf.Max(0f, skillCooldownRemaining - Time.deltaTime);
            }
        }

        private void UpdateSelectionVisual()
        {
            if (selectionIndicator != null)
            {
                selectionIndicator.SetActive(isSelected);
            }
        }

        private void InitializeMovementSprites()
        {
            if (movementSpriteRenderer == null)
            {
                movementSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            }

            if (movementSpriteRenderer == null)
            {
                return;
            }

            idleSprite = movementSpriteRenderer.sprite != null ? movementSpriteRenderer.sprite : portraitSprite;
            idleSpriteUnitsHeight = GetSpriteUnitsHeight(idleSprite);

            ApplyConfiguredSpriteScale();

            string resourceFolder = ResolveHeroResourceFolder();
            string leftPath = string.IsNullOrWhiteSpace(walkLeftResourcePath)
                ? $"{resourceFolder}/{resourceFolder.ToLowerInvariant()}_walk_left"
                : walkLeftResourcePath;
            string rightPath = string.IsNullOrWhiteSpace(walkRightResourcePath)
                ? $"{resourceFolder}/{resourceFolder.ToLowerInvariant()}_walk_right"
                : walkRightResourcePath;

            walkLeftSprites = LoadOrderedSprites(leftPath);
            walkRightSprites = LoadOrderedSprites(rightPath);

            if (walkRightSprites.Length > 0)
            {
                facingDirection = FacingDirection.Right;
            }
            else if (walkLeftSprites.Length > 0)
            {
                facingDirection = FacingDirection.Left;
            }

            ApplyIdleSprite();
        }

        private void UpdateMovementSprite(Vector3 movementDelta, bool isMoving, float deltaTime)
        {
            if (movementSpriteRenderer == null)
            {
                return;
            }

            if (!isMoving)
            {
                walkAnimationTimer = 0f;
                walkFrameIndex = 0;
                ApplyIdleSprite();
                return;
            }

            if (movementDelta.x <= -HorizontalMovementThreshold)
            {
                facingDirection = FacingDirection.Left;
            }
            else if (movementDelta.x >= HorizontalMovementThreshold)
            {
                facingDirection = FacingDirection.Right;
            }

            Sprite[] activeSprites = facingDirection == FacingDirection.Left ? walkLeftSprites : walkRightSprites;
            if (activeSprites == null || activeSprites.Length == 0)
            {
                ApplyIdleSprite();
                return;
            }

            walkAnimationTimer += deltaTime * Mathf.Max(0.01f, walkFramesPerSecond);
            walkFrameIndex = Mathf.FloorToInt(walkAnimationTimer) % activeSprites.Length;
            ApplySprite(activeSprites[walkFrameIndex]);
        }

        private void ApplyIdleSprite()
        {
            if (movementSpriteRenderer == null)
            {
                return;
            }

            if (idleSprite != null)
            {
                ApplySprite(idleSprite);
                return;
            }

            Sprite[] facingSprites = facingDirection == FacingDirection.Left ? walkLeftSprites : walkRightSprites;
            if (facingSprites != null && facingSprites.Length > 0)
            {
                ApplySprite(facingSprites[0]);
            }
        }

        private void ApplySprite(Sprite sprite)
        {
            if (movementSpriteRenderer == null || sprite == null)
            {
                return;
            }

            movementSpriteRenderer.sprite = sprite;
            float spriteUnitsHeight = GetSpriteUnitsHeight(sprite);
            if (idleSpriteUnitsHeight > 0f && spriteUnitsHeight > 0f)
            {
                movementSpriteRenderer.transform.localScale = configuredSpriteScale * (idleSpriteUnitsHeight / spriteUnitsHeight);
                return;
            }

            movementSpriteRenderer.transform.localScale = configuredSpriteScale;
        }

        private void ApplyConfiguredSpriteScale()
        {
            if (movementSpriteRenderer == null)
            {
                return;
            }

            configuredSpriteScale = Vector3.one * 0.35f;
            if (heroDefinition != null && heroDefinition.SpriteScale != Vector3.zero)
            {
                configuredSpriteScale = heroDefinition.SpriteScale;
            }

            movementSpriteRenderer.transform.localScale = configuredSpriteScale;
        }

        private string ResolveHeroResourceFolder()
        {
            if (heroDefinition != null && !string.IsNullOrWhiteSpace(heroDefinition.DisplayName))
            {
                return heroDefinition.DisplayName.Replace(" ", string.Empty);
            }

            if (!string.IsNullOrWhiteSpace(heroName))
            {
                return heroName.Replace(" ", string.Empty);
            }

            return gameObject.name.Replace(" ", string.Empty);
        }

        private static Sprite[] LoadOrderedSprites(string resourcePath)
        {
            if (string.IsNullOrWhiteSpace(resourcePath))
            {
                return Array.Empty<Sprite>();
            }

            Sprite[] loadedSprites = Resources.LoadAll<Sprite>(resourcePath)
                .Where(sprite => sprite != null)
                .OrderBy(sprite => sprite.name, StringComparer.Ordinal)
                .ToArray();

            if (loadedSprites.Length > 0)
            {
                return loadedSprites;
            }

            Texture2D texture = Resources.Load<Texture2D>(resourcePath);
            if (texture == null)
            {
                return Array.Empty<Sprite>();
            }

            Sprite runtimeSprite = Sprite.Create(
                texture,
                new Rect(0f, 0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f),
                100f);
            runtimeSprite.name = texture.name;
            return new[] { runtimeSprite };
        }

        private static float GetSpriteUnitsHeight(Sprite sprite)
        {
            if (sprite == null || sprite.pixelsPerUnit <= 0f)
            {
                return 0f;
            }

            return sprite.rect.height / sprite.pixelsPerUnit;
        }

        private void UseFallbackMomoPop()
        {
            EnemyPathFollower[] enemies = FindObjectsByType<EnemyPathFollower>(FindObjectsInactive.Exclude);

            foreach (EnemyPathFollower enemy in enemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance > momoPopRadius)
                {
                    continue;
                }

                if (enemy.TryGetComponent(out Health health))
                {
                    health.TakeDamage(momoPopDamage);
                }

                enemy.ApplySlow(momoPopSlowDuration, momoPopSlowMultiplier);
            }

            skillCooldownRemaining = momoPopCooldown;
        }

        private void ApplyDamageSlowSkill()
        {
            EnemyPathFollower[] enemies = FindObjectsByType<EnemyPathFollower>(FindObjectsInactive.Exclude);
            float radius = skillDefinition.BaseRadius;
            int damage = skillDefinition.BaseDamage + GetSkillDamageBonusFromLevel();

            foreach (EnemyPathFollower enemy in enemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance > radius)
                {
                    continue;
                }

                if (enemy.TryGetComponent(out Health health))
                {
                    health.TakeDamage(damage);
                }

                enemy.ApplySlow(skillDefinition.SlowDuration, skillDefinition.SlowMultiplier);
            }
        }

        private void ApplyTowerBuffSkill()
        {
            TowerAttack[] towers = FindObjectsByType<TowerAttack>(FindObjectsInactive.Exclude);
            float radius = skillDefinition.BaseRadius;

            foreach (TowerAttack tower in towers)
            {
                if (tower == null || Vector3.Distance(transform.position, tower.transform.position) > radius)
                {
                    continue;
                }

                tower.ApplyTemporaryBoost(
                    skillDefinition.TowerBuffDuration,
                    skillDefinition.TowerBuffDamageBonus,
                    skillDefinition.TowerBuffAttackSpeedMultiplier);
            }
        }

        private int GetSkillDamageBonusFromLevel()
        {
            if (heroDefinition == null)
            {
                return equipmentSkillDamageBonus + Mathf.Max(0, persistentSkillRank - 1);
            }

            return Mathf.Max(0, level - 1) * heroDefinition.SkillDamagePerLevel
                + equipmentSkillDamageBonus
                + Mathf.Max(0, persistentSkillRank - 1);
        }

#if UNITY_EDITOR
        private void SyncSceneEditsToHeroDefinition()
        {
            SerializedObject serializedHeroDefinition = new SerializedObject(heroDefinition);
            bool changed = false;

            changed |= SetFloatIfDifferent(serializedHeroDefinition.FindProperty("moveSpeed"), moveSpeed);
            changed |= SetFloatIfDifferent(serializedHeroDefinition.FindProperty("attackRange"), attackRange);
            changed |= SetFloatIfDifferent(serializedHeroDefinition.FindProperty("attacksPerSecond"), attacksPerSecond);
            changed |= SetIntIfDifferent(serializedHeroDefinition.FindProperty("attackDamage"), attackDamage);
            changed |= SetVector3IfDifferent(serializedHeroDefinition.FindProperty("worldScale"), transform.localScale);

            Vector3 desiredSpriteScale = movementSpriteRenderer != null
                ? movementSpriteRenderer.transform.localScale
                : Vector3.zero;
            if (movementSpriteRenderer != null)
            {
                changed |= SetVector3IfDifferent(serializedHeroDefinition.FindProperty("spriteScale"), desiredSpriteScale);
            }

            if (!changed)
            {
                return;
            }

            isSyncingSceneEditsToDefinition = true;
            serializedHeroDefinition.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(heroDefinition);
            EditorApplication.delayCall += SaveDirtyHeroDefinitions;
            isSyncingSceneEditsToDefinition = false;
        }

        private static bool SetFloatIfDifferent(SerializedProperty property, float value)
        {
            if (property == null || Mathf.Approximately(property.floatValue, value))
            {
                return false;
            }

            property.floatValue = value;
            return true;
        }

        private static bool SetIntIfDifferent(SerializedProperty property, int value)
        {
            if (property == null || property.intValue == value)
            {
                return false;
            }

            property.intValue = value;
            return true;
        }

        private static bool SetVector3IfDifferent(SerializedProperty property, Vector3 value)
        {
            if (property == null || property.vector3Value == value)
            {
                return false;
            }

            property.vector3Value = value;
            return true;
        }

        private static void SaveDirtyHeroDefinitions()
        {
            AssetDatabase.SaveAssets();
        }
#endif
    }
}
