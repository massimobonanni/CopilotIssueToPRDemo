# Demo GH-600

- App console C# con .NET 10 LTS, nullable e implicit usings abilitati.
- Mantieni la demo semplice: dati in memoria, nessun database o servizio esterno.
- Logica in `src/TodoConsole`, test xUnit in `tests/TodoConsole.Tests`.
- Mantieni `TodoApplication.Run(TextReader, TextWriter)` testabile senza console reale.
- Messaggi e README in italiano; valida gli input e segnala gli errori esplicitamente.
- Implementa solo la richiesta della issue; non aggiungere funzionalita speculative.
- Aggiorna i test e la documentazione direttamente interessati.
- Verifica con `dotnet build CopilotIssueDemo.sln --configuration Release`
  e `dotnet test CopilotIssueDemo.sln --configuration Release`.
- Apri una PR collegata alla issue, indicando modifiche e verifiche. Non fare merge
  e non abilitare auto-merge. Non modificare workflow o permessi salvo richiesta esplicita.
