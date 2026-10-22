namespace TodoConsole.Tests;

public class TodoApplicationTests
{
    [Fact]
    public void EmptyListShowsExplicitMessage()
    {
        Assert.Contains("Nessuna attivita presente.", Run("2\n0\n"));
    }

    [Fact]
    public void AddedItemsAreDisplayedInOrder()
    {
        var output = Run("1\n  Preparare demo  \n1\nScrivere test\n2\n0\n");

        Assert.Contains("Attivita aggiunta: 1. Preparare demo", output);
        Assert.Contains("Attivita aggiunta: 2. Scrivere test", output);
        Assert.Contains($"1. Preparare demo{Environment.NewLine}2. Scrivere test", output);
        Assert.Contains("Arrivederci!", output);
    }

    [Fact]
    public void BlankTitleIsRejectedAndMenuContinues()
    {
        var output = Run("1\n   \n2\n0\n");

        Assert.Contains("Il titolo non puo essere vuoto.", output);
        Assert.Contains("Nessuna attivita presente.", output);
        Assert.DoesNotContain("Attivita aggiunta:", output);
    }

    [Theory]
    [InlineData("3")]
    [InlineData("abc")]
    [InlineData("")]
    public void UnknownChoiceIsReportedAndMenuContinues(string choice)
    {
        var output = Run($"{choice}\n2\n0\n");

        Assert.Contains("Scelta non valida.", output);
        Assert.Contains("Nessuna attivita presente.", output);
    }

    [Theory]
    [InlineData("")]
    [InlineData("1\n")]
    [InlineData("0\n")]
    [InlineData(" 0 \n")]
    public void ExitOrEndOfInputTerminatesCleanly(string input)
    {
        Assert.EndsWith($"Arrivederci!{Environment.NewLine}", Run(input));
    }

    [Fact]
    public void NewRunStartsWithEmptyMemory()
    {
        Run("1\nTemporanea\n0\n");

        Assert.Contains("Nessuna attivita presente.", Run("2\n0\n"));
    }

    private static string Run(string text)
    {
        using var input = new StringReader(text);
        using var output = new StringWriter();
        TodoApplication.Run(input, output);
        return output.ToString();
    }
}
