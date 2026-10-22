namespace TodoConsole.Tests;

public class TodoListTests
{
    [Fact]
    public void NewListIsEmpty()
    {
        Assert.Empty(new TodoList().Items);
    }

    [Fact]
    public void AddTrimsTitleAndAssignsSequentialIds()
    {
        var todos = new TodoList();

        var first = todos.Add("  Prepare demo  ");
        var second = todos.Add("Write tests");

        Assert.Equal(new TodoItem(1, "Prepare demo"), first);
        Assert.Equal(new TodoItem(2, "Write tests"), second);
        Assert.Equal(new[] { first, second }, todos.Items);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("\t")]
    public void AddRejectsBlankTitleWithoutChangingList(string title)
    {
        var todos = new TodoList();

        Assert.Throws<ArgumentException>(() => todos.Add(title));

        Assert.Empty(todos.Items);
        Assert.Equal(1, todos.Add("Valid title").Id);
    }

    [Fact]
    public void AddRejectsNullTitle()
    {
        var todos = new TodoList();

        Assert.Throws<ArgumentNullException>(() => todos.Add(null!));
        Assert.Empty(todos.Items);
    }

    [Fact]
    public void ListsDoNotShareState()
    {
        var first = new TodoList();
        var second = new TodoList();

        first.Add("Only in the first list");

        Assert.Empty(second.Items);
    }

    [Fact]
    public void ItemsCannotBeModifiedByCaller()
    {
        var todos = new TodoList();
        todos.Add("First");

        var collection = Assert.IsAssignableFrom<ICollection<TodoItem>>(todos.Items);

        Assert.True(collection.IsReadOnly);
        Assert.Throws<NotSupportedException>(collection.Clear);
        Assert.Single(todos.Items);
    }
}
