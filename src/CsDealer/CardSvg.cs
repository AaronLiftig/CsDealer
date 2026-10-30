using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace CsDealer
{
    /// <summary>
    /// Renders card faces and backs as self-contained SVG images. No fonts, files or packages are required
    /// beyond a serif font for the indices, and the output contains no element ids, so many cards can be
    /// inlined into one HTML page safely.
    /// </summary>
    public static class CardSvg
    {
        /// <summary>File name used for the card back by <see cref="ExportAll"/>.</summary>
        public const string BackFileName = "back.svg";

        private const double W = 250;
        private const double H = 350;
        private const double Mid = H / 2;

        // Suit shapes drawn in a 100×100 box.
        private const string HeartPath =
            "M50 92C22 70 2 52 2 30C2 14 14 4 28 4C38 4 46 10 50 20C54 10 62 4 72 4C86 4 98 14 98 30C98 52 78 70 50 92Z";
        private const string DiamondPath =
            "M50 2C62 22 76 38 92 50C76 62 62 78 50 98C38 78 24 62 8 50C24 38 38 22 50 2Z";
        private const string SpadePath =
            "M50 4C64 24 96 40 96 62C96 76 85 84 74 84C64 84 57 79 53 72C54 84 58 92 68 98H32C42 92 46 84 47 72"
            + "C43 79 36 84 26 84C15 84 4 76 4 62C4 40 36 24 50 4Z";
        private const string ClubShape =
            "<circle cx=\"50\" cy=\"27\" r=\"20\"/><circle cx=\"27\" cy=\"58\" r=\"20\"/><circle cx=\"73\" cy=\"58\" r=\"20\"/>"
            + "<circle cx=\"50\" cy=\"52\" r=\"12\"/><path d=\"M46 56C46 78 42 90 32 98H68C58 90 54 78 54 56Z\"/>";

        // Pip positions for 2–10 as (column, row): column 0/1/2 = left/center/right, row 0 = top … 1 = bottom.
        private static readonly (int Col, double Row)[][] PipLayouts =
        {
            /* 2  */ new[] { (1, 0.0), (1, 1.0) },
            /* 3  */ new[] { (1, 0.0), (1, 0.5), (1, 1.0) },
            /* 4  */ new[] { (0, 0.0), (2, 0.0), (0, 1.0), (2, 1.0) },
            /* 5  */ new[] { (0, 0.0), (2, 0.0), (1, 0.5), (0, 1.0), (2, 1.0) },
            /* 6  */ new[] { (0, 0.0), (2, 0.0), (0, 0.5), (2, 0.5), (0, 1.0), (2, 1.0) },
            /* 7  */ new[] { (0, 0.0), (2, 0.0), (1, 0.25), (0, 0.5), (2, 0.5), (0, 1.0), (2, 1.0) },
            /* 8  */ new[] { (0, 0.0), (2, 0.0), (1, 0.25), (0, 0.5), (2, 0.5), (1, 0.75), (0, 1.0), (2, 1.0) },
            /* 9  */ new[] { (0, 0.0), (2, 0.0), (0, 1 / 3.0), (2, 1 / 3.0), (1, 0.5), (0, 2 / 3.0), (2, 2 / 3.0), (0, 1.0), (2, 1.0) },
            /* 10 */ new[] { (0, 0.0), (2, 0.0), (1, 1 / 6.0), (0, 1 / 3.0), (2, 1 / 3.0), (0, 2 / 3.0), (2, 2 / 3.0), (1, 5 / 6.0), (0, 1.0), (2, 1.0) },
        };

        private static readonly double[] PipColumns = { 82, 125, 168 };
        private const double PipTop = 82;
        private const double PipBottom = 268;

        // Rendering with default options is cached; the strings are immutable so sharing them is safe.
        private static readonly string?[] Cache = new string?[Card.KeyCount];
        private static string? s_backCache;

        /// <summary>Renders the face of a card.</summary>
        public static string Render(Card card, CardSvgOptions? options = null)
        {
            if (options is null)
            {
                return Cache[card.Key] ??= RenderFace(card, new CardSvgOptions());
            }

            return RenderFace(card, options);
        }

        /// <summary>Renders the back of a card.</summary>
        public static string RenderBack(CardSvgOptions? options = null)
        {
            if (options is null)
            {
                return s_backCache ??= RenderBackCore(new CardSvgOptions());
            }

            return RenderBackCore(options);
        }

        /// <summary>The file name for a card's image, e.g. <c>"AS.svg"</c>, <c>"10H.svg"</c> or <c>"JKR.svg"</c>.</summary>
        public static string FileName(Card card) => card.Abbrev + ".svg";

        /// <summary>Wraps SVG markup in a <c>data:</c> URI usable as an <c>&lt;img src&gt;</c> or CSS background.</summary>
        public static string ToDataUri(string svg) =>
            "data:image/svg+xml;base64," + Convert.ToBase64String(Encoding.UTF8.GetBytes(svg));

        /// <summary>
        /// Writes all 52 faces (plus a joker and the back, by default) to <paramref name="directory"/>,
        /// named by <see cref="FileName"/>. Returns the paths written.
        /// </summary>
        public static IReadOnlyList<string> ExportAll(string directory, CardSvgOptions? options = null,
            bool includeJoker = true, bool includeBack = true)
        {
            Directory.CreateDirectory(directory);
            var paths = new List<string>(54);

            void Write(string name, string svg)
            {
                string path = Path.Combine(directory, name);
                File.WriteAllText(path, svg);
                paths.Add(path);
            }

            foreach (Card card in CardTools.BuildCards(includeJoker ? 1 : 0))
            {
                Write(FileName(card), Render(card, options));
            }

            if (includeBack)
            {
                Write(BackFileName, RenderBack(options));
            }

            return paths;
        }

        //===============================================================================
        // Faces
        //===============================================================================

        private static string RenderFace(Card card, CardSvgOptions o)
        {
            var sb = new StringBuilder(4096);
            string color = card.IsJoker ? o.JokerColor : SuitColor(card.Suit!.Value, o);

            Open(sb, o, card.Name);
            sb.Append("<rect x=\"2\" y=\"2\" width=\"246\" height=\"346\" rx=\"16\" fill=\"").Append(Attr(o.FaceColor))
              .Append("\" stroke=\"").Append(Attr(o.BorderColor)).Append("\" stroke-width=\"2\"/>");

            if (card.IsJoker)
            {
                Joker(sb, color, o);
            }
            else
            {
                Suit suit = card.Suit!.Value;
                sb.Append("<g fill=\"").Append(Attr(color)).Append("\">");

                Corner(sb, card, suit, o, rotated: false);
                Corner(sb, card, suit, o, rotated: true);

                int v = (int)card.Value;

                if (v <= 10)
                {
                    foreach ((int col, double row) in PipLayouts[v - 2])
                    {
                        Pip(sb, suit, PipColumns[col], PipTop + row * (PipBottom - PipTop), 46, row > 0.5);
                    }
                }
                else if (card.Value == CardValue.Ace)
                {
                    Pip(sb, suit, W / 2, Mid, suit == Suit.Spades ? 130 : 110, false);
                }
                else
                {
                    Court(sb, card, suit, color, o);
                }

                sb.Append("</g>");
            }

            return sb.Append("</svg>").ToString();
        }

        private static void Corner(StringBuilder sb, Card card, Suit suit, CardSvgOptions o, bool rotated)
        {
            if (rotated)
            {
                sb.Append("<g transform=\"rotate(180 ").Append(F(W / 2)).Append(' ').Append(F(Mid)).Append(")\">");
            }

            bool ten = card.Value == CardValue.Ten;
            sb.Append("<text x=\"26\" y=\"50\" text-anchor=\"middle\" font-family=\"").Append(Attr(o.FontFamily))
              .Append("\" font-weight=\"bold\" font-size=\"").Append(ten ? "30" : "36").Append('"')
              .Append(ten ? " letter-spacing=\"-2\"" : "").Append('>').Append(card.ValueAbbrev).Append("</text>");
            Pip(sb, suit, 26, 72, 24, false);

            if (rotated)
            {
                sb.Append("</g>");
            }
        }

        private static void Court(StringBuilder sb, Card card, Suit suit, string color, CardSvgOptions o)
        {
            // An original, minimal court design: a tinted frame, a large initial and a pip above and below.
            sb.Append("<rect x=\"54\" y=\"48\" width=\"142\" height=\"254\" rx=\"10\" fill=\"").Append(Attr(color))
              .Append("\" fill-opacity=\"0.07\" stroke=\"").Append(Attr(color)).Append("\" stroke-width=\"2\"/>")
              .Append("<rect x=\"62\" y=\"56\" width=\"126\" height=\"238\" rx=\"6\" fill=\"none\" stroke=\"")
              .Append(Attr(color)).Append("\" stroke-opacity=\"0.45\" stroke-width=\"1\"/>");

            Crown(sb, card.Value);

            sb.Append("<text x=\"125\" y=\"218\" text-anchor=\"middle\" font-family=\"").Append(Attr(o.FontFamily))
              .Append("\" font-weight=\"bold\" font-size=\"110\">").Append(card.ValueAbbrev).Append("</text>");

            Pip(sb, suit, W / 2, 262, 36, true);
        }

        private static void Crown(StringBuilder sb, CardValue value)
        {
            // King: three-point crown. Queen: rounded tiara. Jack: feathered cap.
            string d = value switch
            {
                CardValue.King => "M95 112L101 80L113 98L125 72L137 98L149 80L155 112Z",
                CardValue.Queen => "M97 112C97 96 105 88 111 96C115 84 135 84 139 96C145 88 153 96 153 112Z",
                _ => "M98 112C98 96 110 86 125 86C140 86 152 96 152 112ZM140 90C150 76 162 74 166 78C158 80 150 86 146 94Z",
            };

            sb.Append("<path d=\"").Append(d).Append("\"/>");
            sb.Append("<rect x=\"95\" y=\"113\" width=\"60\" height=\"5\" rx=\"2\"/>");
        }

        private static void Joker(StringBuilder sb, string color, CardSvgOptions o)
        {
            sb.Append("<g fill=\"").Append(Attr(color)).Append("\" font-family=\"").Append(Attr(o.FontFamily))
              .Append("\" font-weight=\"bold\" text-anchor=\"middle\">");

            for (int corner = 0; corner < 2; corner++)
            {
                sb.Append(corner == 0 ? "<g>" : $"<g transform=\"rotate(180 {F(W / 2)} {F(Mid)})\">");

                for (int i = 0; i < 5; i++)
                {
                    sb.Append("<text x=\"24\" y=\"").Append(F(38 + i * 20)).Append("\" font-size=\"19\">")
                      .Append("JOKER"[i]).Append("</text>");
                }

                sb.Append("</g>");
            }

            // Five-pointed star.
            sb.Append("<path d=\"");

            for (int i = 0; i < 10; i++)
            {
                double radius = i % 2 == 0 ? 66 : 27;
                double angle = Math.PI * (i / 5.0 - 0.5);
                sb.Append(i == 0 ? 'M' : 'L').Append(F(W / 2 + radius * Math.Cos(angle))).Append(' ')
                  .Append(F(158 + radius * Math.Sin(angle)));
            }

            sb.Append("Z\"/>");
            sb.Append("<text x=\"125\" y=\"268\" font-size=\"34\" letter-spacing=\"4\">JOKER</text></g>");
        }

        //===============================================================================
        // Back
        //===============================================================================

        private static string RenderBackCore(CardSvgOptions o)
        {
            var sb = new StringBuilder(16384);
            Open(sb, o, "Card back");

            sb.Append("<rect x=\"2\" y=\"2\" width=\"246\" height=\"346\" rx=\"16\" fill=\"").Append(Attr(o.FaceColor))
              .Append("\" stroke=\"").Append(Attr(o.BorderColor)).Append("\" stroke-width=\"2\"/>")
              .Append("<rect x=\"12\" y=\"12\" width=\"226\" height=\"326\" rx=\"10\" fill=\"").Append(Attr(o.BackColor)).Append("\"/>")
              .Append("<rect x=\"20\" y=\"20\" width=\"210\" height=\"310\" rx=\"6\" fill=\"none\" stroke=\"#fff\" stroke-opacity=\"0.6\" stroke-width=\"2\"/>");

            // Diamond lattice as a single path.
            sb.Append("<path fill=\"#fff\" fill-opacity=\"0.16\" d=\"");

            for (double y = 34; y <= 316; y += 14)
            {
                for (double x = 34; x <= 216; x += 14)
                {
                    sb.Append('M').Append(F(x)).Append(' ').Append(F(y - 6))
                      .Append('l').Append("6 6-6 6-6-6z");
                }
            }

            sb.Append("\"/>");

            sb.Append("<circle cx=\"125\" cy=\"175\" r=\"42\" fill=\"").Append(Attr(o.BackColor))
              .Append("\" stroke=\"#fff\" stroke-width=\"3\"/><g fill=\"#fff\">");
            Pip(sb, Suit.Spades, W / 2, Mid, 48, false);
            sb.Append("</g>");

            return sb.Append("</svg>").ToString();
        }

        //===============================================================================
        // Helpers
        //===============================================================================

        private static void Open(StringBuilder sb, CardSvgOptions o, string label)
        {
            sb.Append("<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"").Append(F(o.Width))
              .Append("\" height=\"").Append(F(o.Width * 1.4))
              .Append("\" viewBox=\"0 0 250 350\" role=\"img\" aria-label=\"").Append(label).Append("\">")
              .Append("<title>").Append(label).Append("</title>");
        }

        private static void Pip(StringBuilder sb, Suit suit, double cx, double cy, double size, bool upsideDown)
        {
            sb.Append("<g transform=\"translate(").Append(F(cx - size / 2)).Append(' ').Append(F(cy - size / 2))
              .Append(") scale(").Append(F(size / 100)).Append(')')
              .Append(upsideDown ? " rotate(180 50 50)\">" : "\">");

            switch (suit)
            {
                case Suit.Hearts: sb.Append("<path d=\"").Append(HeartPath).Append("\"/>"); break;
                case Suit.Diamonds: sb.Append("<path d=\"").Append(DiamondPath).Append("\"/>"); break;
                case Suit.Spades: sb.Append("<path d=\"").Append(SpadePath).Append("\"/>"); break;
                default: sb.Append(ClubShape); break;
            }

            sb.Append("</g>");
        }

        private static string SuitColor(Suit suit, CardSvgOptions o) => suit switch
        {
            Suit.Hearts => o.RedColor,
            Suit.Diamonds => o.FourColor ? o.DiamondColor : o.RedColor,
            Suit.Clubs => o.FourColor ? o.ClubColor : o.BlackColor,
            _ => o.BlackColor,
        };

        private static string F(double d) => d.ToString("0.###", CultureInfo.InvariantCulture);

        // Option values are user-supplied; escape them so they cannot break the markup.
        private static string Attr(string value)
        {
            if (value.IndexOfAny(new[] { '"', '<', '>', '&' }) < 0)
            {
                return value;
            }

            return value.Replace("&", "&amp;").Replace("\"", "&quot;").Replace("<", "&lt;").Replace(">", "&gt;");
        }
    }
}
