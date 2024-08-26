using UnityEngine;

namespace Text
{
    public class Options
    {
        public static Options SmallText => new Options().Size(TextSize.Small);
        public static Options StrongText => new Options()
                .Align(TextAlignment.Center)
                .Size(TextSize.Large)
                .Style(FontStyle.Bold);
        public static Options TinyText => new Options().Size(TextSize.Tiny);

        public TextAlignment? AlignmentOption { get; private set; }
        public TextAnchor? AnchorOption { get; private set; }
        public Color? ColorOption { get; private set; }
        public byte SizeOption { get; private set; }
        public FontStyle? StyleOption { get; private set; }

        public Options() { }

        public Options(TextMesh source)
        {
            if (source == null)
            {
                Debug.LogWarning("Invalid source provided for Options; an empty Options instance will be supplied");
            }
            else
            {
                Align(source.alignment);
                Anchor(source.anchor);
                Color(source.color);
                Size((byte)source.fontSize);
                Style(source.fontStyle);
            }
        }

        public Options Align(TextAlignment alignment)
        {
            AlignmentOption = alignment;
            return this;
        }

        public Options Anchor(TextAnchor anchor)
        {
            AnchorOption = anchor;
            return this;
        }

        public Options Color(Color color)
        {
            ColorOption = color;
            return this;
        }

        private Options Size(byte size)
        {
            SizeOption = size;
            return this;
        }

        public Options Size(TextSize size)
        {
            SizeOption = (byte)size;
            return this;
        }

        public Options Style(FontStyle style)
        {
            StyleOption = style;
            return this;
        }

        public enum TextSize
        {
            Large = 50,
            Small = 35,
            Standard = 40,
            Tiny = 28
        }
    }
}
