using System.Collections.Generic;
using UnityEngine;

namespace Text
{
    public static class TextManager
    {
        private static readonly TextMesh prefabTextMesh = Resources.Load<TextMesh>("Prefab Text Mesh");
        private static readonly Dictionary<BaseExcerpt, TextMesh> textMeshes = new Dictionary<BaseExcerpt, TextMesh>();

        public static void DisplayText(BaseExcerpt excerpt)
        {
            DisplayText(excerpt, excerpt.defaultOptions, excerpt.defaultPosition);
        }

        public static void DisplayText(BaseExcerpt excerpt, Options options, Vector2 position)
        {
            TextMesh textMesh = GetTextMeshFor(excerpt);
            if (options != null) Format(textMesh, options);
            textMesh.transform.position = position;
            textMesh.text = excerpt.ToString();
            textMesh.gameObject.SetActive(true);
        }

        public static void DisplayTextAsExtension(BaseExcerpt excerpt, BaseExcerpt precedingText)
        {
            if (IsValidExtension(excerpt, precedingText))
            {
                TextMesh precedingTextMesh = textMeshes[precedingText];
                DisplayText(excerpt, new Options(precedingTextMesh), GetExtensionPosition(precedingTextMesh));
            }
            else
            {
                DisplayText(excerpt);
            }
        }

        private static void Format(TextMesh textMesh, Options options)
        {
            if (options.AlignmentOption != null)
            {
                textMesh.alignment = (TextAlignment)options.AlignmentOption;
                if (options.AnchorOption == null)
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
            if (options.AnchorOption != null) textMesh.anchor = (TextAnchor)options.AnchorOption;
            if (options.ColorOption != null) textMesh.color = (Color)options.ColorOption;
            if (options.SizeOption > 0) textMesh.fontSize = options.SizeOption;
            if (options.StyleOption > 0) textMesh.fontStyle = (FontStyle)options.StyleOption;
        }

        private static Vector2 GetExtensionPosition(TextMesh textBeingExtended)
        {
            Vector2 extensionPosition = textBeingExtended.transform.position;
            extensionPosition.y -= textBeingExtended.GetComponent<MeshRenderer>().bounds.size.y;
            // TODO: infer x/y extension from textMesh alignment & anchor, add x alternative to y logic above
            return extensionPosition;
        }

        private static TextMesh GetTextMeshFor(BaseExcerpt excerpt)
        {
            TextMesh textMesh;
            try
            {
                textMesh = textMeshes[excerpt];
            }
            catch (KeyNotFoundException)
            {
                textMesh = Object.Instantiate(prefabTextMesh).GetComponent<TextMesh>();
                textMeshes.Add(excerpt, textMesh);
            }
            return textMesh;
        }

        public static void HideText(params BaseExcerpt[] excerpts)
        {
            foreach (BaseExcerpt excerpt in excerpts)
            {
                try
                {
                    textMeshes[excerpt].gameObject.SetActive(false);
                }
                catch (KeyNotFoundException)
                {
                    Debug.LogWarning($"Attempted to Hide({excerpt}) but no mesh found");
                }
            }
        }

        private static bool IsValidExtension(BaseExcerpt excerpt, BaseExcerpt precedingText)
        {
            if (precedingText is null)
            {
                Debug.LogWarning($"null argument supplied as preceding text for {excerpt}");
                return false;
            }
            else if (textMeshes.ContainsKey(precedingText) == false)
            {
                Debug.LogError($"Excerpt ##{excerpt}## should extend ##{precedingText}## which is not currently displayed");
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void TearDownDisplayedText()
        {
            foreach (BaseExcerpt excerpt in textMeshes.Keys)
            {
                Object.Destroy(textMeshes[excerpt].gameObject);
            }
            textMeshes.Clear();
        }
    }
}
