using System.Collections.Generic;
using UnityEngine;

public static class Text
{
    private static readonly Vector2 defaultPositionBelowDealtCards = new Vector2(0.7f, 0.5f);
    private static readonly Vector2 defaultPositionBelowDealtCardsPostScript = new Vector2(0.7f, 0.0f);
    private static readonly GameObject prefabTextMesh = Resources.Load<GameObject>("Prefab Text Mesh");

    public abstract class BaseText<T> where T : BaseText<T>, new()
    {
        private static T instance = new T();

        protected readonly Dictionary<int, Vector2> DefaultPositions = new Dictionary<int, Vector2>();
        protected readonly Dictionary<int, TextMesh> TextMeshes = new Dictionary<int, TextMesh>();
        protected readonly Dictionary<int, string> TextValues = new Dictionary<int, string>();

        public static void Display(int reference, params object[] formatStringParams)
        {
            TextMesh textMesh = instance.GetTextMeshFor(reference);
            textMesh.transform.Translate(instance.DefaultPositions[reference]);
            textMesh.text = string.Format(instance.TextValues[reference], formatStringParams);
            textMesh.gameObject.SetActive(true);
        }

        public static void DisplayTemporary(int reference, float seconds, params object[] formatStringParams)
        {
            Display(reference, formatStringParams);
            Timer.DelayThenInvoke(seconds, Hide, reference);
        }

        private TextMesh GetTextMeshFor(int reference)
        {
            TextMesh textMesh;
            try
            {
                textMesh = TextMeshes[reference];
            }
            catch (KeyNotFoundException)
            {
                textMesh = Object.Instantiate(prefabTextMesh).GetComponent<TextMesh>();
                TextMeshes.Add(reference, textMesh);
            }
            return textMesh;
        }

        public static void Hide(int reference)
        {
            try
            {
                instance.TextMeshes[reference].gameObject.SetActive(false);
            }
            catch (KeyNotFoundException)
            {
                Debug.LogWarningFormat("Attempted to Hide({0}) but no mesh found", reference);
            }
        }

        public static void TearDown()
        {
            foreach (int reference in instance.TextMeshes.Keys)
            {
                Object.Destroy(instance.TextMeshes[reference].gameObject);
            }
            instance.TextMeshes.Clear();
        }
    }

    public sealed class Battle : BaseText<Battle>
    {
    }

    public sealed class Healer : BaseText<Healer>
    {
    }

    public sealed class PlayerCreator : BaseText<PlayerCreator>
    {
        public PlayerCreator()
        {
            TextValues.Add((int)TextReference.CharacterSearch, "Finding a Character card...");
            TextValues.Add((int)TextReference.CharacterIdentified, "You are the {0}");
            TextValues.Add((int)TextReference.HPSearch, "Dealing cards for initial HP...");
            TextValues.Add((int)TextReference.HPCalculated, "You have {0} HP (15 + {1})");
            TextValues.Add((int)TextReference.DealHand, "Dealing your starting hand...");

            DefaultPositions.Add((int)TextReference.CharacterSearch, defaultPositionBelowDealtCards);
            DefaultPositions.Add((int)TextReference.CharacterIdentified, defaultPositionBelowDealtCardsPostScript);
            DefaultPositions.Add((int)TextReference.HPSearch, defaultPositionBelowDealtCards);
            DefaultPositions.Add((int)TextReference.HPCalculated, defaultPositionBelowDealtCardsPostScript);
            DefaultPositions.Add((int)TextReference.DealHand, defaultPositionBelowDealtCards);
        }

        public enum TextReference
        {
            CharacterSearch,
            CharacterIdentified,
            HPSearch,
            HPCalculated,
            DealHand
        }
    }

    public sealed class Trap : BaseText<Trap>
    {
    }

    public sealed class Treasure : BaseText<Treasure>
    {
    }
}
