using MomosDefense.Audio;
using MomosDefense.Combat;
using MomosDefense.Enemies;
using MomosDefense.Towers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MomosDefense.Heroes
{
    public sealed class PrototypeHeroController : MonoBehaviour
    {
        public enum HeroSkillType
        {
            MomoPop = 0,
            GroundSlam = 1,
            TowerBloom = 2
        }

        [SerializeField] private string heroName = "Momo";
        [SerializeField] private string skillName = "Momo Pop";
        [SerializeField] private HeroSkillType skillType = HeroSkillType.MomoPop;
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

        private Vector3 destination;
        private float attackTimer;
        private float skillCooldownRemaining;
        private bool isSelected;
        private int currentExperience;
        private int level = 1;
        private int appliedPersistentSkillRank = 1;

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

        private void Awake()
        {
            destination = transform.position;
            worldCamera = worldCamera == null ? Camera.main : worldCamera;
            UpdateSelectionVisual();
        }

        private void Update()
        {
            UpdateCooldowns();
            ReadMoveInput();
            Move();
            AttackNearestEnemy();

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
            UpdateSelectionVisual();
        }

        public void TryUseSkill()
        {
            if (!CanUseSkill)
            {
                return;
            }

            switch (skillType)
            {
                case HeroSkillType.MomoPop:
                    UseMomoPop();
                    break;
                case HeroSkillType.GroundSlam:
                    UseGroundSlam();
                    break;
                case HeroSkillType.TowerBloom:
                    UseTowerBloom();
                    break;
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

        private void ReadMoveInput()
        {
            if (!isSelected || worldCamera == null || !Input.GetMouseButtonDown(0) || IsPointerOverUi())
            {
                return;
            }

            Ray ray = worldCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundMask))
            {
                if (hit.collider.TryGetComponent(out TowerBuildNode _))
                {
                    return;
                }

                destination = hit.point;
                destination.y = transform.position.y;
            }
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

            Health nearest = null;
            float nearestDistance = float.MaxValue;
            Health[] healthTargets = FindObjectsByType<Health>(FindObjectsInactive.Exclude);

            foreach (Health target in healthTargets)
            {
                if (target.gameObject == gameObject || !target.TryGetComponent(out EnemyPathFollower _))
                {
                    continue;
                }

                float distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance <= attackRange && distance < nearestDistance)
                {
                    nearest = target;
                    nearestDistance = distance;
                }
            }

            if (nearest == null)
            {
                return;
            }

            nearest.TakeDamage(attackDamage);
            attackTimer = 1f / attacksPerSecond;
        }

        private void UseMomoPop()
        {
            EnemyPathFollower[] enemies = FindObjectsByType<EnemyPathFollower>(FindObjectsInactive.Exclude);

            foreach (EnemyPathFollower enemy in enemies)
            {
                if (Vector3.Distance(transform.position, enemy.transform.position) > skillRadius)
                {
                    continue;
                }

                if (enemy.TryGetComponent(out Health health))
                {
                    health.TakeDamage(skillDamage);
                }

                enemy.ApplySlow(slowDuration, slowMultiplier);
            }
        }

        private void UseGroundSlam()
        {
            EnemyPathFollower[] enemies = FindObjectsByType<EnemyPathFollower>(FindObjectsInactive.Exclude);

            foreach (EnemyPathFollower enemy in enemies)
            {
                if (Vector3.Distance(transform.position, enemy.transform.position) > skillRadius)
                {
                    continue;
                }

                if (enemy.TryGetComponent(out Health health))
                {
                    health.TakeDamage(skillDamage);
                }

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
            return skillType switch
            {
                HeroSkillType.MomoPop => 1.1f,
                HeroSkillType.GroundSlam => 0.82f,
                HeroSkillType.TowerBloom => 1.22f,
                _ => 1f
            };
        }

        private void UpdateSelectionVisual()
        {
            if (selectionIndicator != null)
            {
                selectionIndicator.SetActive(isSelected);
            }
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
