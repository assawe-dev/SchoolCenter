# Codex Strict Rules

## Core Rule

Codex must not write, modify, delete, refactor, or generate any source code unless I explicitly send the word:

`execute`

If the word `execute` is not present in my instruction, Codex must only analyze, explain, plan, review, or ask clarifying questions.

## Project Understanding

Codex must first understand the existing project structure before suggesting any change.

Codex must follow the current architecture, naming conventions, folder structure, coding style, design patterns, and database access patterns already used in the project.

Codex must not introduce a new architecture, framework, library, pattern, abstraction, or folder structure unless explicitly requested.

## Design Consistency

All UI, components, pages, layouts, forms, tables, buttons, colors, spacing, validation messages, and user flows must follow the existing system design.

Codex must reuse existing components and styles whenever possible.

Codex must not redesign screens or change the user experience unless explicitly requested.

## Database Rules

Codex must understand the database schema before proposing any backend or data-related change.

Codex must respect existing table relationships, foreign keys, indexes, naming conventions, and constraints.

Codex must not create, rename, remove, or alter database tables, columns, indexes, procedures, views, or migrations unless explicitly requested with `execute`.

## Analysis Mode

When I ask for analysis, Codex must respond with:

1. What it understood.
2. Relevant files or modules.
3. Suggested approach.
4. Risks or unclear points.
5. Questions if needed.

Codex must not implement anything during analysis mode.

## Execution Mode

Codex may only write or modify code when I send the word:

`execute`

Before executing, Codex must briefly state:

1. Files that will be changed.
2. What will be changed.
3. Why the change is needed.

Then it may apply the change.

## Safety Rules

Codex must not:

* Delete files.
* Rename files.
* Change public APIs.
* Change database schema.
* Add dependencies.
* Rewrite large parts of the system.
* Modify authentication, authorization, payment, or security logic.

Unless I explicitly request it and include `execute`.

## Response Style

Codex responses must be concise, structured, and focused.

Codex must not assume missing requirements.

Codex must ask when something is unclear instead of guessing.

## Default Behavior

Default mode is always:

Analysis only.

Code writing is forbidden unless I explicitly send:

`execute`
