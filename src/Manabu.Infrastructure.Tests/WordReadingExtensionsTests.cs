using Manabu.Infrastructure.Contexts.Content.Phrases;

namespace Manabu.Infrastructure.Tests;

public class WordReadingExtensionsTests
{
    [Test]
    public void GetAigaaranai()
    {
        var originalForm = "上が上る";
        var conjugatedForm = "上が上らない";
        var reading = "あいがある";

        var result = WordReadingExtensions.Get(reading, originalForm, conjugatedForm);

        Assert.That(result, Is.EqualTo("あいがあらない"));
    }

    [Test]
    public void GetMijiかkunai()
    {
        var originalForm = "短い";
        var conjugatedForm = "短くない";
        var reading = "みじかい";

        var result = WordReadingExtensions.Get(reading, originalForm, conjugatedForm);

        Assert.That(result, Is.EqualTo("みじかくない"));
    }
}
