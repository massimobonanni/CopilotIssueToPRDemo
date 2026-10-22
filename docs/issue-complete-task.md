# Complete task

## Goal

Extend the console demo to mark an existing task as completed, identified by its ID.
This feature does not exist yet: this is not a bug fix.

## Acceptance criteria

- [ ] The menu offers `3. Completa attivita` ("Complete task"), consistent with
      the existing Italian UI, while preserving add, list, and exit.
- [ ] New tasks are initially incomplete.
- [ ] Option 3 prompts for a positive integer ID and completes only the matching task.
- [ ] The list displays the ID, title, and status, using `[ ]` for open tasks
      and `[x]` for completed tasks.
- [ ] Nonexistent IDs, zero, negative values, blank input, and nonnumeric input
      produce an explicit message without unhandled exceptions or changes to other tasks.
- [ ] Completing the same task again leaves its state unchanged and displays
      a message indicating that it was already completed.
- [ ] EOF while entering the ID exits cleanly.
- [ ] State remains in memory only: a new run starts with an empty list.
- [ ] xUnit tests cover successful completion, initial state, repeated completion,
      nonexistent IDs, invalid input, EOF, and the menu. Existing tests remain green;
      update only output expectations affected by intentional changes.
- [ ] `dotnet build CopilotIssueDemo.sln --configuration Release` and
      `dotnet test CopilotIssueDemo.sln --configuration Release` succeed.
- [ ] The README is updated with the new command and an example.

## Manual verification

Start the app, add `Prepare demo` and `Write tests`, then complete task ID 1.
Display the list: `1. [x] Prepare demo` and `2. [ ] Write tests`.
Complete ID 1 again; also try `999`, `0`, `-1`, `abc`, and a blank ID.
Messages must be explicit and the list must remain unchanged. Exit with 0.

## Scope and deliverables

Do not add persistence, deletion, external services, or unnecessary packages.
Keep task logic separate from the console, following the existing structure.
Open a pull request linked to this issue with a summary and the checks performed.
Do not merge or enable auto-merge.
