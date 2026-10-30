using NUnit.Framework;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

namespace CsDealer.Tests
{
    public class TestCardSvg
    {
        [Test]
        public void EveryCardIsWellFormedSvg()
        {
            foreach (Card card in CardTools.BuildCards(1))
            {
                XDocument doc = XDocument.Parse(card.ToSvg());

                Assert.That(doc.Root!.Name.LocalName, Is.EqualTo("svg"));
                Assert.That(doc.Root.Attribute("aria-label")!.Value, Is.EqualTo(card.Name));
            }

            Assert.That(XDocument.Parse(CardSvg.RenderBack()).Root!.Name.LocalName, Is.EqualTo("svg"));
        }

        [Test]
        public void DefaultRenderIsCached()
        {
            Card card = Card.Parse("QH");

            Assert.That(ReferenceEquals(CardSvg.Render(card), CardSvg.Render(card)), Is.True);
        }

        [Test]
        public void OptionsAreApplied()
        {
            var options = new CardSvgOptions { Width = 100, FourColor = true, DiamondColor = "#0000ff" };
            string svg = CardSvg.Render(Card.Parse("2D"), options);

            Assert.That(svg, Does.Contain("width=\"100\""));
            Assert.That(svg, Does.Contain("height=\"140\""));
            Assert.That(svg, Does.Contain("#0000ff"));
        }

        [Test]
        public void OptionValuesAreEscaped()
        {
            var options = new CardSvgOptions { RedColor = "red\"/><script>" };

            Assert.DoesNotThrow(() => XDocument.Parse(CardSvg.Render(Card.Parse("AH"), options)));
        }

        [Test]
        public void NumbersUseInvariantCulture()
        {
            CultureInfo original = Thread.CurrentThread.CurrentCulture;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
                string svg = CardSvg.Render(Card.Parse("9C"), new CardSvgOptions { Width = 62.5 });

                Assert.That(svg, Does.Contain("width=\"62.5\""));
                Assert.That(svg, Does.Not.Contain("62,5"));
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = original;
            }
        }

        [Test]
        public void ExportAllWritesFiles()
        {
            string dir = Path.Combine(Path.GetTempPath(), "csdealer-" + Guid.NewGuid().ToString("N"));

            try
            {
                var paths = CardSvg.ExportAll(dir);

                Assert.That(paths, Has.Count.EqualTo(54));
                Assert.That(File.Exists(Path.Combine(dir, "AS.svg")), Is.True);
                Assert.That(File.Exists(Path.Combine(dir, "10H.svg")), Is.True);
                Assert.That(File.Exists(Path.Combine(dir, "JKR.svg")), Is.True);
                Assert.That(File.Exists(Path.Combine(dir, CardSvg.BackFileName)), Is.True);
                Assert.That(paths.Select(Path.GetFileName).Distinct().Count(), Is.EqualTo(54));
            }
            finally
            {
                Directory.Delete(dir, recursive: true);
            }
        }

        [Test]
        public void DataUri()
        {
            Assert.That(CardSvg.ToDataUri("<svg/>"), Is.EqualTo("data:image/svg+xml;base64,PHN2Zy8+"));
        }
    }
}
