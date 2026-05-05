using MomosDefense.Heroes;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MomosDefense.Editor
{
    public static class HeroInputValidator
    {
        [MenuItem("Momo's Defense/Debug/Validate Hero Input")]
        public static void ValidateHeroInput()
        {
            GameObject managerObject = new GameObject("Hero Input Validator");
            GameObject momoObject = new GameObject("Momo");
            GameObject bulwarkObject = new GameObject("Bulwark");
            GameObject sproutObject = new GameObject("Sprout");

            try
            {
                HeroSelectionManager manager = managerObject.AddComponent<HeroSelectionManager>();
                MomoHeroController momo = momoObject.AddComponent<MomoHeroController>();
                MomoHeroController bulwark = bulwarkObject.AddComponent<MomoHeroController>();
                MomoHeroController sprout = sproutObject.AddComponent<MomoHeroController>();

                SetSerializedString(momo, "heroName", "Momo");
                SetSerializedString(bulwark, "heroName", "Bulwark");
                SetSerializedString(sprout, "heroName", "Sprout");

                SetSerializedHeroes(manager, momo, bulwark, sprout);
                manager.SelectHero(momo);

                Vector3 firstMovePoint = new Vector3(2f, 0f, 3f);
                bool sameHeroMoveHandled = manager.HandleResolvedHits(
                    new[]
                    {
                        new HeroSelectionManager.PointerHit(momo, false, false, Vector3.zero, "Momo Capsule", 0),
                        new HeroSelectionManager.PointerHit(null, false, true, firstMovePoint, "Ground", 0)
                    },
                    false);
                AssertCondition(sameHeroMoveHandled, "Selected hero move should be handled even when their collider is hit first.");
                AssertCondition(manager.SelectedHero == momo, "Selected hero should remain Momo after clicking Momo.");
                AssertApproximatelyEqual(momo.CurrentDestination, new Vector3(firstMovePoint.x, momo.transform.position.y, firstMovePoint.z), "Momo should receive the move destination behind their own collider.");

                Vector3 momoDestinationBeforeSwitch = momo.CurrentDestination;
                bool switchHandled = manager.HandleResolvedHits(
                    new[]
                    {
                        new HeroSelectionManager.PointerHit(bulwark, false, false, Vector3.zero, "Bulwark Capsule", 0)
                    },
                    false);
                AssertCondition(!switchHandled, "Clicking a hero in the world should not be treated as a selection command.");
                AssertCondition(manager.SelectedHero == momo, "Selected hero should stay Momo after clicking Bulwark in the world.");
                AssertApproximatelyEqual(momo.CurrentDestination, momoDestinationBeforeSwitch, "World hero clicks should not move the selected hero on their own.");

                manager.SelectHero(bulwark);
                AssertCondition(manager.SelectedHero == bulwark, "Shortcut or HUD-style hero selection should switch to Bulwark immediately.");

                Vector3 secondMovePoint = new Vector3(-4f, 0f, 1.5f);
                bool switchedHeroMoveHandled = manager.HandleResolvedHits(
                    new[]
                    {
                        new HeroSelectionManager.PointerHit(bulwark, false, false, Vector3.zero, "Bulwark Capsule", 0),
                        new HeroSelectionManager.PointerHit(null, false, true, secondMovePoint, "Ground", 0)
                    },
                    false);
                AssertCondition(switchedHeroMoveHandled, "The newly selected hero should move on the next ground click with no extra selection click.");
                AssertCondition(manager.SelectedHero == bulwark, "Bulwark should stay selected for the move.");
                AssertApproximatelyEqual(bulwark.CurrentDestination, new Vector3(secondMovePoint.x, bulwark.transform.position.y, secondMovePoint.z), "Bulwark should receive the new move destination.");
                AssertCondition(bulwark.transform.position.sqrMagnitude > 0.0001f, "Bulwark should take a visible movement step immediately when the move command is assigned.");

                manager.SelectHero(sprout);
                Vector3 thirdMovePoint = new Vector3(5f, 0f, -2f);
                bool thirdHeroMoveHandled = manager.HandleResolvedHits(
                    new[]
                    {
                        new HeroSelectionManager.PointerHit(null, false, true, thirdMovePoint, "Ground", 0)
                    },
                    false);
                AssertCondition(thirdHeroMoveHandled, "The third hero should accept a move command immediately after selection.");
                AssertCondition(manager.SelectedHero == sprout, "Sprout should remain selected for the move.");
                AssertApproximatelyEqual(sprout.CurrentDestination, new Vector3(thirdMovePoint.x, sprout.transform.position.y, thirdMovePoint.z), "Sprout should receive the move destination immediately.");

                CapsuleCollider colliderInFrontOfGround = bulwarkObject.AddComponent<CapsuleCollider>();
                colliderInFrontOfGround.radius = 0.5f;
                colliderInFrontOfGround.height = 2f;
                Ray rayThroughHeroCollider = new Ray(new Vector3(0f, 10f, -10f), new Vector3(0f, -1f, 1f).normalized);
                AssertCondition(TryResolveMovePoint(manager, rayThroughHeroCollider, sprout.transform.position.y, out Vector3 planeMovePoint), "Runtime move point should resolve from the movement plane.");
                AssertApproximatelyEqual(planeMovePoint, new Vector3(0f, sprout.transform.position.y, 0f), "Runtime move targeting should ignore hero colliders and use the battlefield plane.");

                Debug.Log("[HeroInputValidator] PASS: three-hero selection and move routing behaved as expected.");
            }
            finally
            {
                Object.DestroyImmediate(managerObject);
                Object.DestroyImmediate(momoObject);
                Object.DestroyImmediate(bulwarkObject);
                Object.DestroyImmediate(sproutObject);
            }
        }

        private static void SetSerializedHeroes(HeroSelectionManager manager, params MomoHeroController[] heroes)
        {
            SerializedObject serializedManager = new SerializedObject(manager);
            SerializedProperty heroesProperty = serializedManager.FindProperty("heroes");
            heroesProperty.arraySize = heroes.Length;
            for (int index = 0; index < heroes.Length; index++)
            {
                heroesProperty.GetArrayElementAtIndex(index).objectReferenceValue = heroes[index];
            }

            serializedManager.FindProperty("startingHero").objectReferenceValue = heroes.Length > 0 ? heroes[0] : null;
            serializedManager.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void SetSerializedString(Object target, string propertyName, string value)
        {
            SerializedObject serializedObject = new SerializedObject(target);
            serializedObject.FindProperty(propertyName).stringValue = value;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        private static bool TryResolveMovePoint(HeroSelectionManager manager, Ray ray, float yHeight, out Vector3 movePoint)
        {
            MethodInfo method = typeof(HeroSelectionManager).GetMethod("TryGetMovePoint", BindingFlags.Instance | BindingFlags.NonPublic);
            object[] arguments = { ray, yHeight, default(Vector3) };
            bool resolved = method != null && (bool)method.Invoke(manager, arguments);
            movePoint = resolved ? (Vector3)arguments[2] : default;
            return resolved;
        }

        private static void AssertCondition(bool condition, string message)
        {
            if (!condition)
            {
                throw new System.InvalidOperationException(message);
            }
        }

        private static void AssertApproximatelyEqual(Vector3 actual, Vector3 expected, string message)
        {
            if ((actual - expected).sqrMagnitude > 0.0001f)
            {
                throw new System.InvalidOperationException($"{message} Expected {expected}, got {actual}.");
            }
        }
    }
}
