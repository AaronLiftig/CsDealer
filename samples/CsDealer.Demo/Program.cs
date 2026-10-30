using System;
using System.IO;
using System.Linq;
using System.Text;
using CsDealer;

// Usage:
//   dotnet run --project samples/CsDealer.Demo                  deal two poker hands
//   dotnet run --project samples/CsDealer.Demo -- export <dir>  write every card image (SVG) plus preview.html

if (args.Length >= 1 && args[0] == "export")
{
    string dir = args.Length >= 2 ? args[1] : "cards";
    var paths = CardSvg.ExportAll(dir);
    File.WriteAllText(Path.Combine(dir, "preview.html"), Preview());
    Console.WriteLine($"Wrote {paths.Count} card images and preview.html to {Path.GetFullPath(dir)}");
    return;
}

var deck = new Deck();
deck.Shuffle();

Stack alice = deck.Deal(5);
Stack bob = deck.Deal(5);
alice.Sort(Ranks.Poker);
bob.Sort(Ranks.Poker);

Console.WriteLine("Alice:  " + string.Join(", ", alice.Select(c => c.Abbrev)));
Console.WriteLine("Bob:    " + string.Join(", ", bob.Select(c => c.Abbrev)));
Console.WriteLine($"Cards left in the deck: {deck.Count}");

int aliceAces = alice.Find("Ace").Count;
Console.WriteLine($"Alice holds {aliceAces} ace(s); her high card is the {alice.Peek()}.");

static string Preview()
{
    var html = new StringBuilder("<!doctype html><meta charset=\"utf-8\"><title>CsDealer cards</title>"
        + "<style>body{margin:24px;background:#2d6a4f;display:flex;flex-wrap:wrap;gap:10px}"
        + "img{width:120px;height:168px}</style>");

    foreach (Card card in CardTools.BuildCards(1))
    {
        html.Append($"<img src=\"{CardSvg.FileName(card)}\" alt=\"{card.Name}\">");
    }

    return html.Append($"<img src=\"{CardSvg.BackFileName}\" alt=\"Card back\">").ToString();
}
