namespace TodoConsole.Tests;

public class TodoApplicationTests
{
    [Fact]
    public void EmptyListShowsExplicitMessage()
    {
        Assert.Contains("No tasks available.", Run("2\n0\n"));
    }

    [Fact]
    public void AddedItemsAreDisplayedInOrder()
    {
        var output = Run("1\n  Prepare demo  \n1\nWrite tests\n2\n0\n");

        Assert.Contains("Task added: 1. Prepare demo", output);
        Assert.Contains("Task added: 2. Write tests", output);
        Assert.Contains($"1. Prepare demo{Environment.NewLine}2. Write tests", output);
        Assert.Contains("Goodbye!", output);
    }

    [Fact]
    public void BlankTitleIsRejectedAndMenuContinues()
    {
        var output = Run("1\n   \n2\n0\n");

        Assert.Contains("Title cannot be empty.", output);
        Assert.Contains("No tasks available.", output);
        Assert.DoesNotContain("Task added:", output);
    }

    [Theory]
    [InlineData("3")]
    [InlineData("abc")]
    [InlineData("")]
    public void UnknownChoiceIsReportedAndMenuContinues(string choice)
    {
        var output = Run($"{choice}\n2\n0\n");

        Assert.Contains("Invalid choice.", output);
        Assert.Contains("No tasks available.", output);
    }

    [Theory]
    [InlineData("")]
    [InlineData("1\n")]
    [InlineData("0\n")]
    [InlineData(" 0 \n")]
    public void ExitOrEndOfInputTerminatesCleanly(string input)
    {
        Assert.EndsWith($"Goodbye!{Environment.NewLine}", Run(input));
    }

    [Fact]
    public void NewRunStartsWithEmptyMemory()
    {
        Run("1\nTemporary\n0\n");

        Assert.Contains("No tasks available.", Run("2\n0\n"));
    }

    [Fact]
    public void MenuIsDisplayedInEnglish()
    {
        var output = Run("0\n");

        Assert.Contains("Todo List - GH-600 Demo", output);
        Assert.Contains("1. Add task", output);
        Assert.Contains("2. List tasks", output);
        Assert.Contains("0. Exit", output);
        Assert.Contains("Choice: ", output);
    }

    private static string Run(string text)
    {
        using var input = new StringReader(text);
        using var output = new StringWriter();
        TodoApplication.Run(input, output);
        return output.ToString();
    }
}
