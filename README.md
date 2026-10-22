# CopilotIssueToPRDemo - GH-600 demo

A small C# console application demonstrating the flow **new issue -> automatic assignment
to Copilot coding/cloud agent -> implementation with tests -> pull request -> human review**.

The initial version lets you add and list tasks in memory, then exit.
The **complete task feature is not implemented**: it is the exercise to assign
to the agent, not an intentional bug. No database, network connection, or credentials
are required to run the app. Console messages are in Italian; the exercise issue text is in English.

## Prerequisites

- **.NET 10 LTS SDK**: created and verified with **10.0.402**.
  `global.json` requires this version or a later .NET 10 feature band
  (`latestFeature`); preview SDKs are excluded. The runtime alone is not sufficient.
- Git for the local repository; NuGet access for the initial test dependency restore.
- For the cloud demo: a GitHub repository with Issues and Actions enabled, write
  access, a Copilot plan/access that includes the cloud agent, and the agent enabled
  for both the user and repository under personal/organization policies.
  Cloud execution may consume Copilot usage allowances and Actions minutes.

## Run locally

From the repository directory, in PowerShell:

```powershell
dotnet --version
dotnet restore CopilotIssueDemo.sln
dotnet build CopilotIssueDemo.sln --configuration Release --no-restore
dotnet test CopilotIssueDemo.sln --configuration Release --no-build
dotnet run --project src\TodoConsole --configuration Release --no-build
```

Menu: `1` adds a task with a nonblank title, `2` lists task IDs and titles,
and `0` exits. Leading and trailing whitespace is trimmed. Invalid choices are
reported without stopping the program. End of input (EOF) exits cleanly.
All data is lost when the app closes.

Example input: `1`, `Prepare demo`, `1`, `Write tests`, `2`, `0`,
each on a separate line. The list displays:

```text
1. Prepare demo
2. Write tests
```

## Project structure

| Path | Purpose |
| --- | --- |
| `src/TodoConsole` | Model, in-memory task list, and menu with injectable input/output |
| `tests/TodoConsole.Tests` | xUnit tests for task logic and console flows |
| `docs/issue-complete-task.md` | Ready-to-copy title and body for the exercise issue |
| `.github/copilot-instructions.md` | Concise agent instructions |
| `.github/workflows/assign-copilot.yml` | Automatic assignment of new issues to Copilot |
| `.github/workflows/ci.yml` | Build and tests on pushes to main and pull requests |

## Set up the GitHub demo (manual steps)

Start from the [demo repository](https://github.com/massimobonanni/CopilotIssueToPRDemo)
and create your own fork. Run the exercise in **your fork**, not in the original
repository. Configure Actions, Copilot access, and secrets separately for your fork;
the original repository's secrets are not copied.

1. Open the demo repository on GitHub, select **Fork -> Create a new fork**, and
   choose your personal account or an organization where you can configure the
   repository. Keep the name `CopilotIssueToPRDemo` and copy the `main` branch.
   Clone **your fork** from PowerShell, replacing `YOUR-OWNER` with its owner:

   ```powershell
   git clone https://github.com/YOUR-OWNER/CopilotIssueToPRDemo.git
   Set-Location .\CopilotIssueToPRDemo
   git remote -v
   ```

   `origin` should point to your fork. No `git init`, initial commit, or first push
   is needed: the fork already contains the demo files and commit history.
   Use the commands in **Run locally** to build, test, and try the initial app.

2. In **your fork**, enable **Issues** under **Settings -> General -> Features**
   if needed. Open the **Actions** tab and enable workflows if GitHub asks you to;
   workflows may be disabled initially in a fork. Verify that repository or
   organization settings allow the actions used by the demo and that `main` is
   the **default branch**.
   The `issues` workflow must be present on the fork's default branch **before**
   opening the issue; it is included when you fork `main`. An existing or reopened
   issue does not trigger `issues: opened`; to repeat the demo, open a new issue,
   or rerun a failed workflow after fixing the configuration.
3. Enable Copilot cloud agent for the user and **your fork**. Organization policies
   may require additional approvals. Access granted to the original repository
   does not automatically configure your fork.
4. The user starting the agent configures a **fine-grained personal access token**
   with a short expiration, restricted to **your fork only**, not the original
   repository. Repository permissions:

   | Permission | Access |
   | --- | --- |
   | Metadata | Read |
   | Actions | Read and write |
   | Contents | Read and write |
   | Issues | Read and write |
   | Pull requests | Read and write |

   The token owner must have Copilot access and write access to the repository.
   Complete any required organization approvals for the token.
   In **your fork**, save its value under **Settings -> Secrets and variables -> Actions ->
   New repository secret**, named **`COPILOT_ASSIGN_TOKEN`**.
   The default `GITHUB_TOKEN` is not sufficient to start the cloud agent: a user
   token is required, not an installation token. Never put the token in files,
   issues, logs, or chat. This project does not include or create any tokens.

## End-to-end demonstration

1. Show the initial console app and passing tests: task completion is the missing feature.
2. On GitHub, in **your fork**, open a new issue titled **Complete task** and
   copy the body from `docs/issue-complete-task.md` (omit the first `# ...` line).
   The author must be an `OWNER`, `MEMBER`, or `COLLABORATOR`.
   **Every new issue from these authors triggers assignment**, not just the
   exercise issue: use a dedicated training repository.
3. In Actions, watch **Assegna nuove issue a Copilot** ("Assign new issues to Copilot").
   The workflow uses `actions/github-script@v8`, without checking out code, with
   `permissions: {}`. It sends a REST POST to
   `/repos/{owner}/{repo}/issues/{issue_number}/assignees` with
   `copilot-swe-agent[bot]`, the current repository, and its default branch in
   `agent_assignment`. Issue titles and bodies are not interpolated as JavaScript
   code. Issues from other authors skip the job.
4. Verify that Copilot is assigned to the issue and open its session logs.
   A successful assignment run confirms assignment only, not completion of the
   task. If the bot is missing from the REST response, the workflow explicitly
   fails even if the API returned a successful HTTP status.
5. Wait for the PR linked to the issue. Show the model change, menu option 3,
   tests, and documentation. The agent is instructed to open a PR **without
   merging or enabling auto-merge**; the PR may initially be a draft.
6. If GitHub requires approval to run workflows on the agent's PR, a maintainer
   must inspect the changes before authorizing the run.
   Check **Build e test** ("Build and tests"), review the diff, and try the new menu
   locally. Approval and any eventual merge remain human decisions.

## Troubleshooting and limitations

- No run: check the default branch, published workflow, Actions settings, and
  whether a **new** issue was opened.
- Job skipped: the author does not have one of the three allowed associations.
- Missing secret: the workflow fails before making the REST request.
- HTTP 401/403/404/422 or missing bot: check token expiration and repository scope,
  all permissions listed above, organization approvals, Copilot access,
  agent enablement, and service availability/usage allowances.
- The token grants significant permissions: restrict it to the training repository,
  never print it, and revoke it after the course. Always review PR code and workflows.
- Checking the author association does not sandbox issue content.
  Do not include untrusted instructions or secrets in material given to the agent.
- The agent assignment API is in public preview and its behavior may change.
  The remote flow requires setup and verification in your own account; local tests
  do not prove that Copilot is available or has opened a PR.

Official reference: [Using the cloud agent via the API and REST issue assignment](https://docs.github.com/en/copilot/how-tos/use-copilot-agents/cloud-agent/use-cloud-agent-via-the-api).
