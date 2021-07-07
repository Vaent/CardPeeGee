using System.Collections.Generic;
using UnityEngine;

public static class Text
{
    private static readonly Vector2 defaultPositionAnnounceEncounter = new Vector2(7.5f, 2.5f);
    private static readonly Vector2 defaultPositionBelowDealtCards = new Vector2(0.7f, 0.5f);
    private static readonly Vector2 defaultPositionBelowDealtCardsPostScript = new Vector2(0.7f, 0.0f);
    private static readonly TextMesh prefabTextMesh = Resources.Load<TextMesh>("Prefab Text Mesh");

    public class Options
    {
        public TextAlignment? TextAlignment { get; private set; }
        public TextAnchor? TextAnchor { get; private set; }
        public Color? TextColor { get; private set; }
        public Vector2? TextPosition { get; private set; }
        public byte TextSize { get; private set; }
        public FontStyle? TextStyle { get; private set; }

        public Options() { }

        public Options(TextMesh source)
        {
            if (source == null)
            {
                Debug.LogWarning("Invalid source provided for Options; an empty Options instance will be supplied");
            }
            else
            {
                if (!prefabTextMesh.alignment.Equals(source.alignment)) Align(source.alignment);
                if (!prefabTextMesh.anchor.Equals(source.anchor)) Anchor(source.anchor);
                if (!prefabTextMesh.color.Equals(source.color)) Color(source.color);
                if (!prefabTextMesh.fontSize.Equals(source.fontSize)) Size((byte)source.fontSize);
                if (!prefabTextMesh.fontStyle.Equals(source.fontStyle)) Style(source.fontStyle);
            }
        }

        public Options Align(TextAlignment alignment)
        {
            TextAlignment = alignment;
            return this;
        }

        public Options Anchor(TextAnchor anchor)
        {
            TextAnchor = anchor;
            return this;
        }

        public Options Color(Color color)
        {
            TextColor = color;
            return this;
        }

        public Options Position(Vector2 position)
        {
            TextPosition = position;
            return this;
        }

        public Options Size(byte size)
        {
            TextSize = size;
            return this;
        }

        public Options Size(TextSize size)
        {
            TextSize = (byte)size;
            return this;
        }

        public Options Style(FontStyle style)
        {
            TextStyle = style;
            return this;
        }
    }

    public enum TextSize
    {
        Large = 50,
        Small = 35,
        Standard = 40
    }

    // BaseText and its implementations are defined below

    public abstract class BaseText<T> where T : BaseText<T>, new()
    {
        private static T instance = new T();

        protected readonly Dictionary<int, Vector2> DefaultPositions = new Dictionary<int, Vector2>();
        protected readonly Dictionary<int, TextMesh> TextMeshes = new Dictionary<int, TextMesh>();
        protected readonly Dictionary<int, string> TextValues = new Dictionary<int, string>();

        public static void Display(int reference, params object[] formatStringParams)
        {
            DisplayFormatted(null, reference, formatStringParams);
        }

        public static void DisplayAsExtension(int newReference, int referenceBeingExtended, params object[] formatStringParams)
        {
            if (instance.TextMeshes.ContainsKey(referenceBeingExtended))
            {
                TextMesh textMesh = instance.TextMeshes[referenceBeingExtended];
                Vector2 extensionPosition = textMesh.transform.position;
                extensionPosition.y -= textMesh.GetComponent<MeshRenderer>().bounds.size.y;
                // TODO: infer x/y extension from textMesh alignment & anchor, add x alternative to y logic above
                Options options = new Options(textMesh).Position(extensionPosition);
                DisplayFormatted(options, newReference, formatStringParams);
            }
            else
            {
                Debug.LogError("Attempted to extend text which has not been displayed");
                Display(newReference, formatStringParams);
            }
        }

        public static void DisplayFormatted(Options options, int reference, params object[] formatStringParams)
        {
            TextMesh textMesh = instance.GetTextMeshFor(reference);
            textMesh.transform.Translate(instance.DefaultPositions[reference]);
            textMesh.text = string.Format(instance.TextValues[reference], formatStringParams);
            if (options != null) Format(textMesh, options);
            textMesh.gameObject.SetActive(true);
        }

        private static void Format(TextMesh textMesh, Options options)
        {
            if (options.TextAlignment != null)
            {
                textMesh.alignment = (TextAlignment)options.TextAlignment;
                if (options.TextAnchor == null)
                {
                    switch (textMesh.alignment)
                    {
                        case TextAlignment.Center:
                            textMesh.anchor = TextAnchor.UpperCenter;
                            break;
                        case TextAlignment.Left:
                            textMesh.anchor = TextAnchor.UpperLeft;
                            break;
                        case TextAlignment.Right:
                            textMesh.anchor = TextAnchor.UpperRight;
                            break;
                    }
                }
            }
            if (options.TextAnchor != null) textMesh.anchor = (TextAnchor)options.TextAnchor;
            if (options.TextColor != null) textMesh.color = (Color)options.TextColor;
            if (options.TextPosition != null) textMesh.transform.position = (Vector2)options.TextPosition;
            if (options.TextSize > 0) textMesh.fontSize = options.TextSize;
            if (options.TextStyle > 0) textMesh.fontStyle = (FontStyle)options.TextStyle;
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
                Debug.LogWarning($"Attempted to Hide({reference}) but no mesh found");
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
        public Battle()
        {
            TextValues.Add((int)TextReference.Announce, "a MONSTER\nattacks you");

            DefaultPositions.Add((int)TextReference.Announce, defaultPositionAnnounceEncounter);
        }

        public enum TextReference
        {
            Announce
        }
    }

    public sealed class Healer : BaseText<Healer>
    {
        public Healer()
        {
            TextValues.Add((int)TextReference.Announce, "you meet a\nHEALER");

            DefaultPositions.Add((int)TextReference.Announce, defaultPositionAnnounceEncounter);
        }

        public enum TextReference
        {
            Announce
        }
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
        public Trap()
        {
            TextValues.Add((int)TextReference.Announce, "it's a\nTRAP!");
            TextValues.Add((int)TextReference.Stats, "Difficulty: {0}\nDamage: {1}");
            TextValues.Add((int)TextReference.AttemptEvade, "You try to\navoid it...");

            DefaultPositions.Add((int)TextReference.Announce, defaultPositionAnnounceEncounter);
            DefaultPositions.Add((int)TextReference.Stats, new Vector2(7.5f, 0));
            DefaultPositions.Add((int)TextReference.AttemptEvade, defaultPositionAnnounceEncounter);
        }

        public enum TextReference
        {
            Announce,
            Stats,
            AttemptEvade
        }
    }

    public sealed class Treasure : BaseText<Treasure>
    {
        public Treasure()
        {
            TextValues.Add((int)TextReference.Announce, "you find a\nTREASURE\nCHEST");

            DefaultPositions.Add((int)TextReference.Announce, defaultPositionAnnounceEncounter);
        }

        public enum TextReference
        {
            Announce
        }
    }
}
