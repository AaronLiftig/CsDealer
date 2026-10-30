namespace CsDealer
{
    /// <summary>Appearance settings for <see cref="CardSvg"/>. Colors are any CSS color.</summary>
    public sealed class CardSvgOptions
    {
        /// <summary>Rendered width in pixels; the height is always 1.4× (poker-size 2.5" × 3.5").</summary>
        public double Width { get; set; } = 250;

        /// <summary>Use a four-color deck: blue diamonds and green clubs.</summary>
        public bool FourColor { get; set; }

        /// <summary>Hearts (and diamonds unless <see cref="FourColor"/>).</summary>
        public string RedColor { get; set; } = "#c8102e";

        /// <summary>Spades (and clubs unless <see cref="FourColor"/>).</summary>
        public string BlackColor { get; set; } = "#1b1b1f";

        /// <summary>Diamonds in a four-color deck.</summary>
        public string DiamondColor { get; set; } = "#1f5fbf";

        /// <summary>Clubs in a four-color deck.</summary>
        public string ClubColor { get; set; } = "#1d7a3a";

        /// <summary>Joker artwork.</summary>
        public string JokerColor { get; set; } = "#6b2fa3";

        /// <summary>Card face background.</summary>
        public string FaceColor { get; set; } = "#ffffff";

        /// <summary>Card outline.</summary>
        public string BorderColor { get; set; } = "#b9b9c0";

        /// <summary>Card back background.</summary>
        public string BackColor { get; set; } = "#1f3a68";

        /// <summary>Font used for indices and face letters.</summary>
        public string FontFamily { get; set; } = "Georgia, 'Times New Roman', serif";
    }
}
