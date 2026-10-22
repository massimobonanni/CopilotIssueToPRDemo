namespace TodoConsole;

public static class TodoApplication
{
    public static void Run(TextReader input, TextWriter output)
    {
        var todos = new TodoList();
        output.WriteLine("Lista attivita - demo GH-600");

        while (true)
        {
            output.WriteLine();
            output.WriteLine("1. Aggiungi attivita");
            output.WriteLine("2. Visualizza attivita");
            output.WriteLine("0. Esci");
            output.Write("Scelta: ");

            var choice = input.ReadLine();
            if (choice is null || choice.Trim() == "0")
            {
                output.WriteLine("Arrivederci!");
                return;
            }

            switch (choice.Trim())
            {
                case "1":
                    output.Write("Titolo: ");
                    var title = input.ReadLine();
                    if (title is null)
                    {
                        output.WriteLine("Arrivederci!");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(title))
                    {
                        output.WriteLine("Il titolo non puo essere vuoto.");
                        break;
                    }

                    var item = todos.Add(title);
                    output.WriteLine($"Attivita aggiunta: {item.Id}. {item.Title}");
                    break;

                case "2":
                    if (todos.Items.Count == 0)
                    {
                        output.WriteLine("Nessuna attivita presente.");
                        break;
                    }

                    foreach (var todo in todos.Items)
                    {
                        output.WriteLine($"{todo.Id}. {todo.Title}");
                    }
                    break;

                default:
                    output.WriteLine("Scelta non valida. Usa 1, 2 oppure 0.");
                    break;
            }
        }
    }
}
