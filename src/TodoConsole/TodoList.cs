namespace TodoConsole;

public sealed class TodoList
{
    private readonly List<TodoItem> items = [];

    public IReadOnlyList<TodoItem> Items => items.AsReadOnly();

    public TodoItem Add(string title)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        var item = new TodoItem(items.Count + 1, title.Trim());
        items.Add(item);
        return item;
    }
}
