namespace CombatDicesTeam.Engine.Ui.Tests;

[TestFixture]
public class TextParserTests
{
    [Test]
    public void TestParseText()
    {
        var inputText = "text <style=color1,ani2>styled text</style>";
        var result = TextParser.ParseText(inputText);

        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("text ", result[0].Value);
        Assert.AreEqual(new RichTextNodeStyle(null, null), result[0].Style);

        Assert.AreEqual("styled text", result[1].Value);
        Assert.AreEqual(new RichTextNodeStyle(1, 2), result[1].Style);
    }

    [Test]
    public void TestParseText2()
    {
        var inputText = "text\n<style=color1,ani2>styled text</style>";
        var result = TextParser.ParseText(inputText);

        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("text\n", result[0].Value);
        Assert.AreEqual(new RichTextNodeStyle(null, null), result[0].Style);

        Assert.AreEqual("styled text", result[1].Value);
        Assert.AreEqual(new RichTextNodeStyle(1, 2), result[1].Style);
    }
}