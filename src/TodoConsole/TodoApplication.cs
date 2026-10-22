namespace TodoConsole;

public static class TodoApplication
{
    public static void Run(TextReader input, TextWriter output)
    {
        var todos = new TodoList();
        output.WriteLine("Todo List - GH-600 Demo");

        while (true)
        {
            output.WriteLine();
            output.WriteLine("1. Add task");
            output.WriteLine("2. List tasks");
            output.WriteLine("0. Exit");
            output.Write("Choice: ");

            var choice = input.ReadLine();
            if (choice is null || choice.Trim() == "0")
            {
                output.WriteLine("Goodbye!");
                return;
            }

            switch (choice.Trim())
            {
                case "1":
                    output.Write("Title: ");
                    var title = input.ReadLine();
                    if (title is null)
                    {
                        output.WriteLine("Goodbye!");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(title))
                    {
                        output.WriteLine("Title cannot be empty.");
                        break;
                    }

                    var item = todos.Add(title);
                    output.WriteLine($"Task added: {item.Id}. {item.Title}");
                    break;

                case "2":
                    if (todos.Items.Count == 0)
                    {
                        output.WriteLine("No tasks available.");
                        break;
                    }

                    foreach (var todo in todos.Items)
                    {
                        output.WriteLine($"{todo.Id}. {todo.Title}");
                    }
                    break;

                default:
                    output.WriteLine("Invalid choice. Use 1, 2, or 0.");
                    break;
            }
        }
    }
}
